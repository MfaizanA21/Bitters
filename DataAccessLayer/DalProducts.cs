using System.Data.SqlClient;
using ModelLayer;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

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
                //product.ExpirationDate = reader["ExpirationDate"].ToString();
                if (reader["ExpirationDate"] != DBNull.Value)
                {
                    product.ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);
                }
                else
                {
                    // Handle NULL case, you can set a default value or leave it as DateTime.MinValue
                    product.ExpirationDate = DateTime.MinValue;
                }
                listProducts.Add(product);
			}
			con.Close();
			return listProducts;
		}
		public static void SaveProducts(ModelProducts products)
		{
			SqlConnection con = DBHelper.GetConnection();
			con.Open();
			SqlCommand cmd = new SqlCommand("AddProduct", con);
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
		public static void UploadImages(string FileName, string path, byte[] ImageData, int ProductID)
		{
			SqlConnection con = DBHelper.GetConnection();
			con.Open();
			SqlCommand cmd = new SqlCommand("SP_Image", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@FileName", FileName);
			cmd.Parameters.AddWithValue("@path", path);
			cmd.Parameters.AddWithValue("@ImageData", ImageData);
			cmd.Parameters.AddWithValue("@ProductID", ProductID);
			cmd.ExecuteNonQuery();
			con.Close();
		}
		public static List<ModelImage> GetImages()
		{
			SqlConnection con = DBHelper.GetConnection();
			SqlCommand cmd = new SqlCommand("select * from imagePath", con);
			con.Open();
			SqlDataReader reader = cmd.ExecuteReader();
			List<ModelImage> images = new List<ModelImage>();
			while (reader.Read())
			{
				ModelImage image = new ModelImage();
                if (!reader.IsDBNull(reader.GetOrdinal("ImageData")))
                {
                    image.imageData = reader.GetSqlBinary(reader.GetOrdinal("ImageData")).Value;
                }
                //image.imageData = JsonSerializer.SerializeToUtf8Bytes(reader["ImageData"]);
                image.FileName = reader["FileName"] != DBNull.Value ? reader["FileName"].ToString() : null;
                image.FilePath = reader["FilePath"] != DBNull.Value ? reader["FilePath"].ToString() : null;
				image.ProductID = Convert.ToInt32(reader["ProductId"]); 
                images.Add(image);
			}
			con.Close();
			return images;
		}
		public static int GetLatestProductId()
		{
			int latestProductId = 0;
			SqlConnection con = DBHelper.GetConnection();
			con.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT MAX(ProductID) AS latestProduct FROM Products", con))
            {
                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    latestProductId = Convert.ToInt32(result);
                }
            }
			con.Close();
            return latestProductId;
		}
		public static void MoveExpiredProduct()
		{
			DateTime currentDateTime = DateTime.Now;
			List<ModelProducts> expiredProducts = GetProducts().Where(p => p.ExpirationDate < currentDateTime).ToList();
			foreach (var expiredProduct in expiredProducts) {
				InsertIntoExpiredProducts(expiredProduct);
				DeleteProducts(expiredProduct.ProductId);
			}
		}
		private static void InsertIntoExpiredProducts(ModelProducts expiredProduct)
        {
            SqlConnection con = DBHelper.GetConnection();
			con.Open();
			SqlCommand cmd = new SqlCommand("sp_InsertIntoExpiredProducts", con);
			cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProductId", expiredProduct.ProductId);
            cmd.Parameters.AddWithValue("@Name", expiredProduct.Name);
            cmd.Parameters.AddWithValue("@Price", expiredProduct.Price);
            cmd.Parameters.AddWithValue("@Description", expiredProduct.Description);
            cmd.Parameters.AddWithValue("@Category", expiredProduct.Category);
            cmd.Parameters.AddWithValue("@ExpirationDate", expiredProduct.ExpirationDate);

			cmd.ExecuteNonQuery();
			con.Close();
        }

		//public static void DeleteExpiredProduct(int productId)
		//{
		//	SqlConnection con = DBHelper.GetConnection();
		//	con.Open();
		//	SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductId = @ProductId", con);
		//	cmd.Parameters.AddWithValue("@ProductId", productId);
		//	cmd.ExecuteNonQuery ();
		//	con.Close();
		//}
    }
}
