using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BoardService");
        private BusinessLayer.BoardController bc;
        public BoardService(BusinessLayer.BoardController bc)
        {
            this.bc = bc;
        }
        public Response<Board> GetBoard(string email)
        {
            try
            {
                bc.getBoard(email);
            }
            catch (Exception e)
            {
                return new Response<Board>(e.Message);
            }
            return new Response<Board>(new Board(bc.getBoard(email).getColumnNames(), bc.getBoard(email).getEmailCreator()));


        }
        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            {
                try
                {
                    bc.getBoard(email).editTaskDueDate(taskId, columnOrdinal, dueDate, email);
                }
                catch (Exception e)
                {
                    return new Response(e.Message);
                }
                log.Info("Task due date was updated succesfully");
                return new Response(); //Successfull Update
            }
        }
        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            try
            {
                bc.getBoard(email).editTaskTitle(taskId, columnOrdinal, title, email);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            log.Info("Task title was updated succesfuclly");
            return new Response();//Successfull Update
        }
        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            try
            {

                bc.getBoard(email).editTaskDescription(taskId, columnOrdinal, description, email);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            log.Info("Task description was updated succesfuclly");
            return new Response();//Successfull Update
        }
        public Response AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            try
            {
                bc.getBoard(email).advanceTask(taskId, columnOrdinal, email);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }

            log.Info("Task was advanced succesfully");
            return new Response();//Successfull Advance
        }
        public Response removeColumn(string email, int columnOrdinal)
        {
            try
            {
                bc.getBoard(email).removeColumn(columnOrdinal, email);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            return new Response();//Successfull remove
        }
        public Response<Column> addColumn(string email, int columnOrdinal, string Name)
        {

            try
            {
                bc.getBoard(email).addColumn(columnOrdinal, Name, email);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
            IReadOnlyCollection<Task> tasks = convertToService(bc.getBoard(email.ToLower()).getColumn(columnOrdinal).getTasksForColumns());
            Column toReturn = new Column(tasks, bc.getBoard(email.ToLower()).getColumn(columnOrdinal).getName(), bc.getBoard(email.ToLower()).getColumn(columnOrdinal).getLimit(), columnOrdinal);
            return new Response<Column>(toReturn);//Successfull Add - returned the added column in Response
        }



        public Response<Column> moveColumnRight(string email, int columnOrdinal)
        {

            try
            {
                bc.getBoard(email).MoveColumnRight(columnOrdinal, email);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
            //Creating a ServiceLayer.Column to return in the response
            IReadOnlyCollection<Task> tasks = convertToService(bc.getBoard(email.ToLower()).getColumn(columnOrdinal).getTasksForColumns());
            Column toReturn = new Column(tasks, bc.getBoard(email.ToLower()).getColumn(columnOrdinal).getName(), bc.getBoard(email.ToLower()).getColumn(columnOrdinal).getLimit(), columnOrdinal + 1);
            return new Response<Column>(toReturn);//Successfull Move - returned the moved column in Response
        }

        public Response DeleteTask(string email, int columnOrdinal, int taskId)
        {
            try
            {
                bc.getBoard(email).DeleteTask(columnOrdinal, taskId,email);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response<Column> moveColumnLeft(string email, int columnOrdinal)
        {

            try
            {
                bc.getBoard(email).MoveColumnLeft(columnOrdinal, email);
            }
            catch (Exception e)
            {

                return new Response<Column>(e.Message);
            }
            //Creating a ServiceLayer.Column to return in the response
            IReadOnlyCollection<Task> tasks = convertToService(bc.getBoard(email.ToLower()).getColumn(columnOrdinal).getTasksForColumns());
            Column toReturn = new Column(tasks, bc.getBoard(email.ToLower()).getColumn(columnOrdinal).getName(), bc.getBoard(email.ToLower()).getColumn(columnOrdinal).getLimit(), columnOrdinal - 1);
            return new Response<Column>(toReturn);//Successfull Move - returned the moved column in Response
        }
        private IReadOnlyCollection<Task> convertToService(List<BusinessLayer.Task> tasks) //Helping function to covert list of business Tasks to a list of data Tasks
        {
            List<Task> tasksSer = new List<Task>(); //Empty list of data Tasks
            foreach (BusinessLayer.Task item in tasks)
            {
                tasksSer.Add(new Task(item.getColumnId(), item.getCreationTime(), item.getDueDate(), item.getTitle(), item.getDescription(), item.getEmailAssignee())); //Adding to the list of dataTask the converted Task
            }
            IReadOnlyCollection<Task> toReturn = tasksSer.AsReadOnly();
            return toReturn;
        }


        public Response LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            try
            {
                bc.getBoard(email).setLimit(limit, columnOrdinal, email);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            log.Debug("limit of column " + columnOrdinal + " was set succefully");
            return new Response();
        }
        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
        {
            try
            {
                bc.getBoard(email).addTask(dueDate, title, description,email);

            }
            catch (Exception e)
            {
                return new Response<Task>(e.Message);
            }
            int id = bc.getBoard(email).getTaskCounterKey() - 1; //-1 Because counterKey has already been updated to the value of the next task
            Task toReturn = new Task(id, bc.getBoard(email).getColumn(0).findTask(id).getCreationTime(), dueDate, title, description, email);
            log.Info("Task added succesfully");
            return new Response<Task>(toReturn);
        }

        public Response ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            try
            {
                bc.changeColumnName(email, columnOrdinal, newName);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response<Column> GetColumn(string email, string columnName)
        {
            try
            {
                bc.getBoard(email).getColumn(columnName);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
            int limit = bc.getBoard(email).getColumn(columnName).getLimit();
            int ordinal = bc.getBoard(email).getColumn(columnName).getKey();
            IReadOnlyCollection<IntroSE.Kanban.Backend.BusinessLayer.Task> collection = (bc.getBoard(email.ToLower()).getColumn(columnName).getTasks());
            List<Task> newList = new List<Task>();
            foreach (IntroSE.Kanban.Backend.BusinessLayer.Task busTask in collection)
            {
                Task toAdd = new Task(busTask.getKey(), busTask.getCreationTime(), busTask.getDueDate(), busTask.getTitle(), busTask.getDescription(), busTask.getEmailAssignee());
                newList.Add(toAdd);
            }
            IReadOnlyCollection<Task> newCollection = newList.AsReadOnly();
            Column toReturn = new Column(newCollection, columnName, limit, ordinal);
            return new Response<Column>(toReturn);
        }
        public Response<Column> GetColumn(string email, int columnOrdinal)
        {
            try
            {
                bc.getBoard(email).getColumn(columnOrdinal);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }

            IReadOnlyCollection<BusinessLayer.Task> collection = (bc.getBoard(email).getColumn(columnOrdinal).getTasks());
            List<Task> newList = new List<Task>();
            foreach (IntroSE.Kanban.Backend.BusinessLayer.Task busTask in collection)
            {
                Task toAdd = new Task(busTask.getKey(), busTask.getCreationTime(), busTask.getDueDate(), busTask.getTitle(), busTask.getDescription(), busTask.getEmailAssignee());  //Converting each task from business to service
                newList.Add(toAdd);
            }
            IReadOnlyCollection<Task> newCollection = newList.AsReadOnly();
            int limit = bc.getBoard(email).getColumn(columnOrdinal).getLimit();

            string name = bc.getBoard(email).getColumn(columnOrdinal).getName();
            Column toReturn = new Column(newCollection, name, limit, columnOrdinal);
            return new Response<Column>(toReturn);
        }
        public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                bc.AssignTask(email, columnOrdinal, taskId, emailAssignee);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            return new Response();
        }

    }
}