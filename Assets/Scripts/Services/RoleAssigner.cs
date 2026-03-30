using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services
{
    public class RoleAssigner
    {
        public List<Data.PlayerType> AssignRandomRoles(int playerCount)
        {
            if (playerCount <= 0)
            {
                return new List<Data.PlayerType>();
            }

            List<int> indexs = Enumerable.Range(0, playerCount).ToList();
            for (int i = 0; i < indexs.Count; i++)
            {
                int temp = indexs[i];
                int randomIndex = Random.Range(i, indexs.Count);
                indexs[i] = indexs[randomIndex];
                indexs[randomIndex] = temp;
            }

            List<Data.PlayerType> roles = new List<Data.PlayerType>(playerCount);
            foreach(int i in indexs)
            {
                if(i == 0)
                {
                    roles.Add(Data.PlayerType.Supporter);
                }
                else
                {
                    roles.Add(Data.PlayerType.Shooter);
                }
            }

            return roles;
        }
    }
}