using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class addTaskViewModel : NotifiableObject
    {
        //Properties-------------------------------------------------------------------------------
        BackendController controller;
        BoardModel board;
        private DateTime dueDate=DateTime.Now;
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
        //Constructor-------------------------------------------------------------------------------
        public addTaskViewModel(BoardModel board)
        {
            this.board = board;
            this.controller = board.Controller;
        }
        //Methods-------------------------------------------------------------------------------

        public bool addTask(ColumnModel column, ColumnModel columnBackup)
        {
            Message = "";
            try
            {
                controller.addTask(board.user.Email,Title, Description, DueDate, column, columnBackup);
            }
            catch (Exception e)
            {
                Message = e.Message; //Raise error message if adding was unsuccesful
                return false;
            }
            return true;//Added sucesfully
        }
    }
}
