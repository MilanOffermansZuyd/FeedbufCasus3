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
            SearchedUserListView.ItemsSource = goals;
        }

        public SearchedUserWIndow(List<Action> actions)
        {
            InitializeComponent();
            SearchedUserListView.ItemsSource = actions;
        }

        public SearchedUserWIndow(List<Feedback> feedback)
        {
            InitializeComponent();
            SearchedUserListView.ItemsSource = feedback;
        }
    }
}
