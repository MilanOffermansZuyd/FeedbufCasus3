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
    }
}