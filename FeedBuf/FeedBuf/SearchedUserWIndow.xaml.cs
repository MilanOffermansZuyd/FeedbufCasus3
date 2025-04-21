using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FeedBuf
{
    /// <summary>
    /// Interaction logic for SearchedUserWIndow.xaml
    /// </summary>
    public partial class SearchedUserWIndow : Window
    {
        public SearchedUserWIndow()
        {
            InitializeComponent();
        }

        public SearchedUserWIndow(List<Goal> goals)
        {
            InitializeComponent();
            SearchedUserListView.Items.Clear();
            SearchedUserListView.ItemsSource = goals;
            ShowListView();
        }

        public SearchedUserWIndow(List<UserAction> actions)
        {
            InitializeComponent();
            SearchedUserListView.Items.Clear();
            SearchedUserListView.ItemsSource = actions;
            ShowListView();
        }

        public SearchedUserWIndow(List<Feedback> feedback)
        {
            InitializeComponent();
            SearchedUserFeedbackListView.Items.Clear();
            SearchedUserFeedbackListView.ItemsSource = feedback;
            ShowFeedbackListView();
        }

        public void ShowListView()
        {
            SearchedUserFeedbackListView.Visibility = Visibility.Collapsed;
            SearchedUserListView.Visibility = Visibility.Visible;
        }

        public void ShowFeedbackListView()
        {
            SearchedUserListView.Visibility = Visibility.Collapsed;
            SearchedUserFeedbackListView.Visibility = Visibility.Visible;
        }
    }
}
