using FeedBuf.Catagory;
using Microsoft.Data.SqlClient;

namespace FeedBuf
{
    public class DAL
    {
        string connectionString = "Server=localhost;Database=Feedbuf;Trusted_Connection=True;TrustServerCertificate=True;";
        List<Category> categories = new List<Category>();
        List<ActionFeedback> actionFeedbacks = new List<ActionFeedback>();
        List<Feedback> feedbacks = new List<Feedback>();
        List<Goal> goals = new List<Goal>();
        List<Message> messages = new List<Message>();
        List<UserAction> userActions = new List<UserAction>();
        List<ZuydUser> zuydUsers = new List<ZuydUser>();

        //Category
        public List<Category> FillCategorysFromDatabase() 
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
        public Category GetCategoryFromDatabaseBy(int Id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM Category WHERE Id = @ID";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var type = reader[1].ToString();

                            return new Category(id, type);
                        }
                    }
                    return null;
                }
            }
        }

        public List<Category> AddCategoryFromDatabase(Category category)
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

        public List<Category> DeleteCategoryFromDatabase(int categoryId)
        {
            categories.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Category WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", categoryId);
                    command.ExecuteNonQuery();

                    return FillCategorysFromDatabase();
                }
            }
        }

        public List<Category> UpdateCategoryFromDatabase(Category category)
        {
            categories.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE Category SET Type = @Type WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", category.Id);
                    command.Parameters.AddWithValue("@Type", category.Type);
                    command.ExecuteNonQuery();

                    return FillCategorysFromDatabase();
                }
            }
        }

        //Zuyduser
        public List<ZuydUser> FillZuydUsersFromDatabase()
        {
            zuydUsers.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM ZuydUser";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var firstname = reader[1].ToString();
                            var lastName = reader[2].ToString();
                            var email = reader[3].ToString();
                            var password = reader[4].ToString();
                            var role = int.Parse(reader[5].ToString());

                            zuydUsers.Add(new ZuydUser(id, firstname, lastName, email, password, role));
                        }
                    }
                    return zuydUsers;
                }
            }
        }

        public ZuydUser GetZuydUserFromDatabaseBy(int Id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM ZuydUser WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var firstname = reader[1].ToString();
                            var lastName = reader[2].ToString();
                            var email = reader[3].ToString();
                            var password = reader[4].ToString();
                            var role = int.Parse(reader[5].ToString());
                            return new ZuydUser(id, firstname, lastName, email, password, role);
                        }
                        return null;
                    }
                }
            }
        }

        public List<ZuydUser> AddZuydUserFromDatabase(ZuydUser zuydUser)
        {
            categories.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO ZuydUser (FirstName,LastName,Email,Password,Role) VALUES (@FirstName, @LastName, @Email, @Password, @Role) SELECT @@IDENTITY";
                    command.Parameters.AddWithValue("@FirstName", zuydUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", zuydUser.LastName);
                    command.Parameters.AddWithValue("@Email", zuydUser.Email);
                    command.Parameters.AddWithValue("@Password", zuydUser.Password);
                    command.Parameters.AddWithValue("@Role", zuydUser.Role);
                    var newId = Convert.ToInt32(command.ExecuteScalar());
                    var firstname = zuydUser.FirstName;
                    var lastName = zuydUser.LastName;
                    var email = zuydUser.Email;
                    var password = zuydUser.Password;
                    var role = zuydUser.Role;

                    zuydUsers.Add(new ZuydUser(newId, firstname, lastName, email, password, role));

                    return zuydUsers;
                }
            }
        }

        public List<ZuydUser> DeleteZuydUserFromDatabase(int zuydUserId)
        {
            zuydUsers.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM ZuydUser WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", zuydUserId);
                    command.ExecuteNonQuery();

                    return FillZuydUsersFromDatabase();
                }
            }
        }

        public List<ZuydUser> UpdateZuydUserFromDatabase(ZuydUser zuydUser)
        {
            zuydUsers.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE ZuydUser SET FirstName = @FirstName ,LastName = @LastName, Email = @Email, Password = @Password, Role = @Role WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", zuydUser.Id);
                    command.Parameters.AddWithValue("@FirstName", zuydUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", zuydUser.LastName);
                    command.Parameters.AddWithValue("@Email", zuydUser.Email);
                    command.Parameters.AddWithValue("@Password", zuydUser.Password);
                    command.Parameters.AddWithValue("@Role", zuydUser.Role);
                    command.ExecuteNonQuery();

                    return FillZuydUsersFromDatabase();
                }
            }
        }

        //Goal
        public List<Goal> FillGoalsFromDatabase()
        {
            goals.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Goal";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var category = GetCategoryFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                            var softDeadline = DateTime.Parse(reader[4].ToString());
                            var hardDeadline = DateTime.Parse(reader[5].ToString());
                            var isFinished = bool.Parse(reader[6].ToString());
                            var message = reader[7].ToString();
                            int? subId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);
                            var openForFeedback = Convert.ToBoolean(reader[9]);


                            if (subId == null)
                            {
                                goals.Add(new Goal(id, softDeadline, hardDeadline, isFinished, category, message, student, author, openForFeedback, subId));
                            }
                        }
                    }
                    return goals;
                }
            }
        }

        public List<Goal> GetGoalsByStudentName(string name)
        {
            List<Goal> results = new List<Goal>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                    SELECT * FROM Goal 
                    JOIN ZuydUser ON Goal.StudentId = ZuydUser.Id
                    WHERE ZuydUser.FirstName LIKE @FirstName";

                command.Parameters.AddWithValue("@FirstName", "%" + name + "%");

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = int.Parse(reader[0].ToString());
                        var category = GetCategoryFromDatabaseBy(int.Parse(reader[1].ToString()));
                        var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                        var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                        var softDeadline = DateTime.Parse(reader[4].ToString());
                        var hardDeadline = DateTime.Parse(reader[5].ToString());
                        var isFinished = bool.Parse(reader[6].ToString());
                        var message = reader[7].ToString();
                        int? subId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);
                        var openForFeedback = Convert.ToBoolean(reader[9]);

                        results.Add(new Goal(id, softDeadline, hardDeadline, isFinished, category, message, student, author, openForFeedback, subId));
                    }
                }
            }

            return results;
        }


        public Goal GetGoalFromDatabaseBy(int Id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM Goal WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var category = GetCategoryFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                            var softDeadline = DateTime.Parse(reader[4].ToString());
                            var hardDeadline = DateTime.Parse(reader[5].ToString());
                            var isFinished = bool.Parse(reader[6].ToString());
                            var message = reader[7].ToString();
                            int? subId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);
                            var openForFeedback = Convert.ToBoolean(reader[9]);

                            return new Goal(id, softDeadline, hardDeadline, isFinished, category, message, student, author, openForFeedback, subId);
                        }
                        return null;
                    }
                }
            }
        }

        public List<Goal> GetAllFinishedGoalsFromDatabaseBy(int Id)
        {
            goals.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM Goal WHERE IsFinished = 1 and StudentId = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var category = GetCategoryFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                            var softDeadline = DateTime.Parse(reader[4].ToString());
                            var hardDeadline = DateTime.Parse(reader[5].ToString());
                            var isFinished = bool.Parse(reader[6].ToString());
                            var message = reader[7].ToString();
                            int? subId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);
                            var openForFeedback = Convert.ToBoolean(reader[9]);

                            goals.Add( new Goal(id, softDeadline, hardDeadline, isFinished, category, message, student, author, openForFeedback, subId));
                        }
                        return goals;
                    }
                }
            }
        }

        public List<Goal> GetAllOpenGoalsFromDatabaseBy(int Id)
        {
            goals.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM Goal WHERE Id = @Id and OpenForFeedback = 1 and IsFinished = 0";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var category = GetCategoryFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                            var softDeadline = DateTime.Parse(reader[4].ToString());
                            var hardDeadline = DateTime.Parse(reader[5].ToString());
                            var isFinished = bool.Parse(reader[6].ToString());
                            var message = reader[7].ToString();
                            int? subId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);
                            var openForFeedback = Convert.ToBoolean(reader[9]);

                            goals.Add(new Goal(id, softDeadline, hardDeadline, isFinished, category, message, student, author, openForFeedback, subId));
                        }
                        return goals;
                    }
                }
            }
        }

        public List<Goal> GetAllDoneGoalsFromDatabaseBy(int Id)
        {
            goals.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM Goal WHERE Id = @Id and IsFinished = 1";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var category = GetCategoryFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                            var softDeadline = DateTime.Parse(reader[4].ToString());
                            var hardDeadline = DateTime.Parse(reader[5].ToString());
                            var isFinished = bool.Parse(reader[6].ToString());
                            var message = reader[7].ToString();
                            int? subId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);
                            var openForFeedback = Convert.ToBoolean(reader[9]);

                            goals.Add(new Goal(id, softDeadline, hardDeadline, isFinished, category, message, student, author, openForFeedback, subId));
                        }
                        return goals;
                    }
                }
            }
        }
        public List<Goal> GetAllToDoGoalsFromDatabaseBy(int Id)
        {
            goals.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Goal WHERE StudentId = @Id and IsFinished = 0";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var category = GetCategoryFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                            var softDeadline = DateTime.Parse(reader[4].ToString());
                            var hardDeadline = DateTime.Parse(reader[5].ToString());
                            var isFinished = bool.Parse(reader[6].ToString());
                            var message = reader[7].ToString();
                            int? subId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);
                            var openForFeedback = bool.Parse(reader[9].ToString());

                            goals.Add(new Goal(id, softDeadline, hardDeadline, isFinished, category, message, student, author, openForFeedback, subId));
                        }
                        return goals;
                    }
                }
            }
        }

        public List<Goal> AddGoalFromDatabase(Goal goal)
        {
            goals.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    // Basis van de INSERT-statement
                    string baseQuery = "INSERT INTO Goal (CategoryId, StudentId, AuthorId, SoftDeadline, HardDeadline, IsFinished, Message, OpenForFeedback";

                    // Als er een SubId is, voeg die toe
                    if (goal.SubId.HasValue)
                    {
                        baseQuery += ", SubId) VALUES (@CategoryId, @StudentId, @AuthorId, @SoftDeadline, @HardDeadline, @IsFinished, @Text, @OpenForFeedback, @SubId)";
                    }
                    else
                    {
                        baseQuery += ") VALUES (@CategoryId, @StudentId, @AuthorId, @SoftDeadline, @HardDeadline, @IsFinished, @Text, @OpenForFeedback)";
                    }

                    // Voeg SELECT @@IDENTITY toe om het nieuwe ID op te halen
                    baseQuery += "; SELECT @@IDENTITY";

                    command.CommandText = baseQuery;

                    // Voeg parameters toe
                    command.Parameters.AddWithValue("@CategoryId", goal.Category.Id);
                    command.Parameters.AddWithValue("@StudentId", goal.Student.Id);
                    command.Parameters.AddWithValue("@AuthorId", goal.Author.Id);
                    command.Parameters.AddWithValue("@SoftDeadline", goal.SoftDeadline);
                    command.Parameters.AddWithValue("@HardDeadline", goal.HardDeadline);
                    command.Parameters.AddWithValue("@IsFinished", goal.IsFinished);
                    command.Parameters.AddWithValue("@Text", goal.Text);
                    command.Parameters.AddWithValue("@OpenForFeedback", goal.OpenForFeedback);

                    if (goal.SubId.HasValue)
                    {
                        command.Parameters.AddWithValue("@SubId", goal.SubId.Value);
                    }

                    // Haal het nieuwe ID op
                    int newId = Convert.ToInt32(command.ExecuteScalar());

                    // Voeg de nieuwe goal toe aan de lijst
                    goals.Add(new Goal(newId, goal.SoftDeadline, goal.HardDeadline, goal.IsFinished, goal.Category, goal.Text, goal.Student, goal.Author, goal.OpenForFeedback, goal.SubId));

                    return goals;
                }
            }
        }


        public List<Goal> DeleteGoalFromDatabase(int goalId)
        {
            goals.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Goal WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", goalId);
                    command.ExecuteNonQuery();

                    return FillGoalsFromDatabase();
                }
            }
        }

        public List<Goal> UpdateGoalFromDatabase(Goal goal)
        {
            goals.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE Goal SET AuthorId = @AuthorId, StudentId = @StudentId, SoftDeadline = @SoftDeadline, HardDeadline = @HardDeadline, IsFinished = @IsFinished, CategoryId = @CategoryId, Message = @Text, OpenForFeedback = @OpenForFeedback WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", goal.Id);
                    command.Parameters.AddWithValue("@AuthorId", goal.Author.Id);
                    command.Parameters.AddWithValue("@StudentId", goal.Student.Id);
                    command.Parameters.AddWithValue("@CategoryId", goal.Category.Id);
                    command.Parameters.AddWithValue("@SoftDeadline", goal.SoftDeadline);
                    command.Parameters.AddWithValue("@HardDeadline", goal.HardDeadline);
                    command.Parameters.AddWithValue("@IsFinished", goal.IsFinished);
                    command.Parameters.AddWithValue("@Text", goal.Text);
                    command.Parameters.AddWithValue("@OpenForFeedback", goal.OpenForFeedback);
                    command.ExecuteNonQuery();

                    return FillGoalsFromDatabase();
                }
            }
        }

        public List<Goal> GetSubGoalByGoalId(int goalId)
        {
            List<Goal> subGoals = new List<Goal>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT Id FROM SubGoal WHERE GoalId = @GoalId";
                    command.Parameters.AddWithValue("@GoalId", goalId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int subGoalId = Convert.ToInt32(reader["Id"]);

                            // Haal de subgoal op uit de Goal-tabel
                            Goal subGoal = GetGoalFromDatabaseBy(subGoalId);

                            if (subGoal != null)
                            {
                                subGoals.Add(subGoal);
                            }
                        }
                    }
                }
            }

            return subGoals;
        }

        public List<Goal> GetSubGoalsForGoal(int goalId)
        {
            List<Goal> subGoals = new List<Goal>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = "SELECT * FROM Goal WHERE SubId = @GoalId";
                    command.Parameters.AddWithValue("@GoalId", goalId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int categoryId = reader.GetInt32(1);
                            int studentId = reader.GetInt32(2);
                            int authorId = reader.GetInt32(3);
                            DateTime softDeadline = reader.GetDateTime(4);
                            DateTime hardDeadline = reader.GetDateTime(5);
                            bool isFinished = reader.GetBoolean(6);
                            string message = reader.GetString(7);
                            bool openForFeedback = reader.GetBoolean(8);
                            int? subId = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9);

                            var category = GetCategoryFromDatabaseBy(categoryId);
                            var student = GetZuydUserFromDatabaseBy(studentId);
                            var author = GetZuydUserFromDatabaseBy(authorId);

                            subGoals.Add(new Goal(id, softDeadline, hardDeadline, isFinished, category, message, student, author, openForFeedback, subId));
                        }
                    }
                }
            }

            return subGoals;
        }



        //Feedback
        public List<Feedback> FillFeedbacksFromDatabase()
        {
            feedbacks.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM Feedback";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var goal = GetGoalFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var student = GetZuydUserFromDatabaseBy( int.Parse(reader[3].ToString()));
                            var text = reader[4].ToString();

                            feedbacks.Add(new Feedback(id, goal, text, student, student, string.Empty));
                        }
                    }
                    return feedbacks;
                }
            }
        }

        public List<Feedback> GetFeedbackByStudentName(string name)
        {
            List<Feedback> results = new List<Feedback>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                    SELECT * FROM Feedback 
                    JOIN ZuydUser ON Feedback.StudentId = ZuydUser.Id
                    WHERE ZuydUser.FirstName LIKE @FirstName";

                command.Parameters.AddWithValue("@FirstName", "%" + name + "%");

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = int.Parse(reader[0].ToString());
                        var goal = GetGoalFromDatabaseBy(int.Parse(reader[1].ToString()));
                        var author = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                        var student = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                        var text = reader[4].ToString();

                        results.Add(new Feedback(id, goal, text, student, student, string.Empty));
                    }
                }
            }

            return results;
        }

        public Feedback GetFeedbackFromDatabaseBy(int Id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM Feedback WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var goal = GetGoalFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var student = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                            var text = reader[4].ToString();

                            return new Feedback(id, goal, text, student, student, string.Empty);
                        }
                        return null;
                    }
                }
            }
        }

        public List<Feedback> AddFeedbackFromDatabase(Feedback feedback)
        {
            feedbacks.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Feedback (GoalId,StudentId,AuthorId,Message) VALUES (@GoalId, @StudentId, @AuthorId,@Message) SELECT @@IDENTITY";
                    command.Parameters.AddWithValue("@GoalId", feedback.Goal.Id);
                    command.Parameters.AddWithValue("@StudentId", feedback.Student.Id);
                    command.Parameters.AddWithValue("@AuthorId", feedback.Author.Id);
                    command.Parameters.AddWithValue("@Message", feedback.Text);
                    var newId = Convert.ToInt32(command.ExecuteScalar());
                    var Goal = feedback.Goal;
                    var User = feedback.Student;

                    feedbacks.Add(new Feedback(newId, Goal, feedback.Text, User, User , string.Empty));

                    return feedbacks;
                }
            }
        }

        public List<Feedback> DeleteFeedbackFromDatabase(int feedBackId)
        {
            feedbacks.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Feedback WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", feedBackId);
                    command.ExecuteNonQuery();

                    return FillFeedbacksFromDatabase();
                }
            }
        }

        public List<Feedback> UpdateFeedbackFromDatabase(Feedback feedback)
        {
            feedbacks.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE Feedback SET GoalId = @GoalId, StudentId = @StudentId, AuthorId = @AuthorId , Message = @Message WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", feedback.Id);
                    command.Parameters.AddWithValue("@GoalId", feedback.Goal.Id);
                    command.Parameters.AddWithValue("@StudentId", feedback.Student.Id);
                    command.Parameters.AddWithValue("@AuthorId", feedback.Author.Id);
                    command.Parameters.AddWithValue("@Message", feedback.Text);

                    command.ExecuteNonQuery();

                    return FillFeedbacksFromDatabase();
                }
            }
        }

        //UserAction
        public List<UserAction> FillUserActionsFromDatabase()
        {
            userActions.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM UserAction";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var goal = GetGoalFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                            var isFinished = bool.Parse(reader[4].ToString());
                            var createdOn = DateTime.Parse(reader[5].ToString());
                            var softDeadline = DateTime.Parse(reader[6].ToString());
                            var hardDeadline = DateTime.Parse(reader[7].ToString());
                            var text = reader[8].ToString();
                            var openForFeedback =bool.Parse( reader[9].ToString());


                            userActions.Add(new UserAction(id, goal, createdOn, softDeadline, hardDeadline, isFinished, text, student, author, string.Empty, openForFeedback));
                        }
                    }
                    return userActions;
                }
            }
        }

        public List<UserAction> GetUserActionsByStudentName(string name)
        {
            List<UserAction> results = new List<UserAction>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                    SELECT * FROM UserAction 
                    JOIN ZuydUser ON UserAction.StudentId = ZuydUser.Id
                    WHERE ZuydUser.FirstName LIKE @FirstName";

                command.Parameters.AddWithValue("@FirstName", "%" + name + "%");

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = int.Parse(reader[0].ToString());
                        var goal = GetGoalFromDatabaseBy(int.Parse(reader[1].ToString()));
                        var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                        var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                        var isFinished = bool.Parse(reader[4].ToString());
                        var createdOn = DateTime.Parse(reader[5].ToString());
                        var softDeadline = DateTime.Parse(reader[6].ToString());
                        var hardDeadline = DateTime.Parse(reader[7].ToString());
                        var text = reader[8].ToString();
                        var openForFeedback = bool.Parse(reader[9].ToString());

                        results.Add(new UserAction(id, goal, createdOn, softDeadline, hardDeadline, isFinished, text, student, author, string.Empty, openForFeedback));
                    }
                }
            }

            return results;
        }

        public UserAction GetUserActionFromDatabaseBy(int Id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM UserAction WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var goal = GetGoalFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var student = GetZuydUserFromDatabaseBy(int.Parse(reader[2].ToString()));
                            var author = GetZuydUserFromDatabaseBy(int.Parse(reader[3].ToString()));
                            var isFinished = bool.Parse(reader[4].ToString());
                            var createdOn = DateTime.Parse(reader[5].ToString());
                            var softDeadline = DateTime.Parse(reader[6].ToString());
                            var hardDeadline = DateTime.Parse(reader[7].ToString());
                            var text = reader[8].ToString();
                            var openForFeedback = bool.Parse( reader[9].ToString());

                            return new UserAction(id, goal, createdOn, softDeadline, hardDeadline, isFinished, text, student, author, string.Empty, openForFeedback);
                        }
                        return null;
                    }
                }
            }
        }

        public List<UserAction> AddUserActionFromDatabase(UserAction userAction)
        {
            userActions.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO UserAction (GoalId,StudentId,AuthorId,IsFinished,CreatedOn,SoftDeadline,HardDeadline,Message,OpenForFeedback) VALUES (@GoalId,@StudentId, @AuthorId, @IsFinished,@CreatedOn,@SoftDeadline,@HardDeadline,@Message,@OpenForFeedback) SELECT @@IDENTITY";
                    command.Parameters.AddWithValue("@GoalId", userAction.Goal.Id);
                    command.Parameters.AddWithValue("@StudentId", userAction.Student.Id);
                    command.Parameters.AddWithValue("@AuthorId", userAction.Author.Id);
                    command.Parameters.AddWithValue("@IsFinished", userAction.IsFinished);
                    command.Parameters.AddWithValue("@CreatedOn", userAction.CreatedOn);
                    command.Parameters.AddWithValue("@SoftDeadline", userAction.SoftDeadline);
                    command.Parameters.AddWithValue("@HardDeadline", userAction.HardDeadline);
                    command.Parameters.AddWithValue("@Message", userAction.Text);
                    command.Parameters.AddWithValue("@OpenForFeedback", userAction.OpenForFeedback);
                    var newId = Convert.ToInt32(command.ExecuteScalar());
                    var goal = userAction.Goal;
                    var isFinished = userAction.IsFinished;
                    var createdOn = userAction.CreatedOn;
                    var softDeadline = userAction.SoftDeadline;
                    var hardDeadline = userAction.HardDeadline;
                    var student = userAction.Student;
                    var author = userAction.Author;
                    var Message = userAction.Text;
                    var openForFeedback = userAction.OpenForFeedback;

                    userActions.Add(new UserAction(newId, goal, createdOn, softDeadline, hardDeadline, isFinished, Message, student, author, string.Empty, openForFeedback));

                    return userActions;
                }
            }
        }

        public List<UserAction> DeleteUserActionFromDatabase(int userActionId)
        {
            userActions.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM UserAction WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", userActionId);
                    command.ExecuteNonQuery();

                    return FillUserActionsFromDatabase();
                }
            }
        }

        public List<UserAction> UpdateUserActionFromDatabase(UserAction userAction)
        {
            userActions.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE UserAction SET GoalId = @GoalId,StudentId = StudentId , AuthorId = @AuthorId ,IsFinished = @IsFinished, CreatedOn = @CreatedOn ,SoftDeadline = @SoftDeadline, HardDeadline = @HardDeadline, Message = @Message WHERE Id = @Id";
                    command.Parameters.AddWithValue("@GoalId", userAction.Goal.Id);
                    command.Parameters.AddWithValue("@StudentId", userAction.Student.Id);
                    command.Parameters.AddWithValue("@AuthorId", userAction.Author.Id);
                    command.Parameters.AddWithValue("@IsFinished", userAction.IsFinished);
                    command.Parameters.AddWithValue("@CreatedOn", userAction.CreatedOn);
                    command.Parameters.AddWithValue("@SoftDeadline", userAction.SoftDeadline);
                    command.Parameters.AddWithValue("@HardDeadline", userAction.HardDeadline);
                    command.Parameters.AddWithValue("@Message", userAction.Text);
                    command.Parameters.AddWithValue("@Id", userAction.Id);

                    command.ExecuteNonQuery();

                    return FillUserActionsFromDatabase();
                }
            }
        }

        //ActionFeedback
        internal List<ActionFeedback> FillActionFeedBacksFromDatabase()
        {
            actionFeedbacks.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT* FROM UserActionFeedback";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());
                            var feedback = GetFeedbackFromDatabaseBy(int.Parse(reader[1].ToString()));
                            var userAction = GetUserActionFromDatabaseBy(int.Parse(reader[2].ToString()));


                            actionFeedbacks.Add(new ActionFeedback(id, userAction, feedback));
                        }
                    }
                    return actionFeedbacks;
                }
            }
        }

        internal List<ActionFeedback> AddActionFeedBackFromDatabase(ActionFeedback actionFeedback)
        {
            actionFeedbacks.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO UserActionFeedback (FeedbackId,ActionId) VALUES (@FeedbackId, @userActionId) SELECT @@IDENTITY";
                    command.Parameters.AddWithValue("@userActionId", actionFeedback.UserAction.Id);
                    command.Parameters.AddWithValue("@FeedbackId", actionFeedback.Feedback.Id);
                    var newId = Convert.ToInt32(command.ExecuteScalar());
                    var userAction = actionFeedback.UserAction;
                    var feedback = actionFeedback.Feedback;

                    actionFeedbacks.Add(new ActionFeedback(newId, userAction, feedback));

                    return actionFeedbacks;
                }
            }
        }

        internal List<ActionFeedback> DeleteActionFeedBackFromDatabase(int actionFeedbackId)
        {
            actionFeedbacks.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM UserActionFeedback WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", actionFeedbackId);
                    command.ExecuteNonQuery();

                    return FillActionFeedBacksFromDatabase();
                }
            }
        }

        internal List<ActionFeedback> UpdateActionFeedBackFromDatabase(ActionFeedback actionFeedback)
        {
            userActions.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE UserActionFeedback SET ActionId = @ActionId,FeedbackId = @FeedbackId WHERE Id = @Id";
                    command.Parameters.AddWithValue("@ActionId", actionFeedback.UserAction.Id);
                    command.Parameters.AddWithValue("@FeedbackId", actionFeedback.Feedback.Id);
                    command.Parameters.AddWithValue("@Id", actionFeedback.Id);

                    command.ExecuteNonQuery();

                    return FillActionFeedBacksFromDatabase();
                }
            }
        }

    }
}
