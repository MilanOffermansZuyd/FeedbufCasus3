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
using static System.Net.Mime.MediaTypeNames;

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
            FillActionListView(ActionListView);
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
            string shortDescription = "";

            Feedback feedback = new Feedback(id, goal, body, student, author, shortDescription);
            dal.AddFeedbackFromDatabase(feedback);
        }

        //Add Action
        private void AddUserActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (User.Role == 0) //Student
            {
                DateTime createdOn = DateTime.Now;
                DateTime soft = ActionSoftDeadlinePicker.SelectedDate.Value;
                DateTime hard = ActionHardDeadlinePicker.SelectedDate.Value;
                string shortDescription = ActionShortDescTxtBx.Text;

                if (GoalSelectionListBx.SelectedItem is ListBoxItem selectedItem)
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
                string text = ActionTextTxtBx.Text;
                ZuydUser student = User;
                ZuydUser author = User;
                bool OpenForFeedback = ActionOpenForFBChckBx.IsChecked == true;
                bool finished = false;
                Goal goal = null;
                


                UserAction userAction = new UserAction(0, goal, createdOn, soft, hard, finished, text, student, author, shortDescription, OpenForFeedback);
                dal.AddUserActionFromDatabase(userAction);
            }

            else //Teacher
            {
                DateTime createdOn = DateTime.Now;
                DateTime soft = ActionSoftDeadlinePicker.SelectedDate.Value;
                DateTime hard = ActionHardDeadlinePicker.SelectedDate.Value;
                string shortDescription = ActionShortDescTxtBx.Text;
                Goal goal = null;

                if (GoalSelectionListBx.SelectedItem is ListBoxItem selectedItem)
                {
                    goal = selectedItem;
                }
                else
                {
                    MessageBox.Show("Geen item geselecteerd.");
                }

                string text = ActionTextTxtBx.Text;
                ZuydUser student = User;
                ZuydUser author = User;
                bool OpenForFeedback = ActionOpenForFBChckBx.IsChecked == true;
                bool finished = false;


                UserAction userAction = new UserAction(0, goal, createdOn, soft, hard, finished, text, student, author, shortDescription, OpenForFeedback);


                dal.AddUserActionFromDatabase(userAction);

            }
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

        private void FillGoalListView(ListView listView)
        {
            var goals = dal.FillGoalsFromDatabase();
            GoalsListView.ItemsSource = goals;
        }

        private void FillActionListView(ListView listView)
        {
            var actions = dal.FillUserActionsFromDatabase();
            ActionListView.ItemsSource = actions;
        }

        private void GoalsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GoalsListView.SelectedItem is Goal selectedGoal)
            {
                // Haal de bijbehorende subgoals op via de DAL
                var subGoals = dal.GetSubGoalByGoalId(selectedGoal.Id);

                // Open het subgoal venster en geef de lijst door
                var subGoalWindow = new SubGoalWindow(subGoals);
                subGoalWindow.Show();
            }
        }

    }
}