using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class BoardModel : NotifiableModelObject
    {
        // list of column, User
        public UserModel user;
        private ObservableCollection<ColumnModel> columns;
        public ObservableCollection<ColumnModel> Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                RaisePropertyChanged("Columns");
            }
        }

        //Constructors---------------------------------------------------------
        public BoardModel (BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            this.Columns = controller.getColumns(user.Email);
        }
        public BoardModel (BoardModel boardToCopy) : base(boardToCopy.Controller) //Copy constructor used for backup board
        {
            UserModel user = new UserModel(boardToCopy.Controller, boardToCopy.user.Email);
            this.user = user;
            this.Columns = new ObservableCollection<ColumnModel>();
            
            foreach(ColumnModel column in boardToCopy.columns)
            {
                ColumnModel toAddColumn = new ColumnModel(boardToCopy.Controller,column.Name,column.Limit,boardToCopy.user.Email,column.Ordinal);
                toAddColumn.Tasks = new ObservableCollection<TaskModel>();
                foreach (TaskModel task in column.Tasks)
                {
                    TaskModel toAddTask = new TaskModel(boardToCopy.Controller, user.Email, task.CreationTime, task.DueDate, task.Title, task.Description, task.TaskId);
                    toAddColumn.Tasks.Add(toAddTask);
                }
                this.Columns.Add(toAddColumn);
            }
        }

        public BoardModel(BackendController controller, UserModel user, Board board) : this(controller, user)
        {
            this.user = user;
            this.Columns = controller.getColumns(user.Email);
        }
        //Methods---------------------------------------------------------
        public void addTask(TaskModel task)
        {
            Columns[0].addTask(task);
        }

        public ColumnModel getColumn(int ordinal)
        {
            if (ordinal>=0 & ordinal < Columns.Count)
            {
                foreach (ColumnModel item in Columns)
                {
                    if (ordinal == item.Ordinal)
                        return item;
                }
                
            }
            return null;
        }

        public ColumnModel getColumn(string columnName)
        {
            foreach (var item in Columns)
            {
                if (item.Name.Equals(columnName))
                {
                    return item;
                }
            }
            return null; //If column was not found
        }
    }
}
