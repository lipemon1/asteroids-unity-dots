namespace Asteroids.Statics
{
    public static class ScoreHandler
    {
        static int _CurScore;

        public delegate void OnScoreChangedDelegate(int newScore);
        public static OnScoreChangedDelegate OnScoreChanged;

        public static void AddScore(int scoreAmount)
        {
            _CurScore += scoreAmount;
            OnScoreChanged?.Invoke(_CurScore);
        }
    }   
}