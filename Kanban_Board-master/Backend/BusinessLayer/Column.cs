using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Column    
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusinessLayer.Column");
        private List<Task> tasks { get; set; }
        private int limit { get; set; }
        private string name { get; set; }
        private int boardId { get; set; }
        private int key;

        public Column() { }//empty constructor
        public Column (string name,int columnOrdinal, int boardId)
        {
            if (!validColumnName(name))
            {
                log.Warn("invalid name");
                throw new Exception("invalid name");
            }
            this.name = name;
            tasks = new List<Task>(); //New empty list without tasks
            this.key = columnOrdinal;
            this.limit = 100;
            this.name = name;
            this.boardId = boardId;

            //inserting to database
            dataColumnController DCC = new dataColumnController();
           DCC.Insert(this.toDalObject());
        }
        public Column(dataColumn dataColumn,List<Task> tasks)
        {
            this.limit = dataColumn.limit;
            this.name = dataColumn.name;
            this.tasks = tasks;
            this.boardId = dataColumn.boardId;
            this.key = dataColumn.ordinal;
        }

        public void addTaskForAdvance(Task toAdd) //Helping function for advanceTask()
        {
            tasks.Add(toAdd);
        }  
        public Task removeTask(int key)
        {
            Task toRemove = null;
            foreach (Task task in tasks)
            {
                if (task.getKey() == key)
                {
                    toRemove = task;
                }
            }
            if (toRemove != null)
            {
                tasks.Remove(toRemove);
               
            }
            return toRemove; //can be null
        }
        virtual public bool addTask(Task toInsert)
        {
            if (tasks.Count < limit | limit==-1)//limited amount of tasks in Column or no limited Column
            {
                tasks.Add(toInsert);
                return true;
            }
            else
                return false;//  Exception is thrown in User class
        }
        public Task findTask (int key) //Find a task by taskID
        {
            Task toFind = null;
            bool found = false;
            foreach (Task task in tasks) if(!found)
            {
                if (task.getKey() == key)
                {
                    toFind = task;
                    found = true;
                }
            }
            return toFind;
        }
        private bool validColumnName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 15) //Name check
            {
                return false;
            }
            return true;
        }
        public void changeName(string newName)
        {
            if (!validColumnName(newName))
            {
                log.Warn("invalid name");
                throw new Exception("invalid name");
            }
            this.name = newName;
            dataColumnController DCC = new dataColumnController();
            DCC.Update(boardId, key, "name", newName);
        }
        public void editTaskDueDate(int taskId, DateTime dueDate, string email)
        {
            Task toEdit = findTask(taskId);
            if (toEdit != null) //if task is found
            {
                toEdit.setDueDate(dueDate, email);
            }
            else
            {
                log.Warn("Task does not exist");
                throw new Exception("Task does not exist");
            }
        }
        public void editTaskTitle(int taskId, string title, string email)
        {
            Task toEdit = findTask(taskId);
            if (toEdit != null)//if task is found
            {
                toEdit.setTitle(title, email);
            }
            else
            {
                log.Warn("Task does not exist");
                throw new Exception("Task does not exist");
            }
        }
        public void editTaskDescription(int taskId, string description, string email)
        {

            Task toEdit = findTask(taskId);
            if (toEdit != null)//if task is found
            {

                toEdit.setDescription(description, email);
            }
            else
            {
                log.Warn("Task does not exist");
                throw new Exception("Task does not exist");
            }
        }
        public void setLimit(int limit)//sets the limit field
        {
            if ((limit<0 & limit !=-1) | limit < tasks.Count) //Limit check
            {
                log.Warn("invalid limit");
                throw new Exception("invalid limit");
            }
            this.limit = limit;
            //Updating database
            DataAccessLayer.dataColumnController DCC = new DataAccessLayer.dataColumnController();
            DCC.Update(boardId, key, "limited", limit);
        }
        public bool isFull()
        {
            if (limit == -1) //Unlimited
                return false;
            return !(tasks.Count < limit);
        }
        public int getLimit() { return this.limit; }
        public IReadOnlyCollection<Task>  getTasks() { return this.tasks; }
        public List<Task> getTasksForColumns() { return this.tasks; }
        public string getName()
        {
            return this.name;
        }

        public void setKey(int key) {
            
            dataColumnController DCC = new dataColumnController();
            DCC.Update(boardId, this.key, "columnOrdinal", key);
            this.key = key;//must be after writing to data base
        }
        public int getKey() { return key; }
        public dataColumn toDalObject() //Converting this object to data object
        {
            dataColumn toReturn = new dataColumn(boardId, key, limit, name);
            return toReturn;
        }
        public void DeleteData()
        {
            this.tasks = new List<Task>(); //Deleting all taskss by creating a new empty list
        }
        public void AssignTask(int taskId, string emailAssignee, string email)
        {
            Task toEdit = findTask(taskId);
            if (toEdit==null)
            {
                log.Warn("invalid limit");
                throw new Exception("invalid limit");
            }
            toEdit.setEmailAssignee(emailAssignee, email);

        }
        public void DeleteTask(int taskId,string email)
        {
            Task toDelete = findTask(taskId);
            if (!toDelete.getEmailAssignee().Equals(email))
            {
                log.Warn("only assignee user can delete his tasks");
                throw new Exception("only assignee user can delete his tasks");
            }
            if (toDelete==null)
            {
                log.Warn("task does not exist");
                throw new Exception("task does not exist");
            }
            tasks.Remove(toDelete);
            // update in DB
            dataTaskController DTC = new dataTaskController();
            DTC.Delete(boardId, taskId);

        }
    }
}
