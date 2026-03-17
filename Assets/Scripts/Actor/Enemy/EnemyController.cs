using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public MeshRenderer ringTarget;

        public float damage = 10;
        public float speed = 5;

        [Header("Type Materials")]
        public Material typeRed;
        public Material typeGreen;
        public Material typeBlue;

        [Header("HP")]
        public int hpRed = 1;
        public int hpGreen = 1;
        public int hpBlue = 1;
        public TextMeshPro hpRedText;
        public TextMeshPro hpGreenText;
        public TextMeshPro hpBlueText;
        private int totalHP;

        public List<Data.ElementType> Types = new List<Data.ElementType>();

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            SetType();

            CalculateTotalHP();
            UpdateHPTexts();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
            }
            else if (other.CompareTag("Barricade"))
            {
                Events.PlayerFieldEvents.HandleEnemyCollision(damage);
            }

            Destroy(gameObject);
        }

        private void SetType()
        {
            Data.ElementType type = GetRandomElementType();
            Types.Add(type);

            SetRingTarget(type);
            SetHPtexts(type);
        }

        private void SetHealthByType(Data.ElementType type, int amount)
        {
            switch (type)
            {
                case Data.ElementType.Red:
                    hpRed = amount; 
                    hpRedText.gameObject.SetActive(true);
                    break;

                case Data.ElementType.Green:
                    hpGreen = amount;
                    hpGreenText.gameObject.SetActive(true);
                    break;

                case Data.ElementType.Blue:
                    hpBlue = amount;
                    hpBlueText.gameObject.SetActive(true);
                    break;
            }
        }

        public void SetMultipleLives()
        {
            foreach (Data.ElementType myType in Types)
            {
                SetHealthByType(myType, 10); 
            }

            CalculateTotalHP();
            UpdateHPTexts();
        }

        public void SetMultiType()
        {
            int randomIndex = Random.Range(1, 3);

            switch (randomIndex)
            {
                case 1:
                    SetType();
                    SetType();
                    break;
                case 2:
                    SetType();
                    SetType();
                    SetType();
                    break;
            }
        }

        public void TakeDamage(Data.ElementType type)
        {
            if(Types.Contains(type))
            {
                switch (type)
                {
                    case Data.ElementType.Red:
                        if(hpRed > 0)
                        {
                            hpRed--;
                        }
                        break;
                    case Data.ElementType.Green:
                        if (hpGreen > 0)
                        {
                            hpGreen--;
                        }
                        break;
                    case Data.ElementType.Blue:
                        if (hpBlue > 0)
                        {
                            hpBlue--;
                        }
                        break;
                }

                CalculateTotalHP();
                UpdateHPTexts();

                if(totalHP <= 0)
                {
                    DestroySelf();
                }
            }
        }

        public void Launch(Vector3 direction)
        {
            if (rb != null)
            {
                rb.linearVelocity = direction * speed;
            }
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }

        #region HPText
        private void SetHPtexts(Data.ElementType type)
        {
            if (type == Data.ElementType.Red)
            {
                hpRedText.gameObject.SetActive(true);
            }
            if (type == Data.ElementType.Green)
            {
                hpGreenText.gameObject.SetActive(true);
            }
            if (type == Data.ElementType.Blue)
            {
                hpBlueText.gameObject.SetActive(true);
            }
        }

        private void UpdateHPTexts()
        {
            hpRedText.text = "" + hpRed;
            hpGreenText.text = "" + hpGreen;
            hpBlueText.text = "" + hpBlue;
        }
        #endregion

        private void SetRingTarget(Data.ElementType type)
        {
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

        private void CalculateTotalHP()
        {
            totalHP = 0;

            if (Types.Contains(Data.ElementType.Red))
            {
                totalHP += hpRed;
            }

            if (Types.Contains(Data.ElementType.Green))
            {
                totalHP += hpGreen;
            }

            if (Types.Contains(Data.ElementType.Blue))
            {
                totalHP += hpBlue;
            }
        }

        private Data.ElementType GetRandomElementType()
        {
            int randomIndex = Random.Range(0, 3);

            if (randomIndex == 0)
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
