namespace SammysBBQ.Data
{
    public abstract class AbsSingleton<T> where T : new()
    {
        static AbsSingleton() { }

        private static readonly T instance = new T();

        public static T Instance { get { return instance; } }
    }
}
