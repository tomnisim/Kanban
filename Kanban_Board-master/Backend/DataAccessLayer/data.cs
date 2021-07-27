using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class Data
    {
        public Data() { }

        public Dictionary<string, BusinessLayer.User> load() {
            string objestAsJson = File.ReadAllText("data.json");
            Dictionary<string, BusinessLayer.User> temp = JsonSerializer.Deserialize<Dictionary<string, BusinessLayer.User>>(objestAsJson);
            return temp;
        }
        public void save( Dictionary<string, BusinessLayer.User> usersToSave) {
                string objetAsJson = JsonSerializer.Serialize(usersToSave);
                File.WriteAllText("data.json", objetAsJson);
        }
    }
}

