using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services
{
    public class RoleAssigner
    {
        public List<int> AssignRandomRoles(int playerCount)
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
    }
}