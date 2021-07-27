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
    /// Interaction logic for assignTaskWindow.xaml
    /// </summary>
    public partial class assignTaskWindow : Window
    {
        TaskModel task;
        TaskModel taskBackup;
        ColumnModel column;
        assignTaskViewModel vm;
        public assignTaskWindow(TaskModel selectedTask, ColumnModel column, TaskModel taskBackup)
        {
            InitializeComponent();
            vm = new assignTaskViewModel(column.Controller);
            this.DataContext = vm;
            this.task = selectedTask;
            this.column = column;
            this.taskBackup = taskBackup;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            bool isAssigned = vm.AssignTask(this.task, column.email, column,taskBackup);
            if (isAssigned) //Only if the task was assigned close this window
            {
                this.Close();
            }
        }
    }
}
