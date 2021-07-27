using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class limitColumnTasksViewModel : NotifiableObject
    {
        //Properties -------------------------------------------------------------------------------------------------------------

        private BackendController controller;
        private UserModel user;
        private ColumnModel column;
        private ColumnModel backupColumn;
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
        private int limit;
        public int Limit
        {
            get
            {
                return limit;
            }
            set
            {
                limit = value;
                RaisePropertyChanged("Limit");
            }
        }
        //Constructor -------------------------------------------------------------------------------------------------------------

        public limitColumnTasksViewModel(BackendController controller,UserModel user ,ColumnModel column,ColumnModel backupColumn)
        {
            this.user = user;
            this.column = column;
            this.controller = controller;
            this.backupColumn = backupColumn;
        }
        //Methods -------------------------------------------------------------------------------------------------------------

        public bool limitColumn()
        {
            Message = "";
            try
            {
                controller.limitColumnTasks(column, user.Email, column.Ordinal, Limit, this.backupColumn);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
            return true;
        }
    }
}