using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;




namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class dal // an object that save the dalController
    {
       public const string IDColumnName = "ID";
       protected dalController controller;
       protected dal (dalController controller)
       {
            this.controller = controller;
       }
        protected dal() { }
        
    }
}
