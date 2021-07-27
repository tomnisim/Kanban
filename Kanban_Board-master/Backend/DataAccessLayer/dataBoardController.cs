
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class dataBoardController:dalController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("dataBoardController");

        private const string MessageTableName = "Kanban_DB.db";
        private const string tableName = "Board";
        public dataBoardController() : base(MessageTableName,tableName)
        {

        }

        public List<dataBoard> SelectAllBoards()
        {
            List<dataBoard> result = Select().Cast<dataBoard>().ToList();

            return result;
        }
        public bool Update(int boardId, string attributeName, int attributeValue) // updating the int values columns - board id,task counter key and column counter key
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{attributeName}]=@{attributeName} where boardId={boardId}"
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
        // didnt update the email creator field - a field that can not be changed

        public bool Insert(dataBoard board) // insert new board to database according to the dataBoard fields
        {
     
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName} ({dataBoard.boardEmailCreator},{dataBoard.boardTaskCounterKey},{dataBoard.boardColumnCounterKey},{dataBoard.boardBoardId}) " +
                        $"VALUES (@emailCreator,@taskCounterKey,@columnCounterKey,@boardId);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailCreator", board.emailCreator);
                    SQLiteParameter taskCounterKeyParam = new SQLiteParameter(@"taskCounterKey", board.taskCounterKey);
                    SQLiteParameter columnCounterKeyParam = new SQLiteParameter(@"columnCounterKey", board.columnCounterKey);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", board.boardId);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(taskCounterKeyParam);
                    command.Parameters.Add(columnCounterKeyParam);
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

        protected override dal ConvertReaderToObject(SQLiteDataReader reader) // reading from DB and create dataBoard Object
        {
            dataBoard result = new dataBoard(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3)));
            return result;
        }
    }
}
