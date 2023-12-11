using System.Data.SqlClient;

namespace Presentation.Auth
{
    public class UserAccountService
    {
        private List<UserAccount> _users;
        public UserAccountService()
        {
            _users = new List<UserAccount>
            {
                new UserAccount {UserName = "admin",Password="admin", Role = "Administrator"},
                new UserAccount {UserName = "Faizan",Password="Asghar", Role = "Administrator"},
                new UserAccount {UserName = "user",Password="user", Role = "User"},
                new UserAccount {UserName = "Hashaam",Password="Awan", Role = "User"},
            };
            }
        public UserAccount? GetByUserAccount(string userName)
        {
            return _users.FirstOrDefault(x=> x.UserName == userName);
        }

       // public static List<UserAccount> GetUsers()
       // {
            //SqlConnection con = DataAccessLayer.DBHelper.GetConnection();
            //SqlCommand cmd = new SqlCommand("Select * from Users", con);


            //return _users;
        //}

    }
}
