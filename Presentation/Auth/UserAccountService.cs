using DataAccessLayer;
using ModelLayer;
using Presentation.Auth;
using System.Data.SqlClient;

namespace Presentation.Auth
{
    public class UserAccountService
    {

        public static UserAccount GetUsers(string userName, string password)
        {
            try
            {
                SqlConnection con = DBHelper.GetConnection();
                SqlCommand cmd = new SqlCommand("SP_AuthUsers", con);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                UserAccount userAccount = new UserAccount();
                while(reader.Read())
                {
                    userAccount.UserName = reader["UserName"].ToString();
                    userAccount.Password = reader["Password"].ToString();
                    userAccount.Role = reader["Role"].ToString();
                }
                reader.Close();
                con.Close();
                return userAccount;
            }
            catch
            {
                return new UserAccount();
            }
            
        }
        

    }
}


