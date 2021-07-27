using Presentation.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Task = IntroSE.Kanban.Backend.ServiceLayer.Task;

namespace Presentation
{
    public class BackendController
    {
        public IService Service { get; private set; }
        public BackendController(IService service)
        {
            this.Service = service;
        }

        public BackendController() //Constructor
        {
            this.Service = new Service();
            Service.LoadData();
        }
        public Board getBoard(string email)
        {
            return Service.GetBoard(email).Value;
        }
        public ObservableCollection<ColumnModel> getColumns(string email)
        {
            IReadOnlyCollection<string> columnNames = Service.GetBoard(email).Value.getColumnsName();
            ObservableCollection<ColumnModel> columns = new ObservableCollection<ColumnModel>();
            foreach (string item in columnNames)
            {
                Column temp = Service.GetColumn(email, item).Value;
                ColumnModel toAdd = new ColumnModel(this, temp, email, temp.ordinal);
                ObservableCollection<TaskModel> TasksModel = new ObservableCollection<TaskModel>();
                foreach (var taskService in temp.Tasks)
                {
                    TasksModel.Add(new TaskModel(this, taskService));
                }
                toAdd.Tasks = TasksModel;
                columns.Add(toAdd);
            }
            return columns;
        }

        // first screen
        public UserModel Login(string username, string password)
        {
            Response<User> response = Service.Login(username, password);
            if (response.ErrorOccured) //if login failed
                throw new Exception(response.ErrorMessage);
            else
                return new UserModel(this, username);
        }
        public UserModel Register(string email, string password, string nickname)
        {
            Response response = Service.Register(email, password, nickname);
            if (response.ErrorOccured)//if register failed
                throw new Exception(response.ErrorMessage);
            else
                return new UserModel(this, email);

        }
        public UserModel Register(string email, string password, string nickname, string emailHost)
        {
            Response response = Service.Register(email, password, nickname, emailHost);
            if (response.ErrorOccured)//if register failed
                throw new Exception(response.ErrorMessage);
            else
                return new UserModel(this, email);

        }
        // second screen
        public void addTask(string email, string title, string description, DateTime dueDate, ColumnModel column, ColumnModel backupColumn)
        {
            Response<Task> response = Service.AddTask(email, title, description, dueDate);
            if (response.ErrorOccured)//if addtask failed
                throw new Exception(response.ErrorMessage);
            else //task was added
            {
                Task temp = response.Value;
                TaskModel toAdd = new TaskModel(this, temp);

                toAdd.BackGroundColor = new System.Windows.Media.SolidColorBrush(Colors.Blue);

                column.Tasks.Add( toAdd);
                if (backupColumn != null) //if tasks are currently filtered and there is a need to add the task in the backup board
                {
                    TaskModel toAddBackUp = new TaskModel(this, temp);
                    toAdd.BackGroundColor = new System.Windows.Media.SolidColorBrush(Colors.Blue);
                    backupColumn.Tasks.Add(toAddBackUp);
                }
            }

        }
        public ColumnModel addColumn(string email, int columnOrdinal, string name,BoardModel board)
        {
            Response<Column> response = Service.AddColumn(email, columnOrdinal, name);
            if (response.ErrorOccured) //if addColumn failed
                throw new Exception(response.ErrorMessage);
            else //if column was added
            {
                Column temp = response.Value;
                ColumnModel column = new ColumnModel(this, temp, email, columnOrdinal);
                for(int i=board.Columns.Count-1; i>=columnOrdinal;i--) //Update the board of the Presentation
                {
                    board.Columns[i].Ordinal = i + 1;
                }
                return column;
            }
        }
        public void changeColumnName(ColumnModel column, string email, int columnOrdinal, string name,ColumnModel backupColumn)
        {
            Response response = Service.ChangeColumnName(email, columnOrdinal, name);
            if (response.ErrorOccured) //if change failed
                throw new Exception(response.ErrorMessage);
            else //if name was changed
            {
                column.Name = name;
                if (backupColumn != null) //Update the backup column if it exists
                    backupColumn.Name = name;
            }

        }
        public void removeColumn(string email, int columnOrdinal, UserModel user, BoardModel board, ColumnModel column)
        {
            Response res = Service.RemoveColumn(email, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            else//updating in Presentation
            {
                ///update other columns ordinals
                for (int i = column.Ordinal + 1; i < board.Columns.Count; i++)
                {
                    board.getColumn(i).Ordinal--;
                }
                
                if (column.Tasks.Count==0)
                {
                    board.Columns.Remove(column);
                }
                
                else
                {
                    if (column.Ordinal==0)//have to move his tasks to the right column
                    {
                        column.Ordinal = -1;
                        foreach (TaskModel task in column.Tasks)
                        {
                            board.getColumn(0).Tasks.Add(task);
                        }
                        board.Columns.Remove(column);
                    }
                    else
                    {
                        foreach (TaskModel task in column.Tasks)//have to move his tasks to the left column
                        {
                            board.getColumn(column.Ordinal-1).Tasks.Add(task);
                        }
                        board.Columns.Remove(column);
                    }
                }
                
            }
        }
        public void moveColumnRight(string email, int columnOrdinal, ColumnModel columnToMove, ColumnModel columnFriend, BoardModel board)
        {
            Response<Column> response = Service.MoveColumnRight(email, columnOrdinal);
            if (response.ErrorOccured) //if column moving failed
                throw new Exception(response.ErrorMessage);
            else //if column was moved
            {
                columnToMove.Ordinal++;
                columnFriend.Ordinal--;
                board.Columns.Remove(columnToMove);
                board.Columns.Remove(columnFriend);
                board.Columns.Insert(columnFriend.Ordinal, columnFriend);
                board.Columns.Insert(columnToMove.Ordinal, columnToMove);
               
            }

        }
        public void moveColumnLeft(string email, int columnOrdinal, ColumnModel columnToMove, ColumnModel columnFriend, BoardModel board)
        {
            Response<Column> response = Service.MoveColumnLeft(email, columnOrdinal);
            if (response.ErrorOccured)//if column moving failed
                throw new Exception(response.ErrorMessage);
            else//if column was moved
            {
                columnToMove.Ordinal--;
                columnFriend.Ordinal++;
                board.Columns.Remove(columnToMove);
                board.Columns.Remove(columnFriend);
                board.Columns.Insert(columnToMove.Ordinal, columnToMove);
                board.Columns.Insert(columnFriend.Ordinal, columnFriend);
            }
        }
        public void limitColumnTasks(ColumnModel column, string email, int columnOrdinal, int limit,ColumnModel backupColumn)
        {
            Response response = Service.LimitColumnTasks(email, columnOrdinal, limit);
            if (response.ErrorOccured) 
                throw new Exception(response.ErrorMessage);
            else //if limit was set
            {
                column.Limit = limit;
                if(backupColumn!=null)//Update the backup column if it exists
                {
                    backupColumn.Limit = limit;
                }
            }
        }
        public void logout(string email)
        {
            Response response = Service.Logout(email);
            if (response.ErrorOccured) //if logout failed
                throw new Exception(response.ErrorMessage);

        }
        
        public void editTaskTitle(string email, int columnOrdinal, int taskId, string title, TaskModel task,TaskModel backupTask)
        {
            Response response = Service.UpdateTaskTitle(email, columnOrdinal, taskId, title);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
            else
            {
                //Updating the board of the presentation
                task.Title = title;
                if (backupTask != null)//Update the task of the backup board as well if it exists (if tasks are currently filtered)
                    backupTask.Title = title;
            }

        }
        public void editTaskDescription(string email, int columnOrdinal, int taskId, string description, TaskModel task,TaskModel backupTask)
        {
            Response response = Service.UpdateTaskDescription(email, columnOrdinal, taskId, description);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
            else
            {
                //Updating the board of the presentation
                task.Description = description;
                if (backupTask != null)//Update the task of the backup board as well if it exists (if tasks are currently filtered)
                    backupTask.Description = description;
            }
        }
        public void editTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate, TaskModel task,TaskModel backupTask)
        {
            Response response = Service.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
            else
            {
                //Updating the board of the presentation
                task.DueDate = dueDate;
                if (backupTask != null)//Update the task of the backup board as well if it exists (if tasks are currently filtered)
                    backupTask.DueDate = dueDate;
            }
        }
        public void deleteTask(string email, int ordinal, int taskId, BoardModel board,BoardModel backupBoard)
        {
            Response response = Service.DeleteTask(email, ordinal, taskId);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
            else
            {
                //Updating the board of the presentation
                ColumnModel columnFromToRemove = board.getColumn(ordinal);
                columnFromToRemove.Tasks.Remove(columnFromToRemove.getTask(taskId));
                if(backupBoard!=null)//Delete the task from the backup board as well if it exists (if tasks are currently filtered)
                {
                    ColumnModel columnFromToRemoveB = backupBoard.getColumn(ordinal);
                    columnFromToRemoveB.Tasks.Remove(columnFromToRemoveB.getTask(taskId));
                }
            }
        }
        public void advanceTask(string email, int columnOrdinal, int taskId, BoardModel board,BoardModel backupBoard)
        {
            Response response = Service.AdvanceTask(email, columnOrdinal, taskId);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
            else
            {
                //Updating the board of the presentation
                ColumnModel currentColumn = board.getColumn(columnOrdinal);
                ColumnModel nextColumn = board.getColumn(columnOrdinal + 1);
                TaskModel toRemove = currentColumn.getTask(taskId); //Will never be null
                currentColumn.Tasks.Remove(toRemove); //Updating the coulmn in model of Presentation
                nextColumn.Tasks.Add(toRemove);
                if(backupBoard!=null) //Updating the backup board if it exists
                {
                    ColumnModel currentColumnB = backupBoard.getColumn(columnOrdinal);
                    ColumnModel nextColumnB = backupBoard.getColumn(columnOrdinal + 1);
                    TaskModel toRemoveB = currentColumnB.getTask(taskId); //Will never be null
                    currentColumnB.Tasks.Remove(toRemoveB); //Updating the coulmn in model of Presentation
                    nextColumnB.Tasks.Add(toRemoveB);
                }
            }
            

        }
        public void assignTask(string email, TaskModel task, ColumnModel column, string emailAssignee,TaskModel backupTask)
        {
            Response response = Service.AssignTask(email, column.Ordinal, task.TaskId, emailAssignee);
            if(response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            else
            {
                //Updating the board of the presentation
                task.EmailAssignee = emailAssignee;

                if (email.Equals(emailAssignee))
                    task.BackGroundColor = new System.Windows.Media.SolidColorBrush(Colors.Blue);
                else
                    task.BackGroundColor = new System.Windows.Media.SolidColorBrush(Colors.White);

                if (backupTask != null) //Updating the backup board if it exists
                {
                    backupTask.EmailAssignee = emailAssignee;
                    if (email.Equals(emailAssignee))
                        backupTask.BackGroundColor = new System.Windows.Media.SolidColorBrush(Colors.Blue);
                    else
                        backupTask.BackGroundColor = new System.Windows.Media.SolidColorBrush(Colors.White);

                }
            }
        }


    }
}