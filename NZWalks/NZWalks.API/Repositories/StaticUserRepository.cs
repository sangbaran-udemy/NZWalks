using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
            /*new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Read Only",
                LastName = "User",
                UserEmail = "readonly@user.com",
                UserName = "readonly@user.com",
                UserPassword= "Readonly@user",
                Roles = new List<string>() {"reader"}
            },
            new User() 
            {
                Id = Guid.NewGuid(),
                FirstName = "Read Write",
                LastName = "User",
                UserEmail = "readwrite@user.com",
                UserName = "readwrite@user.com",
                UserPassword= "Readwrite@user",
                Roles = new List<string>() {"reader", "writer"}
            }*/
        };
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = Users.Find(x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase) 
            && x.UserPassword.Equals(password));

            return user;
        }
    }
}
