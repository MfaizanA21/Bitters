using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer;
using System.Data;
namespace DataAccessLayer
{
	public class DalProducts
	{
		public static List<ModelProducts> GetProducts()
		{
			SqlConnection con = DBHelper.GetConnection();
			SqlCommand cmd = new SqlCommand("select * from Products", con);
			con.Open();
			SqlDataReader reader = cmd.ExecuteReader();
			List<ModelProducts> listProducts = new List<ModelProducts>();	
			while (reader.Read())
			{
				ModelProducts product = new ModelProducts();
				product.ProductId = Convert.ToInt32(reader["ProductID"]);
                product.Name = reader["Name"].ToString();
				product.Price = Convert.ToInt32(reader["Price"]);
				product.Description = reader["Description"].ToString();
				product.Category = reader["Category"].ToString();
		//		product.ImageUrl = reader["URL"].ToString();
				listProducts.Add(product);
			}
			con.Close();
			return listProducts;
		}

		public static void SaveProducts(ModelProducts products)
		{
			SqlConnection con = DBHelper.GetConnection();
			con.Open();
			SqlCommand cmd = new SqlCommand("SP_saveProducts", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Name",products.Name);
			cmd.Parameters.AddWithValue("@Category",products.Category);
			cmd.Parameters.AddWithValue("@Price",products.Price);
			cmd.Parameters.AddWithValue("@Description",products.Description);
			cmd.ExecuteNonQuery();
			con.Close();
		}
        public static void DeleteProducts(int id)
        {
            SqlConnection con = DBHelper.GetConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_DeleteProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", id);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void UpdateProducts(int id, string name, int price, string description)
        {
            SqlConnection con = DBHelper.GetConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_UpdateProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", id);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
