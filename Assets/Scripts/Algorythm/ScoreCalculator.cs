namespace Algorythm
{
    public static class ScoreCalculator
    {
        private const long ROUND_MULTIPLIER = 1_000_000L;

        public static long GetCombinedScore(int finalRound, int totalScore)
        {
            return (long)finalRound * ROUND_MULTIPLIER + totalScore;
        }
    }
}