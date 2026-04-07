using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services
{
    public static class RoleAssigner
    {
        public static List<int> AssignRandomRoles(int playerCount)
        {
            if (playerCount <= 0)
            {
                return new List<int>();
            }

            List<int> roles = Enumerable.Range(0, playerCount).ToList();
            for (int i = 0; i < roles.Count; i++)
            {
                int temp = roles[i];
                int randomIndex = Random.Range(i, roles.Count);
                roles[i] = roles[randomIndex];
                roles[randomIndex] = temp;
            }

            return roles;
        }

        public static List<int> AssignRandomColors(int playerCount)
        {
            if (playerCount <= 0)
            {
                return new List<int>();
            }

            List<int> colors = Enumerable.Range(0, playerCount - 1).ToList();
            for (int i = 0; i < colors.Count; i++)
            {
                int temp = colors[i];
                int randomIndex = Random.Range(i, colors.Count);
                colors[i] = colors[randomIndex];
                colors[randomIndex] = temp;
            }

            return colors;
        }
    }
}