using Newtonsoft.Json;
using System.Collections.Generic;

namespace Data
{
    [System.Serializable]
    public class TeamData
    {
        public string teamName;
        public List<string> memberNames;
        public int finalRound;
        public int totalScore;
    }
}