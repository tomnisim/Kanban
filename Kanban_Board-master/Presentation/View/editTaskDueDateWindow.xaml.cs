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
    /// Interaction logic for editTaskDueDateWindow.xaml
    /// </summary>
    public partial class editTaskDueDateWindow : Window
    {
        private editTaskDueDateViewModel editTaskDueDateVM;
        private TaskModel task;
        private UserModel user;
        private ColumnModel column;
        private TaskModel backupTask;
        public editTaskDueDateWindow(TaskModel task, UserModel user, ColumnModel column,TaskModel backupTask) //Constructor
        {
            InitializeComponent();
            editTaskDueDateVM = new editTaskDueDateViewModel();
            this.DataContext = editTaskDueDateVM;
            this.task = task;
            this.column = column;
            this.user = user;
            this.backupTask = backupTask;
        }

        private void editTaskDueDate_Click(object sender, RoutedEventArgs e)
        {
            bool isEditted=editTaskDueDateVM.editTaskDueDate(task, user, column,backupTask);
            if(isEditted) //Only close the window if the task was editted sucessfully
                this.Close();
        }
    }
}