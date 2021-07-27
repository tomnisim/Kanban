using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class dataBoard : dal
    {
        // ------------------- names of the columns in the Board table -------------------------------------------------------------- // 
        public const string boardEmailCreator = "emailCreator";
        public const string boardTaskCounterKey = "taskCounterKey";
        public const string boardColumnCounterKey = "columnCounterKey";
        public const string boardBoardId = "boardId";
        // ------------------- values to save in the DATABASE ------------------------------------------------------------------------ //
        public string emailCreator;
        public int boardId;
        public int taskCounterKey;
        public int columnCounterKey;
        public dataBoard(string emailCreator, int taskCounterKey, int columnCounterKey,int boardId): base(new dataBoardController())
        {
            this.boardId = boardId;
            this.emailCreator = emailCreator;
            this.taskCounterKey =taskCounterKey;
            this.columnCounterKey = columnCounterKey;

        }

    }
}
