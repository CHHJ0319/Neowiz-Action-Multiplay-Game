using System;


namespace Events 
{
    public static class PlayerFieldEvents
    {
        public static event Action<float> OnEnemyCollided;
        public static event Action<float> OnHPChanged;

        public static void HandleEnemyCollision(float damage)
        {
            OnEnemyCollided?.Invoke(damage);
        }

        public static void UpdateHPBar(float hpRate)
        {
            OnHPChanged?.Invoke(hpRate);
        }
    }
}
