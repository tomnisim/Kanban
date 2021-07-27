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
    /// Interaction logic for editTaskDescriptionWindow.xaml
    /// </summary>
    public partial class editTaskDescriptionWindow : Window
    {

        private editTaskDescriptionViewModel editTaskDescriptionVM;
        private TaskModel task;
        private TaskModel backupTask;

        public editTaskDescriptionWindow(UserModel user, ColumnModel column, TaskModel task,TaskModel backupTask) //Constructor
        {
            InitializeComponent();
            editTaskDescriptionVM = new editTaskDescriptionViewModel(user, column, task,backupTask);
            this.DataContext = editTaskDescriptionVM;
            this.task = task;
            this.backupTask = backupTask;
        }

        private void OK_Click(object sender, RoutedEventArgs e) //OK Button was clicked
        {
            bool isEditted = editTaskDescriptionVM.editTaskDescription();
            if(isEditted) //Only close the window if the action was succecsfully done
                this.Close();
        }
    }
}
