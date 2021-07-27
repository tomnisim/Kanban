
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class dataTaskController : dalController
    {
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("dataTaskController");

        private const string MessageTableName = "Kanban_DB.db";
        private const string tableName = "Task";

        public dataTaskController() : base(MessageTableName,tableName)
        {

        }
        public List<dataTask> SelectAllTasks()
        {
            List<dataTask> result = Select().Cast<dataTask>().ToList();

            return result;
        }
        public bool Delete(int boardId, int key)  // select the row by board id and task id(primary key)
        { 
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {tableName}  WHERE boardId={boardId} AND taskId={key}"
                };
                try
                {
                    
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }
        public bool Update(int boardId, int key, string attributeName, int attributeValue) // updating the int values columns - board id,column ordinal & task id
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{attributeName}]=@{attributeName} WHERE boardId={boardId} AND taskId={key}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }
        public bool Update(int boardId, int key, string attributeName, string attributeValue) // updating the string values columns - title,description & emailAssignee
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{attributeName}]=@{attributeName} where boardId={boardId} AND taskId={key}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }
        public bool Update(int boardId, int key, string attributeName, DateTime attributeValue)// updating the Date Time values columns - creationTime,DueDate 
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{attributeName}]=@{attributeName} where boardId={boardId} AND taskId={key}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }
        virtual public bool Insert(dataTask task)// insert new task to database according to the dataUser fields
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                { 
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName} ({dataTask.taskBoardId}, {dataTask.taskColumn}, {dataTask.taskId},{dataTask.taskTitle},{dataTask.taskDescription}, {dataTask.taskDueDate}, {dataTask.taskCreationTime},{dataTask.taskEmailAssignee}) " +
                        $"VALUES (@boardId,@column,@taskId,@title,@description,@dueDate,@creationTime,@emailAssignee);";


                    SQLiteParameter titleParam = new SQLiteParameter(@"title", task.title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"description", task.description);
                    SQLiteParameter columnParam = new SQLiteParameter(@"column", task.column);
                    SQLiteParameter taskIdParam = new SQLiteParameter(@"taskId", task.id);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDate", task.dueDate);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTime", task.creationTime);
                    SQLiteParameter emailAssigneeParam = new SQLiteParameter(@"emailAssignee", task.emailAssignee);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", task.boardId);

                    command.Parameters.Add(emailAssigneeParam);
                    command.Parameters.Add(columnParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(taskIdParam);
                    command.Parameters.Add(boardIdParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Warn("Insert failed");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        protected override dal ConvertReaderToObject(SQLiteDataReader reader) // reading the data from database and create a new DataTask Object
        {
            dataTask result = new dataTask(Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)),reader.GetString(3), reader.GetString(4), (DateTime)reader.GetValue(5), (DateTime)reader.GetValue(6), reader.GetString(7));
            return result;

        }
    }
}
