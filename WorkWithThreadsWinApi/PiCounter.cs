namespace WorkWithThreadsWinApi
{
    internal class PiCounter
    {
        public void CalculatePi()
        {
            double i = 0;
            double pi = 0;
            while (true)
            {
                pi += (1.0 / (1.0 + 2.0 * i)) * ((i % 2 == 0) ? 1 : (-1));
                i++;
            }
        }
    }
}
