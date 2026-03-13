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

        public Data.ElementType Type { get; private set; } = Data.ElementType.Red;

        private void Awake()
        {
            Data.ElementType type = GetRandomElementType();
            SetType(type);
        }

        private void SetType(Data.ElementType type)
        {
            Type = type;

            if (ringTarget != null)
            {
                switch (type)
                {
                    case Data.ElementType.Red:
                        ringTarget.material = typeRed;
                        break;
                    case Data.ElementType.Green:
                        ringTarget.material = typeGreen;
                        break;
                    case Data.ElementType.Blue:
                        ringTarget.material = typeBlue;
                        break;
                }
            }
        }

        public void TakeDamage(Data.ElementType type)
        {
            if(Type == type)
            {
                DestroySelf();
            }
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public Data.ElementType GetRandomElementType()
        {
            int randomIndex = Random.Range(0, 3);

            if(randomIndex == 0)
            {
                return Data.ElementType.Red;
            }
            else if (randomIndex == 1)
            {
                return Data.ElementType.Green;
            }
            else if (randomIndex == 2)
            {
                return Data.ElementType.Blue;
            }
            else
            {
                return Data.ElementType.Red;
            }
        }
    }
}
