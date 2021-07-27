
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class dataColumnController : dalController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("dataColumnController");

        private const string MessageTableName = "Kanban_DB.db";
        private const string tableName="Column";

        public dataColumnController() : base(MessageTableName, tableName)
        {

        }
        public bool Update(int boardId, int columnOrdinal, string attributeName, int attributeValue)// updating the int values columns - board id,column ordinal & limit
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                
                SQLiteCommand command = new SQLiteCommand()
                {
                    
                    Connection = connection,
                    CommandText = $"UPDATE {tableName} SET [{attributeName}]=@{attributeName} WHERE columnOrdinal={columnOrdinal} AND  boardId ={boardId} "

                                    

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
        public bool Update(int boardId, int columnOrdinal, string attributeName, string attributeValue)// updating the string value column - column name
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {

                SQLiteCommand command = new SQLiteCommand()
                {

                    Connection = connection,
                    CommandText = $"UPDATE {tableName} SET [{attributeName}]=@{attributeName} WHERE columnOrdinal={columnOrdinal} AND  boardId ={boardId} "



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

        public List<dataColumn> SelectAllColumns() 
        {
            List<dataColumn> result = Select().Cast<dataColumn>().ToList();

            return result;
        }

        public bool Insert(dataColumn column)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName} ({dataColumn.columnBoardId},{dataColumn.columnOrdinal},{dataColumn.columnLimit},{dataColumn.columnName}) " +
                        $"VALUES (@columnBoardId,@columnOrdinal,@columnLimit,@columnName);";


                    SQLiteParameter boardIdParam = new SQLiteParameter(@"columnBoardId", column.boardId);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"@columnOrdinal", column.ordinal);
                    SQLiteParameter nameParam = new SQLiteParameter(@"columnName", column.name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"columnLimit", column.limit);

                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(limitParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();

                }
                catch(Exception e)
                {
                    log.Warn(e.ToString());
                    log.Warn("Insert failed");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }// insert new column to database according to the dataColumn fields

        protected override dal ConvertReaderToObject(SQLiteDataReader reader) // reading from DB and create DataColumn Object
        {
            dataColumn result = new dataColumn(Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), reader.GetString(3));
            return result;

        }
        public bool Delete(int boardId, int columnId)
        {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where boardId={boardId} AND columnOrdinal={columnId}"
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
        }// select the row by board id and column ordinal(double primary key)
    }
}
