using UnityEngine;

namespace Actor.Item
{
    public class BulletBox : ItemBox
    {
        private int ammo = 10;

        private bool isCollected = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isCollected) return;

            if (other.CompareTag("Player"))
            {
                Actor.Player.PlayerController player = other.gameObject.GetComponent<Actor.Player.PlayerController>();

                if(player.PlayerType.Value.role == Data.PlayerRole.Shooter)
                {
                    //AddAmmo(player);
                }
            }
        }

        private void AddAmmo(Actor.Player.PlayerController player)
        {
            player.AddAmmo(ammo);

            isCollected = true;
            Destroy(gameObject);
        }
    }
}