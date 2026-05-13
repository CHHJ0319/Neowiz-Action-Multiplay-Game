using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Services
{
    public static class NetworkService
    {
        public static string JoinCode { get; private set; }

        private const int m_MaxConnections = 4;

        public static IEnumerator ConfigureTransportAndStartNgoAsHost()
        {
            var serverRelayUtilityTask = AllocateRelayServerAndGetJoinCode(m_MaxConnections);
            while (!serverRelayUtilityTask.IsCompleted)
            {
                yield return null;
            }
            if (serverRelayUtilityTask.IsFaulted)
            {
                Debug.LogError("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
                string message = Utils.LocaleLoader.GetConnectionMessage("ERR_RELAY_ALLOCATION_FAILED");
                UI.CanvasController.Instance.ShowCommonPopup(message);
                yield break;
            }

            var relayServerData = serverRelayUtilityTask.Result;
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(relayServerData);

            bool isSuccess = NetworkManager.Singleton.StartHost();

            if (!isSuccess)
            {
                Debug.LogError("NGO Host failed to start. Check your NetworkManager settings.");
                string message = Utils.LocaleLoader.GetConnectionMessage("ERR_HOST_START_FAILED");
                UI.CanvasController.Instance.ShowCommonPopup(message);
                yield break;
            }

            NetworkManager.Singleton.OnServerStopped += (bool isHost) => {
                Debug.LogWarning("Host has been stopped. Returning to Main Menu...");
                string message = Utils.LocaleLoader.GetConnectionMessage("ERR_SERVER_STOPPED_UNEXPECTEDLY");
                UI.CanvasController.Instance.ShowCommonPopup(message);
            };

            yield return new WaitForSeconds(2.0f);
        }

        public static IEnumerator ConfigureTransportAndStartNgoAsClient(string relayJoinCode, string password)
        {
            var clientRelayUtilityTask = JoinRelayServerFromJoinCode(relayJoinCode);

            while (!clientRelayUtilityTask.IsCompleted)
            {
                yield return null;
            }

            if (clientRelayUtilityTask.IsFaulted)
            {
                Debug.LogError("Exception thrown when attempting to connect to Relay Server. Exception: " + clientRelayUtilityTask.Exception.Message);
                string message = Utils.LocaleLoader.GetConnectionMessage("ERR_RELAY_JOIN_FAILED");
                UI.CanvasController.Instance.ShowCommonPopup(message);
                yield break;
            }

            var relayServerData = clientRelayUtilityTask.Result;
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            byte[] payload = Encoding.ASCII.GetBytes(password);
            NetworkManager.Singleton.NetworkConfig.ConnectionData = payload;

            if (!NetworkManager.Singleton.StartClient())
            {
                Debug.LogError("NGO Client failed to start.");
                string message = Utils.LocaleLoader.GetConnectionMessage("ERR_CLIENT_START_FAILED");
                UI.CanvasController.Instance.ShowCommonPopup(message);
                yield break;
            }

            bool isTimedOut = false;
            float timeoutDuration = 10f;
            float timer = 0f;

            while (!NetworkManager.Singleton.IsConnectedClient)
            {
                timer += Time.deltaTime;
                if (timer > timeoutDuration)
                {
                    isTimedOut = true;
                    break;
                }
                yield return null;
            }

            if (isTimedOut)
            {
                Debug.LogError("Connection Timed Out or Password Incorrect.");
                string message = Utils.LocaleLoader.GetConnectionMessage("ERR_CONNECTION_TIMEOUT");
                UI.CanvasController.Instance.ShowCommonPopup(message);
                NetworkManager.Singleton.Shutdown();
                yield break;
            }
        }

        public static void ShutdownNetwork()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.Shutdown();
                JoinCode = null;
            }
        }

        private static async Task<RelayServerData> AllocateRelayServerAndGetJoinCode(int maxConnections, string region = null)
        {
            Allocation allocation;
            try
            {
                allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
            }
            catch (Exception e)
            {
                Debug.LogError($"Relay create allocation request failed {e.Message}");
                throw;
            }

            Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"server: {allocation.AllocationId}");

            try
            {
                JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                Events.UIEvents.SetJoinCode(JoinCode);
            }
            catch
            {
                Debug.LogError("Relay create join code request failed");
                throw;
            }

            return allocation.ToRelayServerData("dtls");
        }

        private static async Task<RelayServerData> JoinRelayServerFromJoinCode(string joinCode)
        {
            JoinAllocation allocation;
            try
            {
                joinCode = joinCode.Trim().ToUpper();

                allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            }
            catch
            {
                Debug.LogError("Relay create join code request failed");
                throw;
            }

            Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
            Debug.Log($"client: {allocation.AllocationId}");

            return allocation.ToRelayServerData("dtls");
        }

        public static bool IsHost()
        {
            return NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost;
        }
    }
}
