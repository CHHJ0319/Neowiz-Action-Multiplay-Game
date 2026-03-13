using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public MeshRenderer ringTarget;

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

        private void Awake()
        {
            SetType(Data.ElementType.Red);
            SetType(Data.ElementType.Green);

            CalculateTotalHP();
            UpdateHPTexts();
        }

        private void SetType(Data.ElementType type)
        {
            Types.Add(type);

            SetRingTarget(type);
            SetHPtexts(type);
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
    }
}
