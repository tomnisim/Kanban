using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    public class UserController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("UserController"); //Logger
        private Dictionary<string, User> users { get; set; } //Contains all registered users by email key
        private BoardController bc;

        public UserController() //Empty contructor - intializes user's dictionary and BoardController with the new user's dictionary
        {
            this.users = new Dictionary<string, User>();
            this.bc = new BoardController(users);
        }
        public void login(string email, string password)
        {
            if (users.ContainsKey(email)) //User was found in the dictionary
            {
                users[email].login(password);
            }
            else //No such user in the dictionary
            {
                log.Warn("User does not exist");
                throw new Exception("User does not exist");
            }
        }
        public void Register(string email, string nickname, string password)
        {
            email = email.ToLower();
            if (!IsValidEmail(email))  //Email check
            {
                log.Warn("invalid email");
                throw new Exception("invalid email");
            }
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length == 0 || nickname.IndexOf(' ') != -1)//nickname check
            {
                log.Warn("Invalid nickname");
                throw new Exception("invaild nickname");
            }
            if (users.ContainsKey(email.ToLower()))//if email already exists
            {
                log.Warn("Email already exsits");
                throw new Exception("Email already exists");
            }

            if (isValidPassword(password))//throws exceptions
            {
                User userToAdd = new User(email.ToLower(), nickname, password, bc.getNextBoardId());
                users.Add(email.ToLower(), userToAdd); //Adds the new registered user with the key as lower case Email
                bc.register(email, userToAdd.getBoard()); //Update the new user in BoardController as well
            }
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public void Register(string email, string nickname, string password, string emailHost)
        {
            email = email.ToLower();
            emailHost = emailHost.ToLower();
            if (!IsValidEmail(email))  //Email check
            {
                log.Warn("invalid email");
                throw new Exception("invalid email");
            }
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length == 0 || nickname.IndexOf(' ') != -1)//nickname check
            {
                log.Warn("Invalid nickname");
                throw new Exception("invaild nickname");
            }
            if (users.ContainsKey(email))//if email already exists
            {
                log.Warn("Email already exsits");
                throw new Exception("Email already exists");
            }
            if (!users.ContainsKey(emailHost))//if email already exists
            {
                log.Warn("EmailHost does not exsit");
                throw new Exception("EmailHost does not exsit");
            }
            if (isValidPassword(password))//throws exceptions
            {
                Board tempBoard = users[emailHost].getBoard();
                User userToAdd = new User(email, nickname, password, tempBoard);
                users.Add(email, userToAdd); //Adds the new registered user with the key as lower case Email
            }
        }
        public User getUser(string email)
        {

            if (!IsValidEmail(email))
            {
                log.Warn("invalid user email");
                throw new Exception("invalid user email");
            }
            email = email.ToLower();
            if (users.ContainsKey(email))//User exist check
                return users[email];
            log.Warn("User does not exist");
            throw new Exception("User does not exist");
        }
        public void logout(string email)
        {
            email = email.ToLower();
            if (users.ContainsKey(email))//If user exists
            {
                if (!users[email].isLogged())  //If user is already logged out
                {
                    log.Warn("User is not logged in");
                    throw new Exception("User is not logged in");
                }
                users[email].logout(); //Updating this dictionary that the user is logged out
            }
            else  //User was never registered
            {
                log.Warn("User does not exist");
                throw new Exception("User does not exist");
            }

        }
        private bool isValidPassword(string password)
        {
            if (password == null) //if password is null
            {
                log.Warn("Password cannot be null");
                throw new Exception("Password cannot be null");
            }

            bool existUpper = false;
            bool existSmall = false;
            bool existNumber = false;
            if (password.Length < 5 | password.Length > 25) //Length check
            {
                log.Warn("Invalid password length");
                throw new Exception("invalid password length");
            }
            for (int i = 0; i < password.Length; i++) //Contains capital, small, and a digit
            {
                if ((password[i] <= 'Z') & (password[i]) >= 'A')
                    existUpper = true;
                if ((password[i] <= 'z') & (password[i] >= 'a'))
                    existSmall = true;
                if ((password[i] <= '9') & (password[i] >= '0'))
                    existNumber = true;
            }
            if (!existNumber) //Missing digit
            {
                log.Warn("Invalid password - it must contains a digit");
                throw new Exception("Invalid password - it must contains a digit");
            }
            if (!existSmall) //Missing small letter
            {
                log.Warn("Invalid password - it must contains a small character");
                throw new Exception("Invalid password - it must contains a small character");
            }
            if (!existUpper) //Missing capital letter
            {
                log.Warn("Invalid password - it must contains a uppercase letter");
                throw new Exception("Invalid password - it must contains a uppercase letter");
            }
            return true;
        }
        public Dictionary<string, User> getUsers()
        {
            return this.users;
        }
        public void loadata()
        {
            if (users.Count == 0) //Cant load data before deleting it
            {

                DataAccessLayer.dataBoardController boardController = new DataAccessLayer.dataBoardController();
                DataAccessLayer.dataTaskController taskController = new DataAccessLayer.dataTaskController();
                DataAccessLayer.dataColumnController columnController = new DataAccessLayer.dataColumnController();
                DataAccessLayer.dataUserController UserController = new DataAccessLayer.dataUserController();
                //Loading from database to dal objects
                List<DataAccessLayer.dataBoard> allBoards = boardController.SelectAllBoards();
                List<DataAccessLayer.dataTask> allTasks = taskController.SelectAllTasks();
                List<DataAccessLayer.dataColumn> allColumns = columnController.SelectAllColumns();
                List<DataAccessLayer.dataUser> allUsers = UserController.SelectAllUsers();
                List<Board> boards = new List<Board>();

                foreach (var dataUser in allUsers)
                {
                    Board busBoard = null;
                    foreach (var dataBoard in allBoards)
                    {
                        if (dataUser.email.Equals(dataBoard.emailCreator)) //Only add the board created by the user
                        {
                            Dictionary<int, Column> busColumns = new Dictionary<int, Column>();
                            foreach (var dataColumn in allColumns)
                            {
                                if (dataColumn.boardId == dataBoard.boardId)
                                {
                                    List<Task> busTasks = new List<Task>();
                                    foreach (var dataTask in allTasks)
                                    {
                                        if (dataTask.column == dataColumn.ordinal & dataColumn.boardId == dataTask.boardId) //Only add the tasks that match the correct column
                                        {
                                            busTasks.Add(new Task(dataTask));//adding to task list business task which is the copy of the data task
                                        }
                                    }
                                    Column busColumn = new Column(dataColumn, busTasks);
                                    busColumns.Add(busColumn.getKey(), busColumn);
                                }
                            }
                            busBoard = new Board(dataBoard, busColumns);
                            boards.Add(busBoard);
                        }

                    }
                    User busUser = new User(dataUser, busBoard);
                    users.Add(busUser.getEmail(), busUser);
                }

                foreach (var user in users)
                {
                    if (user.Value.getBoard() == null) //A user that is not a board creator
                    {
                        foreach (var board in boards)
                        {
                            if (board.getBoardId() == user.Value.getBoardId())
                            {
                                user.Value.setBoard(board); //board is not null
                                board.addAssignedUser(user.Value.getEmail());
                            }
                        }
                    }

                }
                bc.loadData(users); //Loading all Boards to BoardController

            }

        }
        public void DeleteData()
        {

            //Database tables deletion---------------------------------------------------------------------------------
            dataUserController DUC = new dataUserController();
            dataBoardController DBC = new dataBoardController();
            dataColumnController DCC = new dataColumnController();
            dataTaskController DTC = new dataTaskController();
            DUC.DeleteAll(); //Deleting all Users from database
            log.Info("deleted all users");
            DBC.DeleteAll(); //Deleting all Boards from database
            log.Info("deleted all boards");
            DCC.DeleteAll();//Deleting all Columns from database
            log.Info("deleted all columns");
            DTC.DeleteAll();//Deleting all Tasks from database
            log.Info("deleted all tasks");
            //Variables data deletion----------------------------------------------------------------------------------
            foreach (var item in users) //Calling each board to deleteData (which deletes all columns and tasks)
            {
                item.Value.getBoard().DeleteData();
            }

            this.users = new Dictionary<string, User>(); //Deleting all existing users by creating new empty dictionary
            bc.DeleteData();
        }
        public BoardController getBoardController() { return this.bc; }

    }
}