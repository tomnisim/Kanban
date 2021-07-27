using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class assignTaskViewModel : NotifiableObject
    {
        //Properties-------------------------------------------------------------------------------

        private BackendController Controller;
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
        private string emailAssignee;
        public string EmailAssignee
        {
            get
            {
                return emailAssignee;
            }
            set
            {
                emailAssignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }
        //Constructor-------------------------------------------------------------------------------

        public assignTaskViewModel(BackendController controller)
        {
            this.Controller = controller;
        }

        //Methods-------------------------------------------------------------------------------

        public bool AssignTask(TaskModel task,string email,ColumnModel column, TaskModel backupTask)
        {
            try
            {
                this.Controller.assignTask(email, task,column, EmailAssignee, backupTask);

            }
            catch (Exception e)
            {
                Message = e.Message; //Raise error message
                return false; //Assign failed
            }
            return true; //Assigned succesfully
        }
    }
}
