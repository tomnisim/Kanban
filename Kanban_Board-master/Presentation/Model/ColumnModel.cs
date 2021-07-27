using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        //Properties------------------------------------------------------------------
        
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        private int ordinal;
        public int Ordinal
        {
            get
            {
                return ordinal;
            }
            set
            {
                ordinal = value;
                RaisePropertyChanged("Ordinal");
            }
        }

        private int limit;
        public int Limit
        {
            get
            {
                return limit;
            }
            set
            {
                limit = value;
                RaisePropertyChanged("Limit");
            }
        }
        public string email;
        private ObservableCollection<TaskModel> tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get
            {
                return tasks;
            }
            set
            {
                tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }
        //Constructor---------------------------------------------------------------------------------------------
        public ColumnModel(BackendController controller, string name, int limit, string email, int ordinal) : base(controller)
        {
            this.Limit = limit;
            this.Name = name;
            this.email = email;
            this.Ordinal = ordinal;
        }
        public ColumnModel(BackendController controller, Column column, string email, int ordinal) : this(controller, column.Name, column.Limit, email, ordinal)
        {
            this.Tasks = new ObservableCollection<TaskModel>(column.Tasks.Select((c, i) => new TaskModel(controller, c)));
        }

       
        //Methods--------------------------------------------------------------------------------
        public void addTask(TaskModel task)
        {
            Tasks.Add(task);
        }

        
        public TaskModel getTask(int taskId) //if taskId doesnt exists returns null
        {
            foreach(var item in this.Tasks)
            {
                if (item.TaskId == taskId)
                    return item;
            }
            return null;
        }
        

    }
}
