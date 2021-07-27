using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    class addColumnViewModel : NotifiableObject
    {
        //Properties------------------------------------------------------------------
        private BackendController controller;
        private BoardModel board;
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
        private int ordinal;
        public int Ordinal
        {
            get
            {
                return ordinal;
            }
            set
            {
                ordinal = value;
                RaisePropertyChanged("Ordinal");
            }
        }
        //Constructor------------------------------------------------------------------
        public addColumnViewModel(BoardModel board)
        {
            this.board = board;
            this.controller = board.Controller;
        }
        //Methods------------------------------------------------------------------
        public ColumnModel addColumn()
        {
            Message = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(Name)) //Column name cant be empty or white spaces
                    return controller.addColumn(board.user.Email, Ordinal, Name,board);
                else
                {
                    Message = "Name can not be empty";
                    return null;
                }
            }
            catch (Exception e)
            {
                Message = e.Message; //Raise error message
                return null;
            }


        }
    }
}