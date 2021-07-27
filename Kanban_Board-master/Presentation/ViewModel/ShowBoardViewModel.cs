using Presentation.Model;
using Presentation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    public class ShowBoardViewModel : NotifiableObject
    {

        //Properties---------------------------------------------------------------------------------------------------------
        private BackendController controller;
        private BoardModel board;
        public BoardModel Board
        {
            get
            {
                return board;
            }
            set
            {
                board = value;
                RaisePropertyChanged("Board");
            }
        }
        public UserModel User { get; private set; }
        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }
        private string message;
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
        private ColumnModel _selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                _selectedColumn = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedColumn");
            }
        }
        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                RaisePropertyChanged("SelectedTask");
            }
        }
        private BoardModel backupBoard;
        public BoardModel BackupBoard
        {
            get
            {
                return backupBoard;
            }
            set

            {
                backupBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("BackupBoard");
            }
        }
       
        //Constructor---------------------------------------------------------------------------------------------------------

        public ShowBoardViewModel(BackendController controller, UserModel user, BoardModel board)
        {
            this.User = user;
            this.Board = board;
            this.controller = controller;
            this.SetTasksBorderColors();
        }
        //Methods---------------------------------------------------------------------------------------------------------

        public void Filter() //Copying the board for a new backup board - only used before filtering tasks
        {
            if (backupBoard == null) //Only backup if there isnt any backup board already
                this.backupBoard = new BoardModel(this.Board);
        }


        public void removeColumn(BoardModel board)
        {
            Message = "";
            try
            {
                if(SelectedColumn!=null)              
                {
                    controller.removeColumn(User.Email, SelectedColumn.Ordinal, User, board, SelectedColumn);

                }

            }
            catch (Exception e)
            {
                Message = e.Message; //Raise error message
            }
        }
        public void dueDateSort() //Sorting the tasks by their due date
        {
            foreach (ColumnModel column in Board.Columns)
            {
                column.Tasks = new System.Collections.ObjectModel.ObservableCollection<TaskModel>(column.Tasks.OrderBy(T => T.DueDate));
            }
        }
        public void SetTasksBorderColors() //Colors the task's border
        {
            foreach (ColumnModel column in Board.Columns)
            {
                foreach (TaskModel task in column.Tasks)
                {
                    if (task.EmailAssignee.Equals(User.Email))
                         task.BackGroundColor = new SolidColorBrush(Colors.Blue);
                    else
                        task.BackGroundColor = new SolidColorBrush(Colors.White);
                }
            }
        }
        public void moveColumnRight()
        {

            try
            {
                 controller.moveColumnRight(User.Email, SelectedColumn.Ordinal, SelectedColumn,Board.getColumn(SelectedColumn.Ordinal+1), Board);
                
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }
        public void ClearFilter() //Restoring the board to show all of its tasks
        {
            if (this.BackupBoard != null) //Only restore the board if there is a backup board
            {
                Board = BackupBoard;
                BackupBoard = null;
                this.SetTasksBorderColors();
            }

        }
        public void logout()
        {
            controller.logout(User.Email);
        }
        public void moveColumnLeft()
        {

            try
            {
                controller.moveColumnLeft(User.Email, SelectedColumn.Ordinal, SelectedColumn, Board.getColumn(SelectedColumn.Ordinal - 1), Board);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }
        private ColumnModel getSelectedColumn(TaskModel SelectedTask)
        {
            ColumnModel columnOfSelectedTask = null;
            foreach (ColumnModel column in Board.Columns)
            {
                foreach (TaskModel task in column.Tasks)
                {
                    if (task.Equals(SelectedTask))
                    {
                        columnOfSelectedTask = column;

                    }
                }
            }
            return columnOfSelectedTask;
        }
        public void tasksList_DoubleClick()
        {
            ColumnModel columnOfSelectedTask = getSelectedColumn(SelectedTask);
            if (SelectedTask != null) //Only act if the double click was on a task and therefore the selected task isnt null
            {
                ShowTaskWindow showTaskWindow = new ShowTaskWindow(SelectedTask,User, columnOfSelectedTask,Board,BackupBoard);
                showTaskWindow.ShowDialog();
            }
        }





    }
}