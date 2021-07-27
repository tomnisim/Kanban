using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class dataUserController : dalController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("dataUserController");

        private const string MessageTableName = "Kanban_DB.db";
        private const string tableName="User";
        public dataUserController() : base(MessageTableName,tableName)
        {

        }
        public bool Update(string email, string attributeName, long attributeValue) // select the row by email(primary key)
        {
            email = "'" + email + "'";
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{attributeName}]=@{attributeName} where email={email}"
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

        public List<dataUser> SelectAllUsers()
        {
            List<dataUser> result = Select().Cast<dataUser>().ToList();

            return result;
        }



        public bool Insert(dataUser user) // insert new user to database according to the dataUser fields
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName} ( {dataUser.userEmail},{ dataUser.userNickName},{ dataUser.userPassword}, { dataUser.userBoardId}) " +
                        $"VALUES (@email,@nickName,@password,@boardId);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"email", user.email);
                    SQLiteParameter nickNameParam = new SQLiteParameter(@"nickName", user.nickname);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"password", user.password);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", user.boardId);





                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(nickNameParam);
                    command.Parameters.Add(passwordParam);
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

        protected override dal ConvertReaderToObject(SQLiteDataReader reader)// reading the data from database and create a new dataUser Object
        {
            dataUser result = new dataUser(reader.GetString(0), reader.GetString(2), reader.GetString(1), Convert.ToInt32(reader.GetValue(3)));
            return result;
        }
    }
}

