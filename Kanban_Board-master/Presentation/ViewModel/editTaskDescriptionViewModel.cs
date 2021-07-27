using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class editTaskDescriptionViewModel : NotifiableObject
    {
        //Properties -------------------------------------------------------------------------------------------------------------

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
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
        public editTaskDescriptionViewModel(UserModel user, ColumnModel column, TaskModel task,TaskModel backupTask)
        {
            this.user = user;
            this.column = column;
            this.task = task;
            this.backupTask = backupTask;
        }
        //Methods -------------------------------------------------------------------------------------------------------------

        public bool editTaskDescription() {
            Message = "";
            try
            {
                task.Controller.editTaskDescription(user.Email, column.Ordinal, task.TaskId, Description, task,this.backupTask);
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
