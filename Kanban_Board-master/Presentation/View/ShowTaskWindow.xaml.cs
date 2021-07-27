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
    /// Interaction logic for ShowTask.xaml
    /// </summary>
    public partial class ShowTaskWindow : Window
    {
        
        private ShowTaskViewModel StVM;
        private ColumnModel column;
        private UserModel user;
        private TaskModel task; //Selected task
        private BoardModel board;
        private BoardModel backupBoard;
        public ShowTaskWindow(TaskModel task,UserModel user ,ColumnModel column,BoardModel board,BoardModel backupBoard)
        {
            InitializeComponent();
            StVM = new ShowTaskViewModel(task, column);
            this.DataContext = StVM;
            this.task = task;
            this.column = column;
            this.user = user;
            this.board = board;
            this.backupBoard = backupBoard;
        }

        private void editDescription_Click(object sender, RoutedEventArgs e)
        {
            if (backupBoard != null)
            {
                editTaskDescriptionWindow editTaskDescription = new editTaskDescriptionWindow(user, column, task, backupBoard.getColumn(column.Ordinal).getTask(task.TaskId));
                editTaskDescription.Show();
            }
            else
            {
                editTaskDescriptionWindow editTaskDescription = new editTaskDescriptionWindow(user, column, task, null);
                editTaskDescription.Show();
            }
            this.Close();
        }

        private void editTitle_Click(object sender, RoutedEventArgs e)
        {
            if(backupBoard!=null)
            {
                editTaskTitleWindow editTaskTitle = new editTaskTitleWindow(user, column, task,backupBoard.getColumn(column.Ordinal).getTask(task.TaskId));
                editTaskTitle.Show();
                this.Close();
            }
            else
            {
                editTaskTitleWindow editTaskTitle = new editTaskTitleWindow(user, column, task,null);
                editTaskTitle.Show();
                this.Close();
            }
            

        }

        private void editDueDate_Click(object sender, RoutedEventArgs e)
        {
            if(backupBoard!=null)
            {
                editTaskDueDateWindow editTaskDueDate = new editTaskDueDateWindow(task, user, column, backupBoard.getColumn(column.Ordinal).getTask(task.TaskId));
                editTaskDueDate.Show();
                this.Close();
            }
            else
            {
                editTaskDueDateWindow editTaskDueDate = new editTaskDueDateWindow(task, user, column, null);
                editTaskDueDate.Show();
                this.Close();
            }
            
        }

        private void assignTask_Click(object sender, RoutedEventArgs e)
        {
            if (backupBoard != null)
            {
                assignTaskWindow assignTask = new assignTaskWindow(task, column, backupBoard.getColumn(column.Ordinal).getTask(task.TaskId));
                assignTask.ShowDialog();//Open the assign task window
            }
            else
            {
                assignTaskWindow assignTask = new assignTaskWindow(task, column, null);
                assignTask.ShowDialog();//Open the assign task window
            }
            this.Close();

        }

        private void deleteTask_Click(object sender, RoutedEventArgs e)
        {
            bool isDeleted = StVM.deleteTask(board,backupBoard);
            if(isDeleted)
                this.Close();

        }

        private void advanceTask_Click(object sender, RoutedEventArgs e)
        {
            bool isAdvanced = StVM.advanceTask(user.Email,board,backupBoard);
            if(isAdvanced)
                this.Close();
        }

      
    }
}