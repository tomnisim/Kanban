using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class filterTaskViewModel : NotifiableObject
    {
        //Properties-------------------------------------------------------------------------------------------------------------

        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
                RaisePropertyChanged("Key");
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
        private BoardModel board;
        //Constructor-------------------------------------------------------------------------------------------------------------

        public filterTaskViewModel(BoardModel board)
        {
            this.board = board;
        }
        //Methods -------------------------------------------------------------------------------------------------------------

        public bool filterTasks(BoardModel board)
        {
            if(!string.IsNullOrWhiteSpace(Key)) //Filtering key cant be null or white spaces
            {
                //Removing all the tasks that doesnt contain Key in their title or description from the board
                foreach (ColumnModel column in board.Columns) 
                {
                    ObservableCollection<TaskModel> copy = new ObservableCollection<TaskModel>();
                    foreach (TaskModel task in column.Tasks)
                    {
                        copy.Add(task);
                    }
                    foreach (TaskModel task in copy)
                    {
                        if (((!task.Title.Contains(Key)) & ((task.Description != null) && (!task.Description.Contains(Key)))) | (!task.Title.Contains(Key)) & (task.Description == null))
                        {
                            column.Tasks.Remove(task);
                        }

                    }

                }
                return true;
            }
            //Key is null or whitespaces
            Message = "please enter text to filter"; //Raise error for input 
            return false; //Failed to filter
        }
     }
}

