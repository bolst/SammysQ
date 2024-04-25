namespace SammysBBQ.Auth
{
    public class UserFactory
    {
        private static UserFactory instance = null;
        private UserFactory() { }
        public static UserFactory GetInstance()
        {
            if (instance == null) instance = new UserFactory();
            return instance;
        }
        public async Task<List<User>> Users()
        {
            return await AuthApi.GetUsers();
        }
    }
}