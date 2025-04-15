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
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        DAL dal = new DAL();
        private ZuydUser loggedInUser; // Keep track of logged in user

        public MainWindow()
        {
            InitializeComponent();

            // Login
            LoginPanel.Visibility = Visibility.Visible;
            DashboardPanel.Visibility = Visibility.Collapsed;
            GoalsPanel.Visibility = Visibility.Collapsed;
            ActionPanel.Visibility = Visibility.Collapsed;

            // Externe console
            AllocConsole();

            FillGoalListView(GoalsListView);
        }


        // Login
        private void LoginButton_Click(object sender, RoutedEventArgs e)
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

                WelcomeTextBlock.Text = $"Welkom {user.FirstName} {user.LastName}";

                if (user.Role == 0)
                {
                    Console.WriteLine($"Ingelogd als {user.FirstName} {user.LastName} (student).");
                }
                else if (user.Role == 1)
                {
                    Console.WriteLine($"Ingelogd als {user.FirstName} {user.LastName} (docent).");
                }


            }
            else
            {
                ErrorText.Text = "Ongeldig e-mailadres of wachtwoord.";
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        // Login - Enter down
        private void LoginButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }

        // Logout
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            loggedInUser = null;
            EmailBox.Text = "";
            PasswordBox.Password = "";
            ErrorText.Text = "";

            LoginPanel.Visibility = Visibility.Visible;
            DashboardPanel.Visibility = Visibility.Collapsed;
            GoalsPanel.Visibility = Visibility.Collapsed;
            ActionPanel.Visibility = Visibility.Collapsed;
        }

        // Forgot Password
        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            ForgotPasswordPanel.Visibility = Visibility.Visible;
            ForgotEmailBox.Text = "";
            ForgotPasswordResult.Text = "";
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            RegisterPanel.Visibility = Visibility.Collapsed;

            EmailBox.Text = "";
            PasswordBox.Password = "";
            ErrorText.Text = "";
        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            string email = ForgotEmailBox.Text.Trim().ToLower();
            var users = dal.FillZuydUsersFromDatabase();
            var user = users.FirstOrDefault(u => u.Email.ToLower() == email);

            if (user != null)
            {
                ForgotPasswordResult.Foreground = Brushes.DarkGreen;
                ForgotPasswordResult.Text = $"E-mailadres: {user.Email}\nWachtwoord: {user.Password}";
            }
            else
            {
                ForgotPasswordResult.Text = "Geen gebruiker gevonden met dit e-mailadres.";
                ForgotPasswordResult.Foreground = Brushes.Red;
            }
        }

        private void ForgotPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowPassword_Click(sender, e);
            }
        }

        // Register
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;

            RegisterFirstNameBox.Text = "";
            RegisterLastNameBox.Text = "";
            RegisterEmailBox.Text = "";
            RegisterPasswordBox.Password = "";
            RegisterConfirmPasswordBox.Password = "";
            RegisterResultText.Text = "";
            RoleComboBox.SelectedIndex = 0;
        }

        private void RegisterUser_Click(object sender, RoutedEventArgs e)
        {
            string firstName = RegisterFirstNameBox.Text.Trim();
            string lastName = RegisterLastNameBox.Text.Trim();
            string email = RegisterEmailBox.Text.Trim().ToLower();
            string password = RegisterPasswordBox.Password.Trim();
            string confirmPassword = RegisterConfirmPasswordBox.Password.Trim();

            ComboBoxItem selectedItem = RoleComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null || selectedItem.Tag == null)
            {
                RegisterResultText.Foreground = Brushes.Red;
                RegisterResultText.Text = "Selecteer een rol.";
                return;
            }

            int role = int.Parse(selectedItem.Tag.ToString());

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                RegisterResultText.Foreground = Brushes.Red;
                RegisterResultText.Text = "Vul alle velden in.";
                return;
            }

            if (password != confirmPassword)
            {
                RegisterResultText.Foreground = Brushes.Red;
                RegisterResultText.Text = "Wachtwoorden komen niet overeen.";
                return;
            }

            var users = dal.FillZuydUsersFromDatabase();
            if (users.Any(u => u.Email.ToLower() == email))
            {
                RegisterResultText.Foreground = Brushes.Red;
                RegisterResultText.Text = "Gebruiker met dit e-mailadres bestaat al.";
                return;
            }

            ZuydUser newUser = new ZuydUser(0, firstName, lastName, email, password, role);
            dal.AddZuydUserFromDatabase(newUser);

            RegisterResultText.Foreground = Brushes.DarkGreen;
            RegisterResultText.Text = "Gebruiker succesvol geregistreerd!";

            Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(500);

                RegisterFirstNameBox.Text = "";
                RegisterLastNameBox.Text = "";
                RegisterEmailBox.Text = "";
                RegisterPasswordBox.Password = "";
                RegisterConfirmPasswordBox.Password = "";
                RegisterResultText.Text = "";
                RoleComboBox.SelectedIndex = -1;

                RegisterPanel.Visibility = Visibility.Collapsed;
                LoginPanel.Visibility = Visibility.Visible;

                EmailBox.Text = email;
                PasswordBox.Password = password;
                ErrorText.Text = "";
            });
        }


        private void Register_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegisterUser_Click(sender, e);
            }
        }

        // Password Eye
        private void PasswordEye_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            VisiblePasswordBox.Text = PasswordBox.Password;
            VisiblePasswordBox.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Collapsed;
            VisiblePasswordBox.Focus();
            VisiblePasswordBox.SelectionStart = VisiblePasswordBox.Text.Length;
        }

        private void PasswordEye_PreviewMouseUp(object sender, MouseEventArgs e)
        {
            PasswordBox.Password = VisiblePasswordBox.Text;
            VisiblePasswordBox.Visibility = Visibility.Collapsed;
            PasswordBox.Visibility = Visibility.Visible;
            PasswordBox.Focus();
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
            NotificationPanel.Visibility = Visibility.Hidden;
        }


        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility = Visibility.Visible;
            GoalsPanel.Visibility = Visibility.Hidden;
            ActionPanel.Visibility = Visibility.Hidden;
            NotificationPanel.Visibility = Visibility.Hidden;
        }

        private void GoalButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility = Visibility.Hidden;
            GoalsPanel.Visibility = Visibility.Visible;
            ActionPanel.Visibility = Visibility.Hidden;
            //FillListView(GoalsListView);
            NotificationPanel.Visibility = Visibility.Hidden;
        }
        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility = Visibility.Hidden;
            GoalsPanel.Visibility = Visibility.Hidden;
            ActionPanel.Visibility = Visibility.Hidden;
            NotificationPanel.Visibility = Visibility.Visible;
            NotificationListView.ItemsSource = dal.GetNotificationsFromDatabaseByStudent(loggedInUser.Role);
        }

        private void FillGoalListView(ListView listView)
        {
            var goals = dal.FillGoalsFromDatabase();
            GoalsListView.ItemsSource = goals;
        }
    }
}