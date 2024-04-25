namespace SammysBBQ.Auth
{
    public class UserService
    {
        private List<User>? users;
        private static UserService? instance = null;

        private UserService() { }

        public static UserService GetInstance()
        {
            if (instance == null) { instance = new UserService(); }
            return instance;
        }

        public void AddUser(User newUser)
        {
            if (users == null) { users = new List<User>(); }
            users.Add(newUser);
        }

        public void SetUsers(List<User> _users) { users = _users; }


        public User? GetByUsername(string username)
        {
            if (users == null) return null;
            return users.FirstOrDefault(x => x.Username == username);
        }

    }
}