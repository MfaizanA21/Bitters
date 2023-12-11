using System.Data.SqlClient;

namespace DataAccessLayer
{
	public class DBHelper
	{
		public static SqlConnection GetConnection()
		{
			SqlConnection con = new SqlConnection("Data Source=DESKTOP-3D3UIN2\\SQLEXPRESS01;Initial Catalog=Bitter;Integrated Security=True");
			return con;
		}
	}
}