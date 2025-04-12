using FeedBuf.Catagory;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeedBuf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Externe console
        //[DllImport("kernel32.dll")]
        //public static extern bool AllocConsole();

        DAL dal = new DAL();
        private ZuydUser loggedInUser; // Keep track of logged in user.

        public MainWindow()
        {
            InitializeComponent();

            // Login ---- Temporarily remove the login by commenting it.
            LoginPanel.Visibility = Visibility.Visible;
            DashboardPanel.Visibility = Visibility.Collapsed;
            GoalsPanel.Visibility = Visibility.Collapsed;
            ActionPanel.Visibility = Visibility.Collapsed;

            // Externe console
            //AllocConsole();
        }


        // Login ---- Temporarily remove the login by commenting it.
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim().ToLower();
            string password = PasswordBox.Password.Trim();

            var users = dal.FillZuydUsersFromDatabase();
            var user = users.FirstOrDefault(u => u.Email.ToLower() == email && u.Password == password);

            if (user != null)
            {
                loggedInUser = user;

                LoginPanel.Visibility = Visibility.Collapsed;
                DashboardPanel.Visibility = Visibility.Visible;

                // Welcome message
                WelcomeTextBlock.Text = $"Welkom {user.FirstName} {user.LastName}";

                if (user.Role == 0)
                {
                    Console.WriteLine("Ingelogd als student.");
                }
                else if (user.Role == 1)
                {
                    Console.WriteLine("Ingelogd als docent.");
                }


            }
            else
            {
                ErrorText.Text = "Ongeldig e-mailadres of wachtwoord.";
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        // Login - Enter down
        private void LoginField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e);
            }
        }

        // Logout Button
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            loggedInUser = null;
            EmailBox.Text = "";
            PasswordBox.Password = "";
            ErrorText.Visibility = Visibility.Collapsed;

            LoginPanel.Visibility = Visibility.Visible;
            DashboardPanel.Visibility = Visibility.Collapsed;
            GoalsPanel.Visibility = Visibility.Collapsed;
            ActionPanel.Visibility = Visibility.Collapsed;
        }


        //Add goal
        private void AddGoalButton_Click(object sender, RoutedEventArgs e)
        {
            //placeholder for category type
            string catType = "";

            int id = 1;
            DateTime soft = new DateTime();
            DateTime hard = new DateTime();
            bool finished = false;
            Category category = new Category(catType);
            string body = "";
            ZuydUser student = null;
            ZuydUser author = null;
            bool OpenForFeedback = false;


            Goal goal = new Goal(id, soft, hard, finished, category, body, student, author, OpenForFeedback);

            
            dal.AddGoalFromDatabase(goal);
        }

        //Add Feedback
        private void AddFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            int id = 1;
            string body = "";

            Goal goal = null;
            ZuydUser student = null;
            ZuydUser author = null;

            Feedback feedback = new Feedback(id, goal, body, student, author);
            dal.AddFeedbackFromDatabase(feedback);
        }

        //Add Action
        private void AddUserActionButton_Click(object sender, RoutedEventArgs e)
        {
            int id = 1;
            Goal goal = null;
            DateTime createdOn = new DateTime();
            DateTime soft = new DateTime();
            DateTime hard = new DateTime();

            string body = "";
            ZuydUser student = null;
            ZuydUser author = null;


            UserAction Useraction = new UserAction(id, goal, createdOn, soft, hard, body, student, author);

            dal.AddUserActionFromDatabase(Useraction);
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility = Visibility.Hidden;
            GoalsPanel.Visibility = Visibility.Hidden;
            ActionPanel.Visibility = Visibility.Visible;
        }


        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility = Visibility.Visible;
            GoalsPanel.Visibility = Visibility.Hidden;
            ActionPanel.Visibility = Visibility.Hidden;

        }

        private void GoalButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility = Visibility.Hidden;
            GoalsPanel.Visibility = Visibility.Visible;
            ActionPanel.Visibility = Visibility.Hidden;
            //FillListView(GoalsListView);

        }

        private void FillListView(ListView listView)
        {
            // Clear the ListView
            listView.Items.Clear();

            var test = dal.FillGoalsFromDatabase();
            // Get the data from the database
            foreach (var goal in test)
            {
                // Create a new ListViewItem
                ListViewItem item = new ListViewItem();
                item.Content = goal.Text;
                // Add the item to the ListView
                listView.Items.Add(item);
            }

        }
    }
}