using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class dataTask : dal
    {
        // ------------------- names of the columns in the Task table -------------------------------------------------------------- //
        public const string taskBoardId = "boardId";
        public const string taskId = "taskId";
        public const string taskColumn = "column";
        public const string taskTitle = "title";
        public const string taskDescription = "description";
        public const string taskDueDate = "dueDate";
        public const string taskCreationTime = "creationTime";
        public const string taskEmailAssignee = "emailAssignee";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("dataTask");
        // ------------------- values to save in the DATABASE ------------------------------------------------------------------------ //
        public int id { get; set; }
        public int column { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime dueDate { get; set; }
        public DateTime creationTime { get; set; }
        public int boardId { get; set; }
        public string emailAssignee { get; set; }
        public dataTask() :base()

        {

        }
        public dataTask(int boardId, int column, int taskID, string title,string description, DateTime dueDate,DateTime creationTime, string emailAssignee) :base (new dataTaskController())
        {
            this.boardId = boardId;
            this.column = column;
            this.id = taskID;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.creationTime = creationTime;
            this.emailAssignee = emailAssignee;
        }
    }
}
