using FeedBuf.Catagory;
using System.Diagnostics;
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
        DAL dal = new DAL();
        public MainWindow()
        {
            InitializeComponent();

            // Login ---- Comment dit uit als je de login tijdelijk weg wil
            LoginPanel.Visibility = Visibility.Visible;
            DashboardPanel.Visibility = Visibility.Collapsed;
            GoalsPanel.Visibility = Visibility.Collapsed;
            ActionPanel.Visibility = Visibility.Collapsed;
        }


        // Login ---- Comment dit uit als je de login tijdelijk weg wil
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            var users = dal.FillZuydUsersFromDatabase();
            var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                LoginPanel.Visibility = Visibility.Collapsed;
                DashboardPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorText.Text = "Ongeldig e-mailadres of wachtwoord.";
                ErrorText.Visibility = Visibility.Visible;
            }
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