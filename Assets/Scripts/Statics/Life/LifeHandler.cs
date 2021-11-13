namespace Asteroids.Statics
{
    public static class LifeHandler
    {
        static int _CurPlayerLifes = 5;

        public delegate void OnPlayerLifeChangeDelegate(int curLife);
        public static OnPlayerLifeChangeDelegate OnPlayerLifeChange;

        public static void ReduceLife(int amount)
        {
            if (_CurPlayerLifes > 0)
                _CurPlayerLifes -= amount;

            OnPlayerLifeChange?.Invoke(_CurPlayerLifes);
        }
    }   
}