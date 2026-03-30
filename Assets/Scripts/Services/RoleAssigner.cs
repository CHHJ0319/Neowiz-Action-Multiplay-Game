using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services
{
    public static class RoleAssigner
    {
        public static Data.PlayerType[] AssignRandomRoles(int playerCount)
        {
            if (playerCount <= 0)
            {
                return new Data.PlayerType[0];
            }

            List<int> roles = Enumerable.Range(0, playerCount).ToList();
            for (int i = 0; i < roles.Count; i++)
            {
                int temp = roles[i];
                int randomIndex = Random.Range(i, roles.Count);
                roles[i] = roles[randomIndex];
                roles[randomIndex] = temp;
            }

            List<int> colors = Enumerable.Range(0, playerCount - 1).ToList();
            for (int i = 0; i < colors.Count; i++)
            {
                int temp = colors[i];
                int randomIndex = Random.Range(i, colors.Count);
                colors[i] = colors[randomIndex];
                colors[randomIndex] = temp;
            }

            int colorIndex = 0;
            Data.PlayerType[] types = new Data.PlayerType[playerCount];
            for(int i = 0; i < playerCount; i++)
            {
                if(roles[i] == 0)
                {
                    types[i] = (new Data.PlayerType { role = Data.PlayerRole.Supporter });
                }
                else
                {
                    types[i] = (new Data.PlayerType { role = Data.PlayerRole.Shooter, color = (Data.ElementType)colors[colorIndex] });
                    colorIndex++;
                }
            }

            return types;
        }
    }
}