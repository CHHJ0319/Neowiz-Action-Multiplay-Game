using Unity.Netcode;
using UnityEngine;

namespace Actor.Item
{
    public class BulletBox : ItemBox
    {
        private int ammo = 10;

        public override void Use(Actor.Player.PlayerController player)
        {
            if (IsServer)
            {
                if (player.Role.Value == (int)Data.PlayerRole.Shooter)
                {
                    AddAmmo(player);
                }
            }
        }

        private void AddAmmo(Actor.Player.PlayerController player)
        {
            player.AddAmmo(ammo);

            DespawnSelf();
        }
    }
}