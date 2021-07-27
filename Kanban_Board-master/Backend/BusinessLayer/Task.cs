using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusniessLayer.Task");
        private DateTime creationTime { get; set; }
        private DateTime dueDate { get; set; }
        private string title { get; set; }
        private string description { get; set; }
        private int key { get; set; }
        private int boardId;
        private int columnId;
        private string emailAssignee;
        private const int MaxLengthTitle = 50;



        public Task(string des) //Constructor for tests
        {
            this.description = des;
        }
        public Task(DateTime creationTime) //Constructor for tests
        {
            this.creationTime = creationTime;
        }
        public Task() { }//empty constructor
        public Task(DateTime dueDate, string title, int key, string email, int columnId, int boardId) // construcor without description
        {
            this.columnId = columnId;
            this.boardId = boardId;
            if (isValidTitle(title))
            {
                this.title = title;
            }
            this.creationTime = DateTime.Now;
            if (isValidDueDate(dueDate, creationTime))
            {
                this.dueDate = dueDate;
            }
            
            
            this.description = null;
            this.key = key;
            this.emailAssignee = email;

            //inserting to database
            DataAccessLayer.dataTaskController DTC = new DataAccessLayer.dataTaskController();
            DTC.Insert(this.toDalObject());
        }
        public Task (DateTime dueDate, string title, string description, int key, string email, int columnId,int boardId) // construcor with description
        {
            this.emailAssignee = email;
            this.columnId = columnId;
            this.boardId = boardId;
            if (isValidTitle(title))
            {
                this.title = title;
            }
            this.creationTime = DateTime.Now;
            if (isValidDueDate(dueDate, creationTime))
            {
                this.dueDate = dueDate;
            }
            if (isValidDescription(description))
            {
                this.description = description;
            }
            this.dueDate = dueDate;
            this.title = title;
            this.key = key;



            //inserting to database
            DataAccessLayer.dataTaskController DTC = new DataAccessLayer.dataTaskController();
            DTC.Insert(this.toDalObject());
        }
        public Task(dataTask dataTask) //Copy constructor from data object to business
        {
            if (dataTask.description.Equals(" "))//A null description is saved as " " because database doesnt handle null value
                this.description = null;
            else
            {
                this.description = dataTask.description;
            }
            this.dueDate = dataTask.dueDate;
            this.creationTime = dataTask.creationTime;
            this.title = dataTask.title;           
            this.key = dataTask.id;
            this.boardId = dataTask.boardId;
            this.columnId = dataTask.column;
            this.emailAssignee = dataTask.emailAssignee;
        }


        public DataAccessLayer.dataTask toDalObject()
        {
            if (description==null)//database doesnt handle null
            {
                description = " ";
            }
            DataAccessLayer.dataTask toReturn = new DataAccessLayer.dataTask(boardId, columnId, key, title, description, dueDate, creationTime, emailAssignee);
            return toReturn;
        }
        public bool isValidDueDate(DateTime dueDate, DateTime creationTime)
        {
          
            if (dueDate.Date.CompareTo(creationTime.Date) < 0) //DateTime is not a nullable object
            {
                log.Warn("invalid due date");
                throw new Exception("invalid due date");
            }
            return true;
        }
        public bool isValidTitle(string title)
        {
            if (title == null)
            {
                log.Warn("the title can not be null");
                throw new Exception("the title can not be null");
            }
            if (title.Length == 0)
            {
                log.Warn("the title can not be empty");
                throw new Exception("the title can not be empty");
            }
            if (title.Length > MaxLengthTitle)
            {
                log.Warn("the title must have maximun 50 characters");
                throw new Exception("the title must have maximun 50 characters");
            }
            return true;
        }
        public bool isValidDescription(string description)
        {
            if (description!=null && description.Length > 300)
            { //Description length is over 300 chars
                log.Warn("the description must have maximun 300 characters");
                throw new Exception("the description must have maximun 300 characters");
            }
            return true;
        }
        public int getBoardId() { return this.boardId; }
        public int getColumnId()
        {
            return this.columnId;
        }
        public string getTitle() { return title; }
        public DateTime getCreationTime() { return creationTime; }
        public DateTime getDueDate() { return dueDate; }
        public string getDescription() { return description; }
        public int getKey() { return key; }
        public virtual void setColumnId(int colomnId)
        {
            //Updating database
            dataTaskController DTC = new dataTaskController();
            DTC.Update(boardId, this.key, "column", colomnId);
            //Updating field
            this.columnId = colomnId;

        }
        public void setTitle(string title, string email)
        {

            if (isValidTitle(title))
            {
                if (isAssignedUser(email))
                {
                    this.title = title;
                    //Updating database
                    dataTaskController DTC = new dataTaskController();
                    DTC.Update(boardId, key, "title", title);
                }
            }
                       
        } 
        public void setDueDate(DateTime dueDate, string email)
        {

            if (isValidDueDate(dueDate,DateTime.Now))
            {
                if (isAssignedUser(email))
                {
                    this.dueDate = dueDate;
                    //Updating database
                    dataTaskController DTC = new dataTaskController();
                    DTC.Update(boardId, key, "dueDate", dueDate);
                }
            }
           

        }
        public bool isAssignedUser(string email)
        {
            if (!this.emailAssignee.Equals(email))
            {
                log.Warn("only task assignee can edit task");
                throw new Exception("only task assignee can edit task");
            }

            return true;
        }
        public void setEmailAssignee(string emailAssignee, string email)
        {
            if (isAssignedUser(email))
            {
                this.emailAssignee = emailAssignee;
                //Updating database
                dataTaskController DTC = new dataTaskController();
                DTC.Update(boardId, key, "emailAssignee", emailAssignee);
            }

        }
        public void setDescription(string description,string email)
        {


            if (isValidDescription(description))
            {
                if (isAssignedUser(email))
                {
                    this.description = description;
                    //Updating database
                    dataTaskController DTC = new dataTaskController();
                    DTC.Update(boardId, key, "description", description);
                }
            }
            
        }
        public string getEmailAssignee()
        {
            return this.emailAssignee;
        }
       
        
    }
}
