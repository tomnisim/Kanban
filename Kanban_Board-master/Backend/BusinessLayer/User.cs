using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class User
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusniessLayer.User");
        private string email { get; set; }
        private string nickname { get; set; }
        private string password { get; set; }
        private Board board { get; set; }
        private int boardId;


        public User() { }//empty constructor
        public User(string email, string nickname, string password,int boardId) //Constructing a user that is a board creator
        {
            this.email = email;
            this.nickname = nickname;
            this.password = password;
            this.board = new Board(email,boardId);
            this.boardId = boardId;

            //Inserting to database
            DataAccessLayer.dataUserController DUC = new DataAccessLayer.dataUserController();
            DUC.Insert(this.toDalObject());//insert new user to data base table
        }
        public User(string email, string nickname, string password, Board b) //Constructing a user that is an assigned user
        {
            this.email = email;
            this.nickname = nickname;
            this.password = password;
            this.board = b;
            this.boardId = b.getBoardId();
            b.addAssignedUser(email);

            //Inserting to database
            DataAccessLayer.dataUserController DUC = new DataAccessLayer.dataUserController();
            DUC.Insert(this.toDalObject());//insert new user to data base table
        }
        public User(DataAccessLayer.dataUser user, Board board)//copy constructor data to bussines
        {
            this.email = user.email;
            this.nickname = user.nickname;
            this.password = user.password;
            this.board = board;
            this.boardId = user.boardId;
        }

        public Board getBoard()
        {
                return board;
        }
        public string getNickname() { return nickname; }
        public string getEmail() { return this.email; }
        public void login(string password) // have to check if the password is correct - but the field is in board
        {
            if (this.password.Equals(password))
            {
                board.login();
                log.Debug("Password is correct");
            }

            else
            {
                log.Warn("Password is incorrect");
                throw new Exception("Password is incorrect");
            }
      
        }
        public void logout()
        {
            this.board.logout();
        }
        public bool isLogged()
        {
            return this.board.getLogged();
        }
        public DataAccessLayer.dataUser toDalObject() //Converts BusinessUser to 
        {
            DataAccessLayer.dataUser toReturn = new DataAccessLayer.dataUser(email, password, nickname,this.boardId);
            log.Debug("Converted dataUser to DAL Object succesfully");
            return toReturn;
        }
        public int getBoardId()
        {
            return this.boardId;
        }
        public void setBoard(Board board) //Only called after laoding data and the user has a null board
        {
            this.board = board;
        }

    }
}
