using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class BoardController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BoardController"); //Logger
        private Dictionary<string,Board> boards;  //Contains boards of all the registered users by email

        public BoardController(Dictionary<string, User> users) //Constructor must get registereed users list as a parameter
        {
            boards = new Dictionary<string, Board>();
            foreach(var item in users) //For each user update boards dictionary
            {
                boards.Add(item.Key, item.Value.getBoard());
            }
        }

        //Assumes email is an existing key in the dictionary
        public void register(string email,Board board)  //Updates both dictionary when a new user was registered
        {
            //Already checked in UserController that the email doesnt already exist
            this.boards.Add(email, board);

        }
        public Board getBoard(string email)
        {
            email = email.ToLower();
            foreach(var board in boards)
            {
                foreach (var assignedEmail in board.Value.getAssignedUsers())
                {
                    if (assignedEmail.Equals(email))
                        return board.Value;
                }
            
            }
            log.Warn("No such user");
            throw new Exception("No such user");

        }
        public void loadData(Dictionary<string, User> users)
        {
            foreach (var item in users) //For each user update boards dictionary
            {
                boards.Add(item.Key, item.Value.getBoard());
            }
        }
        public void DeleteData()
        {
            this.boards = new Dictionary<string, Board>();
        }
        public void changeColumnName(string email, int columnOrdinal, string newName)
        {
            email = email.ToLower();
            if (!boards.ContainsKey(email))
            {
                log.Warn("No such user");
                throw new Exception("No such user");
            }
            this.boards[email].changeColumnName(columnOrdinal, newName, email);
        }
        public int getNextBoardId()
        {
            if (boards == null)
            {
                return 0;
            }
            return boards.Count ;
        }
        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            email = email.ToLower();
            this.getBoard(email).AssignTask(columnOrdinal, taskId, emailAssignee, email);
        }
    }
}
