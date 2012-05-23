using System;

namespace TankTop.IntegrationTests
{
    public static class Extensions
    {
        public static void Try<T>(this T obj, Action<T> action)
        {
            try
            {
                action(obj);
            }
            catch (Exception) {}
        }

        public static void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception) {}
        }
    }
}