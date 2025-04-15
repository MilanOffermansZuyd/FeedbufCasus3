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
        public static ZuydUser User;
        DAL dal = new DAL();
        
        public MainWindow()
        {
            InitializeComponent();

            // Login ---- Comment dit uit als je de login tijdelijk weg wil
            LoginPanel.Visibility = Visibility.Hidden;
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
            User = user;
            Console.WriteLine(User.FirstName);
        }


        //Add goal
        private void AddGoalButton_Click(object sender, RoutedEventArgs e)
        {
            if (User.Role == 0) //Student
            {
                int id = 0;
                DateTime soft = SoftDeadlinePicker.SelectedDate.Value;
                DateTime hard = HardDeadlinePicker.SelectedDate.Value;
                string catType = null;
                Category category = null;
                string shortDescription = ShortDescTxtBx.Text;

                if (CategorySelectionListBx.SelectedItem is ListBoxItem selectedItem)
                {
                    catType = selectedItem.Content.ToString();
                }
                else
                {
                    MessageBox.Show("Geen item geselecteerd.");
                }

                if (catType != null)
                {
                    category = new Category(catType);
                }
                string body = GoalTextTxtBx.Text;
                ZuydUser student = User;
                ZuydUser author = User;
                bool OpenForFeedback = OpenForFBChckBx.IsChecked == true;
                bool finished = false;


                Goal goal = new Goal(id, soft, hard, finished, category, body, student, author, OpenForFeedback, shortDescription);
                dal.AddGoalFromDatabase(goal);
            }

            else //Teacher
            {
                int id = 0;
                DateTime soft = new DateTime(2000,01,01);
                DateTime hard = HardDeadlinePicker.SelectedDate.Value;
                string catType = null;
                string shortDescription = ShortDescTxtBx.Text;


                if (CategorySelectionListBx.SelectedItem is ListBoxItem selectedItem)
                {
                    catType = selectedItem.Content.ToString();
                }
                else
                {
                    MessageBox.Show("Geen item geselecteerd.");
                }

                Category category = null;
                if (catType != null)
                {
                    category = new Category(catType);
                }
                string body = GoalTextTxtBx.Text;
                ZuydUser student = User;
                ZuydUser author = User;
                bool OpenForFeedback = OpenForFBChckBx.IsChecked == true;
                bool finished = false;


                Goal goal = new Goal(id, soft, hard, finished, category, body, student, author, OpenForFeedback, shortDescription);

                dal.AddGoalFromDatabase(goal);
            }
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

        private void CreateGoalButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility = Visibility.Hidden;
            GoalsPanel.Visibility = Visibility.Hidden;
            ActionPanel.Visibility = Visibility.Hidden;
            AddGoalPanel.Visibility = Visibility.Visible;
        }
    }
}