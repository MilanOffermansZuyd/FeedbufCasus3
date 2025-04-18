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
    /// Interaction logic for SubGoalWindow.xaml
    /// </summary>
    public partial class SubGoalWindow : Window
    {
        public SubGoalWindow(List<Goal> subGoals)
        {
            InitializeComponent();
            SubGoalListView.ItemsSource = subGoals;
        }


    }
}
