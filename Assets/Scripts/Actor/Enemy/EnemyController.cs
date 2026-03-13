using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Type Material")]
        public MeshRenderer ringTarget;

        public Material typeRed;
        public Material typeGreen;
        public Material typeBlue;

        private float HP;

        public EnemyType Type { get; private set; } = EnemyType.Red;

        private void Awake()
        {
            EnemyType type = GetRandomEnemyType();
            SetType(type);
        }

        private void SetType(EnemyType type)
        {
            Type = type;

            if (ringTarget != null)
            {
                switch (type)
                {
                    case EnemyType.Red:
                        ringTarget.material = typeRed;
                        break;
                    case EnemyType.Green:
                        ringTarget.material = typeGreen;
                        break;
                    case EnemyType.Blue:
                        ringTarget.material = typeBlue;
                        break;
                }
            }
        }

        public void TakeDamage()
        {
            DestroySelf();
        }

        public void DestroySelf()
        {
            //Destroy(gameObject);
        }

        public EnemyType GetRandomEnemyType()
        {
            int randomIndex = Random.Range(0, 3);

            if(randomIndex == 0)
            {
                return EnemyType.Red;
            }
            else if (randomIndex == 1)
            {
                return EnemyType.Green;
            }
            else if (randomIndex == 2)
            {
                return EnemyType.Blue;
            }
            else
            {
                return EnemyType.Red;
            }
        }
    }
}
