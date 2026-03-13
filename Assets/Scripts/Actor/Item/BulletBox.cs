namespace Actor.Item
{
    public class BulletBox : ItemBox
    {
        private int ammo = 10;

        public override void Use()
        {
            AddAmmo();
        }

        private void AddAmmo()
        {
            PlayerController player = transform.root.GetComponent<PlayerController>();

            if (player != null)
            {
                //player.AddAmmo(ammo);
                Destroy(gameObject);
            }
        }
    }
}