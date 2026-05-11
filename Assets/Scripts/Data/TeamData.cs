using Newtonsoft.Json;
using System.Collections.Generic;

namespace Data
{
    [System.Serializable]
    public class TeamData
    {
        [JsonProperty("teamName")]
        public string teamName;

        [JsonProperty("memberNames")]
        public List<string> memberNames;

        [JsonProperty("finalRound")]
        public int finalRound;

        [JsonProperty("totalScore")]
        public int totalScore;
    }
}