using System.Linq;

namespace IdentityServer.Services
{
    public class UserService
    {
        public string Login(string name, string pwd)
        {
            var user = Config.GetUsers().FirstOrDefault(u => u.Username == name && u.Password == pwd);
            return user?.SubjectId ?? "";
        }
    }
}