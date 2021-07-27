using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class ShowTaskViewModel : NotifiableObject
    {

        //Properties------------------------------------------------------------------------
        public BackendController Controller { get; private set; }
        public TaskModel task { get; private set; }
        public ColumnModel column { get; private set; }
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
        //Constrcutor------------------------------------------------------------------------

        public ShowTaskViewModel(TaskModel task, ColumnModel column)
        {
            this.Controller = task.Controller;
            this.task = task;
            this.column = column;
        }
        //Methods------------------------------------------------------------------------

        public bool advanceTask(string email,BoardModel board,BoardModel backupBoard)
        {
            try
            {
                this.Controller.advanceTask(email, column.Ordinal, task.TaskId,board,backupBoard);

            }
            catch(Exception e)
            {
                Message = e.Message; //Raise error message
                return false; //task wasnt advanced
            }
            return true; //task was advanced succcesfully
        }
        public bool deleteTask(BoardModel board,BoardModel backupBoard)
        {
            Message = "";
            try
            {
                 Controller.deleteTask(board.user.Email, column.Ordinal, task.TaskId,board,backupBoard);
            }
            catch (Exception e)
            {
                Message = e.Message; //Raise error message
                return false;  //task was not deleted
            }
            return true; //Deleted succesfully
        }

    }
}