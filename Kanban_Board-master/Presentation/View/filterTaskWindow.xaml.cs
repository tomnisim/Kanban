using Presentation.Model;
using Presentation.ViewModel;
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

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for filterTaskWindow.xaml
    /// </summary>
    public partial class filterTaskWindow : Window
    {
        private filterTaskViewModel filterVM;
        BoardModel board;
        public filterTaskWindow(BoardModel board) //Constructor
        {
            InitializeComponent();
            filterVM = new filterTaskViewModel(board);
            this.DataContext = filterVM;
            this.board = board;
        }

        private void Filter_Click(object sender, RoutedEventArgs e) //Filter tasks button was clicked
        {
            if(filterVM.filterTasks(board)) //Only close this window if the tasks were filtered sucessfully
                this.Close();
        }
    }
}
