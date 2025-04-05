using FeedBuf.Catagory;
using Microsoft.Data.SqlClient;

namespace FeedBuf
{
    public class DAL
    {
        string connectionString = "Data Source=LAPTOP-T4RLVBV6;Initial Catalog=FeedBackBuf;Integrated Security=True;Trust Server Certificate=True";
        List<Category> categories = new List<Category>();

        public List<Category> FillCategoryFromDatabase() 
        {
            categories.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM Category";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var type = reader[1].ToString();

                            categories.Add(new Category(id, type));
                        }
                    }
                    return categories;
                }
            }
        }
        public List<Category> FillCategoryFromDatabase(Category category)
        {
            categories.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Category (Type) VALUES (@Type) SELECT @@IDENTITY";
                    command.Parameters.AddWithValue("@Type", category.Type);
                    var newId = Convert.ToInt32(command.ExecuteScalar());
                    var type = category.Type;

                    categories.Add(new Category(newId, type));

                    return categories;
                }
            }
        }
    }
}
