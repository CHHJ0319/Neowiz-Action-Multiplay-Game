using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Utils
{
    public static class NetworkService
    {
        public static string JoinCode { get; private set; }

        private const int m_MaxConnections = 4;

        public static async void InitializeUnityServicesAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                var playerID = AuthenticationService.Instance.PlayerId;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static IEnumerator ConfigureTransportAndStartNgoAsHost()
        {
            var serverRelayUtilityTask = Utils.NetworkService.AllocateRelayServerAndGetJoinCode(m_MaxConnections);
            while (!serverRelayUtilityTask.IsCompleted)
            {
                yield return null;
            }
            if (serverRelayUtilityTask.IsFaulted)
            {
                Debug.LogError("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
                yield break;
            }

            var relayServerData = serverRelayUtilityTask.Result;
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            yield return new WaitForSeconds(2.0f);
        }

        public static IEnumerator ConfigureTransportAndStartNgoAsClient(string relayJoinCode)
        {
            var clientRelayUtilityTask = Utils.NetworkService.JoinRelayServerFromJoinCode(relayJoinCode);

            while (!clientRelayUtilityTask.IsCompleted)
            {
                yield return null;
            }

            if (clientRelayUtilityTask.IsFaulted)
            {
                Debug.LogError("Exception thrown when attempting to connect to Relay Server. Exception: " + clientRelayUtilityTask.Exception.Message);
                yield break;
            }

            var relayServerData = clientRelayUtilityTask.Result;
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            yield return new WaitForSeconds(2.0f);
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
    }
}
