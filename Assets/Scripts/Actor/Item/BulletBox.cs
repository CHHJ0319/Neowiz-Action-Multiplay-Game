using Unity.Netcode;
using UnityEngine;

namespace Actor.Item
{
    public class BulletBox : ItemBox
    {
        private int ammo = 10;

        private bool isCollected = false;

        public override void Use(Actor.Player.PlayerController player)
        {
            if (isCollected) return;

            AddAmmo(player);
        }

        private void AddAmmo(Actor.Player.PlayerController player)
        {
            player.AddAmmo(ammo);

            isCollected = true;
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}