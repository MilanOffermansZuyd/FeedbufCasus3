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
        private ZuydUser loggedInUser;
        public ZuydUser LoggedInUser => loggedInUser;


        public MainWindow()
        {
            InitializeComponent();

            // Login
            LoginPanel.Visibility = Visibility.Visible;
            DashboardPanel.Visibility = Visibility.Collapsed;
            GoalsPanel.Visibility = Visibility.Collapsed;
            ActionPanel.Visibility = Visibility.Collapsed;
            WelcomeTextBlock.MouseLeftButtonDown += WelcomeTextBlock_Click;
            WelcomeTextBlock.Cursor = Cursors.Hand;
        
            AllocConsole(); // Externe console
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
                Populate7DayInfo();

                WelcomeTextBlock.Text = $"Welkom {user.FirstName} {user.LastName}";

                if (user.Role == 0)
                {
                    Console.WriteLine($"Ingelogd als {user.FirstName} {user.LastName} (student).");
                }
                else if (user.Role == 1)
                {
                    Console.WriteLine($"Ingelogd als {user.FirstName} {user.LastName} (teacher).");
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

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            DashboardPanel.Visibility = Visibility.Visible;
            Populate7DayInfo();
        }


        // Logout
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            loggedInUser = null;
            EmailBox.Text = "";
            PasswordBox.Password = "";
            ErrorText.Text = "";

            HideAllPanels();
            LoginPanel.Visibility = Visibility.Visible;
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


        // Edit Profile / Delete Profile

        private void WelcomeTextBlock_Click(object sender, MouseButtonEventArgs e)
        {
            if (loggedInUser != null)
            {
                HideAllPanels();
                ProfilePanel.Visibility = Visibility.Visible;
                EditFirstNameBox.Text = loggedInUser.FirstName;
                EditLastNameBox.Text = loggedInUser.LastName;
                EditEmailBox.Text = loggedInUser.Email;
                EditPasswordBox.Password = "";
                EditConfirmPasswordBox.Password = "";
                ProfileErrorText.Text = "";

                foreach (ComboBoxItem item in EditRoleComboBox.Items)
                {
                    if (item.Tag != null && int.Parse(item.Tag.ToString()) == loggedInUser.Role)
                    {
                        EditRoleComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void ApplyProfileChanges_Click(object sender, RoutedEventArgs e)
        {
            string firstName = EditFirstNameBox.Text.Trim();
            string lastName = EditLastNameBox.Text.Trim();
            string email = EditEmailBox.Text.Trim();
            string password = EditPasswordBox.Password.Trim();
            string confirmPassword = EditConfirmPasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email))
            {
                ProfileErrorText.Text = "Vul alle verplichte velden in.";
                return;
            }

            if (!string.IsNullOrEmpty(password) && password != confirmPassword)
            {
                ProfileErrorText.Text = "Wachtwoorden komen niet overeen.";
                return;
            }

            loggedInUser.FirstName = firstName;
            loggedInUser.LastName = lastName;
            loggedInUser.Email = email;
            if (!string.IsNullOrEmpty(password))
                loggedInUser.Password = password;

            ComboBoxItem selectedRoleItem = EditRoleComboBox.SelectedItem as ComboBoxItem;
            if (selectedRoleItem == null || selectedRoleItem.Tag == null)
            {
                ProfileErrorText.Text = "Selecteer een rol.";
                return;
            }

            int role = int.Parse(selectedRoleItem.Tag.ToString());
            loggedInUser.Role = role;

            dal.UpdateZuydUserFromDatabase(loggedInUser);
            ProfileErrorText.Text = "";
            MessageBox.Show("Je profiel is succesvol bijgewerkt!");

            WelcomeTextBlock.Text = $"Welkom {loggedInUser.FirstName} {loggedInUser.LastName}";
            ProfilePanel.Visibility = Visibility.Collapsed;
            DashboardPanel.Visibility = Visibility.Visible;
        }
        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Weet je zeker dat je je account wil verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                dal.DeleteZuydUserFromDatabase(loggedInUser.Id);
                loggedInUser = null;
                MessageBox.Show("Je account is verwijderd.");

                ProfilePanel.Visibility = Visibility.Collapsed;
                LoginPanel.Visibility = Visibility.Visible;
                EmailBox.Text = "";
                PasswordBox.Password = "";
            }
        }
        private void Profile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyProfileChanges_Click(sender, e);
            }
        }
        private void CancelProfileChange_Click(object sender, RoutedEventArgs e)
        {
            ProfilePanel.Visibility = Visibility.Collapsed;
            DashboardPanel.Visibility = Visibility.Visible;
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
        private void CreateGoalButton_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInUser.Role == 0) // Student
            {
                if (SoftDeadlinePicker.SelectedDate == null)
                {
                    MessageBox.Show("Selecteer een soft deadline.");
                    return;
                }

                if (HardDeadlinePicker.SelectedDate == null)
                {
                    MessageBox.Show("Selecteer een hard deadline.");
                    return;
                }

                int id = 0;
                DateTime soft = SoftDeadlinePicker.SelectedDate.Value;
                DateTime hard = HardDeadlinePicker.SelectedDate.Value;

                int catType = 0;
                Category category = null;

                if (CategorySelectionListBx.SelectedItem is ListBoxItem selectedItem)
                {
                    catType = MapCategory(selectedItem.Content.ToString());
                    if (catType > 0)
                    {
                        category = new Category(catType, selectedItem.Content.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Geen categorie geselecteerd.");
                    return;
                }

                string body = GoalTextTxtBx.Text;
                string shortDescription = ShortDescTxtBx.Text;
                ZuydUser student = loggedInUser;
                ZuydUser author = loggedInUser;
                bool openForFeedback = OpenForFBChckBx.IsChecked == true;
                bool finished = false;

                Goal goal = new Goal(id, soft, hard, finished, category, body, student, author, openForFeedback, null, shortDescription);
                dal.AddGoalFromDatabase(goal);

                HideAllPanels();
                // Reset formulier
                ClearAllFields(AddGoalStack);
                GoalsPanel.Visibility = Visibility.Visible;
                FillGoalListView(GoalsListView);
            }
            else // Teacher
            {
                if (HardDeadlinePicker.SelectedDate == null)
                {
                    MessageBox.Show("Selecteer een hard deadline.");
                    return;
                }

                int id = 0;
                DateTime soft = new DateTime(2000, 01, 01); // default voor docenten
                DateTime hard = HardDeadlinePicker.SelectedDate.Value;

                int catType = 0;
                string shortDescription = ShortDescTxtBx.Text;
                Category category = null;

                if (CategorySelectionListBx.SelectedItem is ListBoxItem selectedItem)
                {
                    catType = MapCategory(selectedItem.Content.ToString());
                    if (catType > 0)
                    {
                        category = new Category(catType, selectedItem.Content.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Geen categorie geselecteerd.");
                    return;
                }

                string body = GoalTextTxtBx.Text;
                ZuydUser student = loggedInUser;
                ZuydUser author = loggedInUser;
                bool openForFeedback = OpenForFBChckBx.IsChecked == true;
                bool finished = false;

                Goal goal = new Goal(id, soft, hard, finished, category, body, student, author, openForFeedback, null, shortDescription);
                dal.AddGoalFromDatabase(goal);

                HideAllPanels();
                // Reset formulier
                ClearAllFields(AddGoalStack);
                GoalsPanel.Visibility = Visibility.Visible;
                FillGoalListView(GoalsListView);
            }
        }

        private int MapCategory(string? category)
        {
            switch (category)
            {
                case ("Privé Doel")
                :
                    return 1;
                case ("Privé School Doel")
                :
                    return 2;
                case ("School Doel")
                :
                    return 3;
                default: return 1;
            }
        }

        private void GoalButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            GoalsPanel.Visibility = Visibility.Visible;
        }

        private void AddGoalButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            AddGoalPanel.Visibility = Visibility.Visible;
        }

        private void BackToGoalsFromAddGoal_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            GoalsPanel.Visibility = Visibility.Visible;
        }

        private void BackToGoalsFromUpdateGoal_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            GoalsPanel.Visibility = Visibility.Visible;
        }

        private void BackToDashboardFromGoal_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            DashboardPanel.Visibility = Visibility.Visible;
            Populate7DayInfo();
        }

        //Add Action
        private void CreateUserActionButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime createdOn = DateTime.Now;
            DateTime? soft = ActionSoftDeadlinePicker.SelectedDate;
            DateTime? hard = ActionHardDeadlinePicker.SelectedDate;

            if (!soft.HasValue || !hard.HasValue)
            {
                MessageBox.Show("Selecteer een geldige soft en hard deadline.");
                return;
            }

            string shortDescription = ActionShortDescTxtBx.Text;
            if (GoalsSelectionListView.SelectedItem is not Goal goal)
            {
                MessageBox.Show("Geen doel geselecteerd.");
                return;
            }

            string text = ActionTextTxtBx.Text;
            bool OpenForFeedback = ActionOpenForFBChckBx.IsChecked == true;
            bool finished = false;

            ZuydUser student = goal.Student;
            ZuydUser author = loggedInUser;

            UserAction userAction = new UserAction(
                0, goal, createdOn, soft.Value, hard.Value, finished,
                text, student, author, shortDescription, OpenForFeedback
            );

            dal.AddUserActionFromDatabase(userAction);
            FillActionListView(ActionListView);

            ClearAllFields(AddActionStack);

            AddActionPanel.Visibility = Visibility.Collapsed;
            ActionPanel.Visibility = Visibility.Visible;
        }

        private int goalToUpdate;
        private Goal selectedGoalForUpdate;

        private void UpdateGoalButtonView_Click(object sender, RoutedEventArgs e)
        {
            if (GoalsListView.SelectedItem is Goal selectedGoal)
            {
                HideAllPanels();
                UpdateGoalPanel.Visibility = Visibility.Visible;
                goalToUpdate = selectedGoal.Id;
                selectedGoalForUpdate = selectedGoal;
                UGoalTextLbl.Content = selectedGoal.ShortDescription;
                UGoalTextTxtBx.Text = selectedGoal.Text;
                UOpenForFBChckBx.IsChecked = selectedGoal.OpenForFeedback;
                USoftDeadlinePicker.SelectedDate = selectedGoal.SoftDeadline;
                UHardDeadlinePicker.SelectedDate = selectedGoal.HardDeadline;
                UCategorySelectionListBx.SelectedItem = null;
                switch (selectedGoal.Category.Type)
                {
                    case "Privé Doel":
                        UCategorySelectionListBx.SelectedItem = UPrivateGoalItem;
                        break;
                    case "Privé School Doel":
                        UCategorySelectionListBx.SelectedItem = UPrivateSchoolGoalItem;
                        break;
                    case "School Doel":
                        UCategorySelectionListBx.SelectedItem = USchoolGoalItem;
                        break;
                    default:
                        UCategorySelectionListBx.SelectedItem = null;
                        break;
                }

            }
        }

        private int userActionToUpdate;
        private UserAction selectedUserActionForUpdate;

        private void UpdateUserActionButtonView_Click(object sender, RoutedEventArgs e)
        {
            if (ActionListView.SelectedItem is UserAction selectedActionUser)
            {
                HideAllPanels();
                UpdateActionPanel.Visibility = Visibility.Visible;
                FillGoalListView(UGoalsSelectionListView);
                selectedUserActionForUpdate = selectedActionUser;
                userActionToUpdate = selectedActionUser.Id;
                UActionTextLbl.Content = selectedActionUser.ShortDescription;
                UActionTextTxtBx.Text = selectedActionUser.Text;
                UActionOpenForFBChckBx.IsChecked = selectedActionUser.OpenForFeedback;
                UActionSoftDeadlinePicker.Text = selectedActionUser.SoftDeadline.ToString();
                UActionHardDeadlinePicker.Text = selectedActionUser.HardDeadline.ToString();

                foreach (var item in UGoalsSelectionListView.Items)
                {
                    if (item is Goal goal && goal.Id == selectedActionUser.Goal.Id)
                    {
                        UGoalsSelectionListView.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een actie om te bewerken.");
            }
        }

        private void FillGoalListView(ListView listView)
        {
            listView.Items.Clear();
            var allGoals = dal.FillGoalsFromDatabase();
            var echteGoals = allGoals.Where(g => g.SubId == null).ToList();
            foreach ( var item in echteGoals ) 
            {
                listView.Items.Add(item);
            }
        }

        private void FillActionListView(ListView listView)
        {
            listView.Items.Clear();
            var actions = dal.FillUserActionsFromDatabase();
            foreach (var item in actions)
            {
                listView.Items.Add(item);
            }
        }

        private void GoalsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GoalsListView.SelectedItem is Goal selectedGoal)
            {
                var subGoals = dal.GetSubGoalsForGoal(selectedGoal.Id);

                if (subGoals == null)
                {
                    MessageBox.Show("subGoals is NULL!");
                    return;
                }

                if (subGoals.Any())
                {
                    var subGoalWindow = new SubGoalWindow(subGoals);
                    subGoalWindow.Show();
                }
                else
                {
                    MessageBox.Show("Deze goal heeft geen subgoals.");
                }
            }
        }

        private void DeleteGoalButton_Click(object sender, RoutedEventArgs e)
        {
            if (GoalsListView.SelectedItem is Goal selectedGoal)
            {
                var gekoppeldeActies = dal.FillUserActionsFromDatabase()
                                          .Where(a => a.Goal.Id == selectedGoal.Id)
                                          .ToList();

                foreach (var actie in gekoppeldeActies)
                {
                    dal.DeleteUserActionFromDatabase(actie.Id);
                }

                var goals = dal.DeleteGoalFromDatabase(selectedGoal.Id);

                GoalsListView.Items.Clear();
                foreach (var item in goals)
                {
                    GoalsListView.Items.Add(item);
                }

                var acties = dal.FillUserActionsFromDatabase();
                ActionListView.Items.Clear();
                foreach (var actie in acties)
                {
                    ActionListView.Items.Add(actie);
                }
            }
        }

        private void DeleteUserActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActionListView.SelectedItem is UserAction UserActionList)
            {
                var Actioins = dal.DeleteUserActionFromDatabase(UserActionList.Id);

                ActionListView.Items.Clear();
                foreach (var item in Actioins)
                {
                    ActionListView.Items.Add(item);
                }
            }
        }

        private void UpdateGoalButton_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInUser.Role == 0) //Student
            {
                int id = goalToUpdate;
                DateTime soft = USoftDeadlinePicker.SelectedDate.Value;
                DateTime hard = UHardDeadlinePicker.SelectedDate.Value;
                string shortDescription = ShortDescTxtBx.Text;
                Category category = null;

                if (UCategorySelectionListBx.SelectedItem is ListBoxItem selectedItem)
                {
                    var catType = MapCategory(selectedItem.Content.ToString());

                    if (catType > 0)
                    {
                        category = dal.FillCategorysFromDatabase().FirstOrDefault(c => c.Id == catType);
                    }
                }

                if (category == null)
                {
                    var originalGoal = dal.GetGoalFromDatabaseBy(goalToUpdate);
                    category = originalGoal?.Category;

                    if (category == null)
                    {
                        MessageBox.Show("Er is geen categorie geselecteerd en ook geen originele categorie gevonden.");
                        return;
                    }
                }

                string body = UGoalTextTxtBx.Text;
                ZuydUser student = loggedInUser;
                ZuydUser author = loggedInUser;
                bool OpenForFeedback = UOpenForFBChckBx.IsChecked == true;
                bool finished = false;

                Goal goal = new Goal(id, soft, hard, finished, category, body, student, author, OpenForFeedback, null, shortDescription);
                dal.UpdateGoalFromDatabase(goal);

                var selectedGoal = dal.GetGoalFromDatabaseBy(goal.Id);

                UpdateGoalPanel.Visibility = Visibility.Hidden;
                ClearAllFields(UpdateGoalStack);
                GoalsPanel.Visibility = Visibility.Visible;
                FillGoalListView(GoalsListView);
            }
            else //Teacher
            {
                int id = goalToUpdate;
                DateTime soft = USoftDeadlinePicker.SelectedDate.Value;
                DateTime hard = UHardDeadlinePicker.SelectedDate.Value;
                string shortDescription = ShortDescTxtBx.Text;
                Category category = null;

                if (UCategorySelectionListBx.SelectedItem is ListBoxItem selectedItem)
                {
                    var catType = MapCategory(selectedItem.Content.ToString());

                    if (catType > 0)
                    {
                        category = dal.FillCategorysFromDatabase().FirstOrDefault(c => c.Id == catType);
                    }
                }

                if (category == null)
                {
                    var originalGoal = dal.GetGoalFromDatabaseBy(goalToUpdate);
                    category = originalGoal?.Category;

                    if (category == null)
                    {
                        MessageBox.Show("Er is geen categorie geselecteerd en ook geen originele categorie gevonden.");
                        return;
                    }
                }

                string body = UGoalTextTxtBx.Text;
                ZuydUser student = loggedInUser;
                ZuydUser author = loggedInUser;
                bool OpenForFeedback = UOpenForFBChckBx.IsChecked == true;
                bool finished = false;
                Goal goal = new Goal(id, soft, hard, finished, category, body, student, author, OpenForFeedback, null, shortDescription);

                dal.UpdateGoalFromDatabase(goal);

                var selectedGoal = dal.GetGoalFromDatabaseBy(goal.Id);

                UpdateGoalPanel.Visibility = Visibility.Hidden;
                ClearAllFields(UpdateGoalStack);
                GoalsPanel.Visibility = Visibility.Visible;
                FillGoalListView(GoalsListView);
            }
        }

        private void UpdateUserActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInUser.Role == 0) //Student
            {
                DateTime createdOn = DateTime.Now;
                DateTime soft = UActionSoftDeadlinePicker.SelectedDate.Value;
                DateTime hard = UActionHardDeadlinePicker.SelectedDate.Value;
                string shortDescription = ActionShortDescTxtBx.Text;
                Goal goal = null;
                if (UGoalsSelectionListView.SelectedItem is Goal selectedItem)
                {
                    goal = selectedItem;

                }
                else
                {
                    MessageBox.Show("Geen item geselecteerd.");
                    return;
                }

                string text = UActionTextTxtBx.Text;
                ZuydUser student = loggedInUser;
                ZuydUser author = loggedInUser;
                bool OpenForFeedback = UActionOpenForFBChckBx.IsChecked == true;
                bool finished = false;



                UserAction userAction = new UserAction(userActionToUpdate, goal, createdOn, soft, hard, finished, text, student, author, shortDescription, OpenForFeedback);
                dal.UpdateUserActionFromDatabase(userAction);
                FillActionListView(ActionListView);
                ActionPanel.Visibility = Visibility.Visible;
                UpdateActionPanel.Visibility = Visibility.Hidden;
            }
            else //Teacher
            {
                DateTime createdOn = DateTime.Now;
                DateTime soft = UActionSoftDeadlinePicker.SelectedDate.Value;
                DateTime hard = UActionHardDeadlinePicker.SelectedDate.Value;
                string shortDescription = UActionShortDescTxtBx.Text;
                Goal goal = null;
                if (UGoalsSelectionListView.SelectedItem is Goal selectedItem)
                {
                    goal = selectedItem;
                }
                else
                {
                    MessageBox.Show("Geen item geselecteerd.");
                    return;
                }

                string text = UActionTextTxtBx.Text;
                ZuydUser student = loggedInUser;
                ZuydUser author = loggedInUser;
                bool OpenForFeedback = UActionOpenForFBChckBx.IsChecked == true;
                bool finished = false;
                UserAction userAction = new UserAction(userActionToUpdate, goal, createdOn, soft, hard, finished, text, student, author, shortDescription, OpenForFeedback);

                dal.UpdateUserActionFromDatabase(userAction);
                FillActionListView(ActionListView);
                ActionPanel.Visibility = Visibility.Visible;
                UpdateActionPanel.Visibility = Visibility.Hidden;
            }
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            ActionPanel.Visibility = Visibility.Visible;
        }

        private void AddUserActionButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            AddActionPanel.Visibility = Visibility.Visible;
            FillGoalListView(GoalsSelectionListView);
        }

        private void BackToActionsFromUpdateAction_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            ActionPanel.Visibility = Visibility.Visible;
        }


        private void BackToActionsFromAddAction_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            ActionPanel.Visibility = Visibility.Visible;

        }

        private void BackToDashboardFromAction_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            DashboardPanel.Visibility = Visibility.Visible;
            Populate7DayInfo();
        }


        //grijp alle goals die een deadine in de komende 7 dagen hebben en laat deze zien in de dashboard
        private void Populate7DayInfo()
        {

            TextBlock InfoBox = _7DayText;
            ListView DashBoardGoals = _7DayGrid;

            DateTime Today = DateTime.Today;

            List<Goal> Upcoming = new List<Goal>();

            List<Goal> ToDo = dal.GetAllToDoGoalsFromDatabaseBy(loggedInUser.Id);
            foreach (Goal goal in ToDo.ToList())
            {
                if (goal.SoftDeadline > Today && goal.SoftDeadline < Today.AddDays(7))
                {
                    if (!Upcoming.Contains(goal))
                    {
                        Upcoming.Add(goal);
                    }
                }
                if (goal.HardDeadline > Today && goal.HardDeadline < Today.AddDays(7))
                {
                    if (!Upcoming.Contains(goal))
                    {
                        Upcoming.Add(goal);
                    }
                }
            }

            if (Upcoming.Count == 0)
            {
                InfoBox.Visibility = Visibility.Hidden;
                DashBoardGoals.Visibility = Visibility.Hidden;
            }
            else
            {
                InfoBox.Visibility = Visibility.Visible;
                DashBoardGoals.Visibility = Visibility.Visible;
                DashBoardGoals.ItemsSource = Upcoming;
            }

        }


        // Feedback
        private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            FeedbackPanel.Visibility = Visibility.Visible;
            FeedbackListView.ItemsSource = dal.FillFeedbacksFromDatabase();
            GoalComboBox.ItemsSource = dal.FillGoalsFromDatabase().Where(g => g.OpenForFeedback).ToList();
        }

        private void AddFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeedbackTextBox.Text.Trim() == "") return;

            Goal selectedGoal = GoalComboBox.SelectedItem as Goal;
            if (selectedGoal == null)
            {
                MessageBox.Show("Selecteer een doel om feedback op te geven.");
                return;
            }

            Feedback feedback = new Feedback(0, selectedGoal, FeedbackTextBox.Text, loggedInUser, loggedInUser, FeedbackTitleTextBox.Text);
            dal.AddFeedbackFromDatabase(feedback);

            FeedbackListView.ItemsSource = dal.FillFeedbacksFromDatabase();
            FeedbackListView.Items.Refresh();
            FeedbackTextBox.Text = "";
            FeedbackTitleTextBox.Text = "";
            GoalComboBox.SelectedIndex = -1;
        }

        private void UpdateFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeedbackListView.SelectedItem is not Feedback selectedFeedback)
            {
                MessageBox.Show("Selecteer een doel om feedback aan te passen.");
                return;
            }

            if (FeedbackTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Voer nieuwe feedback in om te kunnen updaten.");
                return;
            }

            selectedFeedback.Text = FeedbackTextBox.Text;
            dal.UpdateFeedbackFromDatabase(selectedFeedback);

            FeedbackListView.ItemsSource = null;
            FeedbackListView.ItemsSource = dal.FillFeedbacksFromDatabase();
            FeedbackListView.Items.Refresh();

            FeedbackTextBox.Text = "";
        }

        private void DeleteFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeedbackListView.SelectedItem is not Feedback selectedFeedback)
            {
                MessageBox.Show("Selecteer een doel om feedback te kunnen verwijderen.");
                return;
            }

            dal.DeleteFeedbackFromDatabase(selectedFeedback.Id);

            FeedbackListView.ItemsSource = null;
            FeedbackListView.ItemsSource = dal.FillFeedbacksFromDatabase();
            FeedbackListView.Items.Refresh();

            FeedbackTextBox.Text = "";
        }

        private void BackToDashboardFromFeedback_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            DashboardPanel.Visibility = Visibility.Visible;
            Populate7DayInfo();
        }

        private void FeedbackGoalsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FeedbackListView.SelectedItem is Feedback selectedFeedback)
            {
                FullFeedbackTextBox.Text = selectedFeedback.Text;
                HideAllPanels();
                ViewFeedbackPanel.Visibility = Visibility.Visible;
            }
        }

        private void BackToFeedbackFromView_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            FeedbackPanel.Visibility = Visibility.Visible;
        }

        private void HideAllPanels()
        {
            DashboardPanel.Visibility = Visibility.Collapsed;
            ActionPanel.Visibility = Visibility.Collapsed;
            GoalsPanel.Visibility = Visibility.Collapsed;
            FeedbackPanel.Visibility = Visibility.Collapsed;
            ProfilePanel.Visibility = Visibility.Collapsed;
            AddGoalPanel.Visibility = Visibility.Collapsed;
            AddActionPanel.Visibility = Visibility.Collapsed;
            UpdateGoalPanel.Visibility = Visibility.Collapsed;
            UpdateActionPanel.Visibility = Visibility.Collapsed;
            ViewFeedbackPanel.Visibility = Visibility.Collapsed;
        }
        
                private void ClearAllFields(StackPanel stackPanel)
        {
            foreach (var child in stackPanel.Children)
            {
                if (child is TextBox textBox)
                {
                    textBox.Clear();
                }
                else if (child is PasswordBox passwordBox)
                {
                    passwordBox.Clear();
                }
                else if (child is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = -1;
                }
                else if (child is DatePicker datePicker)
                {
                    datePicker.SelectedDate = null;
                }
                else if (child is CheckBox checkBox)
                {
                    checkBox.IsChecked = false;
                }
                else if (child is ListView listView)
                {
                    listView.Items.Clear();
                }
            }
        }
        
                private readonly string placeholderText = "Zoek op student naam...";

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = "";
                    textBox.Foreground = Brushes.Black;
                }
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholderText;
                    textBox.Foreground = Brushes.Gray;
                }
            }
        }

        private void GoalSearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchText = GoalSearchTextBox.Text;

                if (!string.IsNullOrWhiteSpace(searchText) && searchText != "Zoek op student naam...")
                {
                    List<Goal> filteredGoals = dal.GetGoalsByStudentName(searchText);
                    SearchedUserWIndow searchedUserWIndow = new SearchedUserWIndow(filteredGoals);
                    searchedUserWIndow.Show();
                }
            }
        }

        private void ActionSearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchText = ActionSearchTextBox.Text;

                if (!string.IsNullOrWhiteSpace(searchText) && searchText != "Zoek op student naam...")
                {
                    List<UserAction> filteredActions = dal.GetUserActionsByStudentName(searchText);
                    SearchedUserWIndow searchedUserWIndow = new SearchedUserWIndow(filteredActions);
                    searchedUserWIndow.Show();
                }
            }
        }

        private void FeedbackSearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchText = FeedbackSearchTextBox.Text;

                if (!string.IsNullOrWhiteSpace(searchText) && searchText != "Zoek op student naam...")
                {
                    List<Feedback> filteredFeedback = dal.GetFeedbackByStudentName(searchText);
                    SearchedUserWIndow searchedUserWIndow = new SearchedUserWIndow(filteredFeedback);
                    searchedUserWIndow.Show();
                }
            }
        }
    }
}