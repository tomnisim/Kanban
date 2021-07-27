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
    /// Interaction logic for editTaskTitleWindow.xaml
    /// </summary>
    public partial class editTaskTitleWindow : Window
    {
        private editTaskTitleViewModel editTaskTitleVM;
        private TaskModel task;
        private TaskModel backupTask;
        public editTaskTitleWindow(UserModel user, ColumnModel column, TaskModel task,TaskModel backupTask) //Constructor
        {
            InitializeComponent();
            this.task = task;
            this.backupTask = backupTask;
            editTaskTitleVM = new editTaskTitleViewModel(user, column, task, backupTask);
            this.DataContext = editTaskTitleVM;
        }

        private void OK_Click(object sender, RoutedEventArgs e) //OK Button was clicked
        {
            bool isEditted = editTaskTitleVM.editTaskTitle();
            if(isEditted) //Only close this window if the task title was editted succesfully
                this.Close();
        }
    }
}