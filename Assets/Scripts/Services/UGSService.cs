using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Services
{
    public static class UGSService
    {
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
    }
}
