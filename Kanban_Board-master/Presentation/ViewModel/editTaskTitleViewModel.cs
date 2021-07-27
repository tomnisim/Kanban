using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class editTaskTitleViewModel : NotifiableObject
    {
        //Properties -------------------------------------------------------------------------------------------------------------

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string message = "";
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                RaisePropertyChanged("Message");
            }
        }
        private UserModel user;
        private ColumnModel column;
        private TaskModel task;
        private TaskModel backupTask;
        //Constructor -------------------------------------------------------------------------------------------------------------

        public editTaskTitleViewModel(UserModel user, ColumnModel column, TaskModel task,TaskModel backupTask)
        {
            this.user = user;
            this.column = column;
            this.task = task;
            this.backupTask = backupTask;
        }
        //Methods -------------------------------------------------------------------------------------------------------------

        public bool editTaskTitle()
        {
            Message = "";
            try
            {
                task.Controller.editTaskTitle(user.Email, column.Ordinal, task.TaskId, Title, task,backupTask);
            }
            catch (Exception e)
            {
                Message = e.Message; //Raise error message
                return false;
            }
            return true;
        }
    }
}