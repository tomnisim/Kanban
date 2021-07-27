using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Board
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusniessLayer.Board");
        private Dictionary<int,Column> columns;
        private string emailCreator { get; set; }
        private int boardId;
        private List<string> assignedUsers;
        private int columnCounterKey; //Holds the value of the next columnOrdinal to be added
        private bool isLoggedIn { get; set; } //Will remain false until user has logged in
        private int taskCounterKey { get; set; }//Holds the value of the next taskId to be added, does not represent the number of tasks in the board
        private const int MinNumberOfColumns = 2;
        public Board(Dictionary<int, Column> ColumnsToTest, bool isLogged, string emailCreator,int columnCounterKey) //Constructor for tests
        {
            this.columns = ColumnsToTest;
            this.isLoggedIn = isLogged;
            this.emailCreator = emailCreator;
            this.columnCounterKey = columnCounterKey;
        }
        public Board() {
            columns = new Dictionary<int, Column>();
        }//empty constructor
        public Board(string emailCreator,int boardId) //Default constructor
        {
            this.boardId = boardId;
            this.assignedUsers = new List<string>();
            this.assignedUsers.Add(emailCreator);
            columnCounterKey = 0;
            columns = new Dictionary<int, Column>();
            this.isLoggedIn = false; //Board is not logged in until user logged in
            this.emailCreator = emailCreator;
            Column columnBackLog = new Column("backlog", columnCounterKey,boardId); //Default first column
            columnCounterKey++;
            Column columnInProgress = new Column( "in progress", columnCounterKey, boardId);//Default second column
            columnCounterKey++;
            Column columnDone = new Column("done", columnCounterKey, boardId);//Default third column
            columnCounterKey++;
            columns.Add(0,columnBackLog);
            columns.Add(1, columnInProgress);
            columns.Add(2, columnDone);
            taskCounterKey = 0; //Initialize taskCounterKey with 0 because no tasks were added yet
            
   
            //Inserting to database------------------------------------------------------------------------
            DataAccessLayer.dataBoardController DBC = new DataAccessLayer.dataBoardController();
            DBC.Insert(this.toDalObject());

        }
        public Board (DataAccessLayer.dataBoard dBoard, Dictionary<int, Column> columns) //Copy constructor from data to business
        {

            //update assigned user from data base
            this.boardId = dBoard.boardId;
            this.columns = columns;
            this.taskCounterKey = dBoard.taskCounterKey;
            this.emailCreator = dBoard.emailCreator;
            this.isLoggedIn = false; //Default initialization value
            this.columnCounterKey = dBoard.columnCounterKey;
            this.assignedUsers = new List<string>();
            this.assignedUsers.Add(emailCreator);
        }



        public void changeColumnName(int columnOrdinal, string newName, string email)
        {
            if (!isLoggedIn) //User is not logged in
            {
                log.Warn("user is not logged in");
                throw new Exception("user is not logged in");
            }
            if (!this.emailCreator.Equals(email))//only board creator can change column name
            {
                log.Warn("only board creator can change column name");
                throw new Exception("only board creator can change column name");
            }
            foreach (var item in columns) //Checking unique column name
            {
                if (item.Value.getName().Equals(newName))
                {
                    log.Warn("Column name already exists");
                    throw new Exception("Column name already exists");
                }
            }
            this.getColumn(columnOrdinal).changeName(newName);
        }
        public int getTaskCounterKey() { return taskCounterKey; }
        public void addColumn(int columnOrdinal, string Name, string email)
        {
            email = email.ToLower();
            if (!isLoggedIn) //User is not logged in
            {
                log.Warn("user is not logged in");
                throw new Exception("user is not logged in");
            }
            if (!this.emailCreator.Equals(email))//only board creator can add column
            {
                log.Warn("only board creator can add column");
                throw new Exception("only board creator can add column");
            }
            foreach (var item in columns) //Checking unique column name
            {
                if(item.Value.getName().Equals(Name))
                {
                    log.Warn("Column name already exists");
                    throw new Exception("Column name already exists");
                }
            }
            if (columnOrdinal == columnCounterKey) //Adding a column as the last column
            {
                Column toAdd = new Column(Name, columnCounterKey,boardId);
                columns.Add(columnOrdinal, toAdd);
                columnCounterKey++;//Increment because a column was added
                //Updating the database-----------------------------------------------------------------------------------------------
                DataAccessLayer.dataBoardController DBC = new DataAccessLayer.dataBoardController();
                DBC.Update(boardId, "columnCounterKey", columnCounterKey); 
            }
            else if (columnOrdinal < columnCounterKey & columnOrdinal >= 0) //Adding a column to the "middle" of the columns
            {
                for (int i = columns.Count - 1; i >= columnOrdinal; i--) //Moving all columns (that are bigger than coulmnOrdinal) to the right to make room for the new column
                {
                    Column temp = columns[i];
                    columns.Remove(i);                 
                    temp.setKey(i + 1);
                    columns.Add(i + 1, temp);
                }
                Column toAdd = new Column(Name, columnOrdinal,boardId); //Building the new column
                columns.Add(columnOrdinal,toAdd); //Adding the new column in the columnOrdinal place
                columnCounterKey++; //Increment because a column was added
                //Updating the database-----------------------------------------------------------------------------------------------
                DataAccessLayer.dataBoardController DBC = new DataAccessLayer.dataBoardController();
                DBC.Update(boardId, "columnCounterKey", columnCounterKey);
            } 
            else //Column ordinal is invalid
            {
                log.Warn("invalid column ordinal");
                throw new Exception("invalid column ordinal");
            }
        }
        public void DeleteTask(int columnOrdinal, int taskId,string email)
        {
            this.getColumn(columnOrdinal).DeleteTask(taskId,email);
        }
        public void AssignTask(int columnOrdinal, int taskId, string emailAssignee, string email)
        {
            if (!assignedUsers.Contains(emailAssignee))
            {
                log.Warn("task can not be assign to user that is not a member in the board");
                throw new Exception("task can not be assign to user that is not a member in the board");
            }
            getColumn(columnOrdinal).AssignTask(taskId, emailAssignee, email);
        }
        public void removeColumn(int columnOrdinal, string email)
        {
            email = email.ToLower();
            if (!isLoggedIn) //User is not logged in
            {
                log.Warn("user is not logged in");
                throw new Exception("user is not logged in");
            }
            if (!this.emailCreator.Equals(email))
            {
                log.Warn("only board creator can remove column");
                throw new Exception("only board creator can remove column");
            }
            if (columns.Count<=MinNumberOfColumns) //Board has 2 columns left - Cant remove column
            {
                log.Warn("Cant remove this column - Board must have at least 2 columns");
                throw new Exception("Cant remove this column - Board must have at least 2 columns");
            }
            if (columnOrdinal > 0 & columnOrdinal < columnCounterKey)//Removing column from the "middle"
            {
                Column toRemove = columns[columnOrdinal];
                Column backup = columns[columnOrdinal - 1];
                
                if (backup.getLimit()!=-1 && toRemove.getTasksForColumns().Count + backup.getTasksForColumns().Count > backup.getLimit() ) //The column to the left will exceed the limit after removing current column
                {
                    log.Warn("the limit is smaller then the tasks number");
                    throw new Exception("the limit is smaller then the tasks number");
                }
                List<int> listOfKeysToRemove = new List<int>(); //A list to hold all the keys of tasks we want to remove
                foreach(var item in toRemove.getTasksForColumns()) //Adding all tasks keys to the listOfKeysToRemove
                {
                    listOfKeysToRemove.Add(item.getKey());
                }
                foreach (var item in listOfKeysToRemove) //Moving all tasks from the current column to the column to the left
                {
                    Task removedTask = toRemove.removeTask(item);
                    removedTask.setColumnId(backup.getKey());
                    backup.addTask(removedTask);
                }
                columns.Remove(columnOrdinal);
                //deleting the column row from database-----------------------------------------------------------------------
                DataAccessLayer.dataColumnController DCC = new DataAccessLayer.dataColumnController();
                DCC.Delete(boardId, columnOrdinal);
                //------------------------------------------------------------------------------------------------------------
                for (int i = columnOrdinal + 1; i < columnCounterKey; i++)//Moving column to the left to cover the gap
                {
                    Column temp = columns[i];
                    foreach (var item in temp.getTasksForColumns())
                    {
                        item.setColumnId(i - 1);
                    }
                    columns.Remove(i);
                    temp.setKey(i - 1);
                    columns.Add(i - 1, temp);
                }
                columnCounterKey--; //Decrease because a column was removed
                //Updating the database---------------------------------------------------------------------------------------
                DataAccessLayer.dataBoardController DBC = new DataAccessLayer.dataBoardController();
                DBC.Update(this.boardId, "columnCounterKey", columnCounterKey);

            }
            
        
            else if (columnOrdinal == 0 )//Removing the leftmost column
            {
                Column toRemove = columns[0];
                Column backup = columns[1];
                if (backup.getLimit() != -1 && toRemove.getTasksForColumns().Count + backup.getTasksForColumns().Count > backup.getLimit())//The column to the right will exceed the limit after removing current column
                {
                    log.Warn("the limit is smaller then the tasks number");
                    throw new Exception("the limit is smaller then the tasks number");
                }
                foreach (var item in toRemove.getTasksForColumns())
                {
                    item.setColumnId(columnOrdinal); //Always set to 0
                    backup.addTask(item);
                }
                columns.Remove(columnOrdinal);
                //deleting the column row from database-----------------------------------------------------------------------
                DataAccessLayer.dataColumnController DCC = new DataAccessLayer.dataColumnController();
                DCC.Delete(boardId, columnOrdinal);
                //------------------------------------------------------------------------------------------------------------
                for (int i = columnOrdinal + 1; i < columnCounterKey; i++) //i always starts from 1
                {

                    Column temp = columns[i];
                    foreach (var item in temp.getTasksForColumns())
                    {
                        item.setColumnId(i - 1);
                    }
                    columns.Remove(i);
                    temp.setKey(i - 1);
                    columns.Add(i - 1, temp);
                }
                columnCounterKey--;//Decrease because a column was removed
                //Updating the database---------------------------------------------------------------------------------------
                DataAccessLayer.dataBoardController DBC = new DataAccessLayer.dataBoardController();
                DBC.Update(this.boardId, "columnCounterKey", columnCounterKey);
            }
           
            else //Invalid column ordinal
            {
                log.Warn("invalid column ordinal");
                throw new Exception("invalid column ordinal");
            } 
        }
        public int  getBoardId()
        {
            return this.boardId;
        }
        public void addTask(DateTime dueDate, string title, string description,string email)
        {
            
            if (!isLoggedIn) //User is not logged in
            {
                log.Warn("user is not logged in");
                throw new Exception("user is not logged in");
            }
            Task temp;
            if (description != null) //Description isnt null
            {
                temp = new Task(dueDate, title, description, this.taskCounterKey, email, 0, boardId);  //Using the construcor with description

            }
            else //Description is null
            {
                temp = new Task(dueDate, title, this.taskCounterKey, email, 0, boardId); //Using the construcor without description

            }
            if (!this.columns[0].addTask(temp)) //Task limit reached
            {
                log.Warn("Tasks limit reached");
                throw new Exception("Tasks limit reached");  //Exception is thrown in this class in order to tell if counterKey should be incremented or not
            }
            else //If a task was added
            {
                taskCounterKey++; //Increment because a task was added
                //Updating the database-------------------------------------------------------------------------------
                DataAccessLayer.dataBoardController DBC = new DataAccessLayer.dataBoardController();
                DBC.Update(boardId, "taskCounterKey",taskCounterKey);
            }
        }
        public void advanceTask(int key, int columnOrdinal,string email) {
            email = email.ToLower();
            if (!isLoggedIn) //User is not logged in
            {
                log.Warn("user is not logged in");
                throw new Exception("user is not logged in");
            }
            if (columnOrdinal==columns.Count-1)//Column ordinal is the last column
            {
                log.Warn("Cant advance task from the last column");
                throw new Exception("Cant advance task from the last column");
            }
            else if (columnOrdinal>=0 & columnOrdinal < columns.Count - 1) //ColumnOridnal check
            {
                if (columns[columnOrdinal+1].isFull()) //Next column is full
                {
                    log.Warn("next column is full");
                    throw new Exception("next column is full");
                }
                if (columnOrdinal < 0) //Invalid column id
                {
                    log.Warn("invalid column id");
                    throw new Exception("invalid column id");
                }
                Task removed = columns[columnOrdinal].removeTask(key); //removed contains the removed task(null if the task doesnt exist)

                if (removed != null) //If a task was deleted
                {
                    if (removed.isAssignedUser(email)) // if failed the log message is 'cant edit task'
                    {
                        removed.setColumnId(columnOrdinal + 1); //Updating task's column id to the next column
                        columns[columnOrdinal + 1].addTaskForAdvance(removed); //Adding the task to the next column
                    }
                }
                else//Task doesnt exist in the column
                {
                    log.Warn("Task doesnt exist in the column");
                    throw new Exception("Task doesnt exist in the column");
                }

            }
            else//Invalid Column Ordinal
            {
                log.Warn("Invalid Column Ordinal");
                throw new Exception("Invalid Column Ordinal");
            }
        }
        public void setBoardId(int boardid)
        {
            this.boardId = boardid;
        }

        public void setIsLoggedIn(bool value)
        {
            this.isLoggedIn = value;
        }

        public void setLimit(int limit,int columnId, string email)
        {
            email = email.ToLower();
            if (!isLoggedIn) //User is not logged in
            {
                log.Warn("user is not logged in");
                throw new Exception("user is not logged in");
            }
            if (!this.emailCreator.Equals(email))
            {
                log.Warn("The colomn id doesnt match");
                throw new Exception("The colomn id doesnt match");
            }
            if (columnId < 0 | columnId > columns.Count-1) //Colomn id doesnt match
            {
                log.Warn("The colomn id doesnt match");
                throw new Exception("The colomn id doesnt match");
            }
            if (columnId == columns.Count-1)
            {
                log.Warn("can not set limit to last column");
                throw new Exception("can not set limit to last column");
            }
            columns[columnId].setLimit(limit);
        }
        private bool canEdit(int columnOrdinal) // check if the user is logged in and if the column ordinal is valid
        {
            if (!isLoggedIn) //User is not logged in
            {
                log.Warn("user is not logged in");
                throw new Exception("user is not logged in");
            }
            if (columnOrdinal < 0 | columnOrdinal > columns.Count - 1) //Invalid column ID
            {
                log.Warn("Invalid Column ID");
                throw new Exception("Invalid Column ID");
            }

            if (columnOrdinal == columns.Count - 1)//If columnOrdinal is the last column
            {
                log.Warn("A Done task cant be editted");
                throw new Exception("A Done task cant be editted");
            }
            return true;
        }
        public void editTaskDueDate(int key, int columnOrdinal, DateTime dueDate, string email)
        {
            email = email.ToLower();
            if (canEdit(columnOrdinal))
            {
                columns[columnOrdinal].editTaskDueDate(key, dueDate, email);
            }
        }
        public void editTaskTitle(int key, int columnOrdinal, string title,string email)
        {
            email = email.ToLower();
            if (canEdit(columnOrdinal))
            {
                columns[columnOrdinal].editTaskTitle(key, title, email);
            }
        }
        public void editTaskDescription(int key, int columnOrdinal, string description, string email)
        {
            email = email.ToLower();
            if (canEdit(columnOrdinal))
            {
                columns[columnOrdinal].editTaskDescription(key, description, email);
            }
        }
        public Column getColumn(string columnName)
        {
            foreach (var item in columns) //Searching the requested column
            {
                if (item.Value.getName().Equals(columnName))
                    return item.Value;
            }
            log.Warn("Column Name isnt exist");
            throw new Exception("Column Name isnt exist");
        }
        public Column getColumn(int columnOrdinal)
        {
            if (columnOrdinal >= 0 & columnOrdinal < columns.Count) //ColumnOrdinal check
                return columns[columnOrdinal];
            else
            {
                log.Warn("Invalid Column Ordinal");
                throw new Exception("Invalid Column Ordinal");
            }
        }
        public IReadOnlyCollection<string> getColumnNames() //Returns a read only collection
        {
            string[] namesArr = new string[columns.Count];
            for (int i = 0;i<columns.Count;i++)
            {
                namesArr[i] = columns[i].getName();
            }
            IReadOnlyCollection<string> names = Array.AsReadOnly<string>(namesArr);
            return names;
        }
        public void setEmail(string emailCreator)
        {
            this.emailCreator = emailCreator;
        }
        public void MoveColumnRight(int columnOrdinal, string email)
        {
            email = email.ToLower();
            if (!isLoggedIn) //User os not logged in
            {
                log.Warn("user is not logged in");
                throw new Exception("user is not logged in");
            }
            if (!this.emailCreator.Equals(email))
            {
                log.Warn("only board creator can change column order");
                throw new Exception("only board creator can change column order");
            }
            if (columnOrdinal >= 0 & columnOrdinal < columnCounterKey-1) //ColumnId check
            {
                Column moveRight = columns[columnOrdinal];//moveRight is the current column
                Column right = columns[columnOrdinal + 1];//right is the column to the right of current

                //Switching tasks---------------------------------------------------------------------------------------------------------------------------
                foreach (var item in moveRight.getTasksForColumns())//Changing each task's coulmnID field to the id of the column to the right
                {
                    item.setColumnId(right.getKey());
                }
                foreach (var item in right.getTasksForColumns()) ////Changing each task's coulmnID field to the id of the column to the left
                {
                    item.setColumnId(moveRight.getKey());
                }
                
                //Removing the columns from the dictionary---------------------------------------------------------------------------------------------------
                columns.Remove(columnOrdinal);
                columns.Remove(columnOrdinal+1);

                int moveRightKey = columnOrdinal;
                int rightKey = columnOrdinal + 1;
                //Updating columns's key fields---------------------------------------------------------------------

                moveRight.setKey(-1);
                right.setKey(moveRightKey);
                moveRight.setKey(rightKey);

                //Adding the columns to the dictionary with opposite column IDs------------------------------------------------------------------------------
                columns.Add(columnOrdinal, right);
                columns.Add(columnOrdinal + 1, moveRight);
               

                //Updating the database----------------------------------------------------------------------------------------------------------------------
                
                //DataAccessLayer.dataColumnController DCC = new DataAccessLayer.dataColumnController();
                //DCC.Update(boardId, moveRightKey, "columnOrdinal", -1); //Setting columnOrdinal of current coulmn to -1 as temporary value to avoid duplicate keys in the database
                //DCC.Update(boardId, rightKey, "columnOrdinal", columnOrdinal);//Setting the right column coulmnID the the new value
                //DCC.Update(boardId, -1, "columnOrdinal", columnOrdinal+1);//Setting the current column coulmnID the the new value (from the temporary value)
                
            }
            else
            {
                log.Warn("invalid column ordinal");
                throw new Exception("invalid column ordinal");
            }
        }
        public void MoveColumnLeft(int columnOrdinal, string email)
        {
            email = email.ToLower();
            if (!isLoggedIn) //User is not logged in
            {
                log.Warn("user is not logged in");
                throw new Exception("user is not logged in");
            }
            if (!this.emailCreator.Equals(email))
            {
                log.Warn("only board creator can change column order");
                throw new Exception("only board creator can change column order");
            }
            if (columnOrdinal > 0 & columnOrdinal < columnCounterKey) //ColumnID check
            {
                Column moveLeft = columns[columnOrdinal]; //moveLeft is the current column
                Column left = columns[columnOrdinal -1]; //left is the column to the left of current
                //Switching tasks---------------------------------------------------------------------------------------------------------------------------
                foreach (var item in moveLeft.getTasksForColumns())
                {
                    item.setColumnId(left.getKey());
                }
                foreach (var item in left.getTasksForColumns())
                {
                    item.setColumnId(moveLeft.getKey());
                }
                //Removing the columns from the dictionary---------------------------------------------------------------------------------------------------
                columns.Remove(moveLeft.getKey());
                columns.Remove(left.getKey());
                //Updating columns's key fields---------------------------------------------------------------------

                int moveLeftKey = moveLeft.getKey();
                int leftKey = left.getKey();
                moveLeft.setKey(-1);
                left.setKey(moveLeftKey);
                moveLeft.setKey(leftKey);

                //Adding the columns to the dictionary with opposite column IDs------------------------------------------------------------------------------
                columns.Add(columnOrdinal, left);
                columns.Add(columnOrdinal-1 , moveLeft);


                //Updating the database----------------------------------------------------------------------------------------------------------------------

                //DataAccessLayer.dataColumnController DCC = new DataAccessLayer.dataColumnController();
                //DCC.Update(boardId, moveLeftKey, "columnOrdinal", -1);//Setting columnOrdinal of current coulmn to -1 as temporary value to avoid duplicate keys in the database
                //DCC.Update(boardId, leftKey, "columnOrdinal", moveLeftKey);//Setting the left column coulmnID the the new value
                //DCC.Update(boardId, -1, "columnOrdinal", leftKey);//Setting the current column coulmnID the the new value (from the temporary value)
                

            }
            else
            {
                log.Warn("invalid column ordinal");
                throw new Exception("invalid column ordinal");
            }
        }
        public void login() { isLoggedIn = true; }
        public void logout() { isLoggedIn = false; }
        public bool getLogged() { return isLoggedIn; }
        public DataAccessLayer.dataBoard toDalObject() //Converting this to data object
        {
            DataAccessLayer.dataBoard toReturn = new DataAccessLayer.dataBoard(emailCreator, taskCounterKey,columnCounterKey,boardId);
            log.Debug("Converted dataUser to DAL Object succesfully");
            return toReturn;
        }
        public void DeleteData()
        {
            foreach(var item in columns) //Calling deleteData in each column (which deletes all of its tasks)
            {
                item.Value.DeleteData();
            }
            this.columns = new Dictionary<int, Column>(); //Deleting all boards by creating new empty dictionary
        }
        public string getEmailCreator ()
        {
            return this.emailCreator;
        }
        public void addAssignedUser (string email)
        {
            this.assignedUsers.Add(email);
        }
        public List<string> getAssignedUsers ()
        {
            return assignedUsers;
        }



    }
}
