using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class dataColumn : dal
    {
        // ------------------- names of the columns in the Column table -------------------------------------------------------------- 
        public const string columnBoardId = "boardId";
        public const string columnOrdinal = "columnOrdinal";
        public const string columnName = "name";
        public const string columnLimit = "limited"; 
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("dataColumn");
        // ------------------- values to save in the DATABASE ------------------------------------------------------------------------ //
        public int ordinal { get; set; }
        public string name { get; set; }
        public int limit { get; set; }
        public int boardId { get; set; }

        public dataColumn(int boardId, int columnOrdinal, int limit, string name) : base(new dataColumnController())
        {
            this.name = name;
            this.boardId = boardId;
            this.ordinal = columnOrdinal;
            this.limit = limit;

        }

       
        
    }
}
