using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class changeColumnNameViewModel : NotifiableObject
    {
        //Properties-------------------------------------------------------------------------------

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
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

        private BackendController controller;
        //Constructor-------------------------------------------------------------------------------

        public changeColumnNameViewModel(BackendController controller)
        {
            this.controller = controller;
        }
        //Methods-------------------------------------------------------------------------------

        public bool changeColumnName(BoardModel board, ColumnModel column,ColumnModel backupColumn)
        {
            Message = "";
            try
            {
                if(!string.IsNullOrWhiteSpace(Name)) //Column name cant be null or white spaces
                {
                    controller.changeColumnName(column, board.user.Email, column.Ordinal, Name,backupColumn);
                    return true;
                }
                else
                {
                    Message = "plese enter new name"; //Raise error message
                    return false; 
                }

            }
            catch (Exception e)
            {
                Message = e.Message; //Raise error message
                return false;
            }
        }
    }
}