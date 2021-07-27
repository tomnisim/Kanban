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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShowBoardWindow : Window
    {
        private ShowBoardViewModel showBoardVM;
        private UserModel user;
        public ShowBoardWindow(UserModel user) //Constructor
        {
            this.user = user;
            InitializeComponent();
            showBoardVM = new ShowBoardViewModel(user.Controller,user, user.getBoard());
            this.DataContext = showBoardVM;
        }
        private void addTask_Click(object sender, RoutedEventArgs e) //Add Task button was clicked
        {
            addTaskWindow add = new addTaskWindow(showBoardVM.Board, showBoardVM.BackupBoard);
            add.ShowDialog();

        }

        private void removeColumn_Click(object sender, RoutedEventArgs e) //remove column button was clicked
        {

            showBoardVM.removeColumn(showBoardVM.Board);
        }

        private void moveColumnRight_Click(object sender, RoutedEventArgs e)//moveColumnRightbutton was clicked
        {
            showBoardVM.moveColumnRight();
        }
        private void moveColumnLeft_Click(object sender, RoutedEventArgs e)//moveColumnLeft button was clicked
        {
            showBoardVM.moveColumnLeft();
        }
        private void addColumn_Click(object sender, RoutedEventArgs e)//addColumn button was clicked
        {
            addColumnWindow add = new addColumnWindow(showBoardVM.Board,showBoardVM.BackupBoard);
            add.ShowDialog();
        }

        private void filterTask_Click(object sender, RoutedEventArgs e)//filterTask button was clicked
        {
            showBoardVM.Filter(); //Backup the board before filtering tasks by removing them from it 
            filterTaskWindow filterTaskWindow = new filterTaskWindow(showBoardVM.Board);
            filterTaskWindow.ShowDialog();
        }

        private void dueDateSort_Click(object sender, RoutedEventArgs e)//dueDateSort  button was clicked
        {
            showBoardVM.dueDateSort();
        }

        private void limitColumnTasks_Click(object sender, RoutedEventArgs e)//limitColumnTasks button was clicked
        {
            if(showBoardVM.BackupBoard==null)
            {
                limitColumnTasksWindow limitColumn = new limitColumnTasksWindow(user, this.showBoardVM.SelectedColumn, null);
                limitColumn.ShowDialog();
            }
            else
            {
                limitColumnTasksWindow limitColumn = new limitColumnTasksWindow(user, this.showBoardVM.SelectedColumn,showBoardVM.BackupBoard.getColumn(this.showBoardVM.SelectedColumn.Ordinal));
                limitColumn.ShowDialog();
            }
        }

        private void changeColumnName_Click(object sender, RoutedEventArgs e)//changeColumnName button was clicked
        {
            changeColumnNameWindow changeColumnName = new changeColumnNameWindow(user, this.showBoardVM.SelectedColumn,this.showBoardVM.BackupBoard);
            changeColumnName.ShowDialog();

        }

        private void Logout_Click(object sender, RoutedEventArgs e)//Logout button was clicked
        {
            showBoardVM.logout();
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
        private void tasksList_DoubleClick(object sender, RoutedEventArgs e)//Double clicked a task from a column
        {
            showBoardVM.tasksList_DoubleClick();


        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)//Clear filter button was clicked
        {

            showBoardVM.ClearFilter();
        }

    }
}
