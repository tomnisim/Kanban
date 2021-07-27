using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    class editTaskDueDateViewModel : NotifiableObject
    {
        //Properties -------------------------------------------------------------------------------------------------------------

        private DateTime dueDate = DateTime.Now;
        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
                RaisePropertyChanged("DueDate");
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
        //Only using default empty constructor
        //Methods -------------------------------------------------------------------------------------------------------------

        public bool editTaskDueDate(TaskModel task, UserModel user, ColumnModel column,TaskModel backupTask)
        {
            Message = "";
            try
            {
                task.Controller.editTaskDueDate(user.Email, column.Ordinal, task.TaskId, DueDate, task, backupTask);

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