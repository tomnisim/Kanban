
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class dataUser : dal
    {
        // ------------------- names of the columns in the User table -------------------------------------------------------------- 

        public const string userEmail = "email";
        public const string userPassword = "password";
        public const string userNickName= "nickname";
        public const string userBoardId = "boardId";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("dataUser");
        // ------------------- values to save in the DATABASE ------------------------------------------------------------------------ 
        public string email { get; set; }
        public string password { get; set; }
        public string nickname { get; set; }
        public int boardId { get; set; }

        public dataUser(string email, string password, string nickname,int boardId ) :base(new dataUserController())
        {
            this.email = email;
            this.password = password;
            this.nickname = nickname;
            this.boardId = boardId;

        }


       

    }
}
