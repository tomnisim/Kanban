using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly IReadOnlyCollection<Task> Tasks;
        public readonly string Name;
        public readonly int Limit;
        public readonly int ordinal;
        internal Column(IReadOnlyCollection<Task> tasks, string name, int limit, int ordinal)
        {
            this.Tasks = tasks;
            this.Name = name;
            this.Limit = limit;
            this.ordinal = ordinal;
        }
        // You can add code here
    }
}