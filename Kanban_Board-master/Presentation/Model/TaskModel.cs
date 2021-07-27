using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Presentation.Model
{
    public class TaskModel : NotifiableModelObject
    {
        //Properties------------------------------------------------------------
        private DateTime creationTime;
        public DateTime CreationTime
        {
            get
            {
                return creationTime;
            }
            set
            {
                creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        private DateTime dueDate;
        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string emailCreator;
        public string EmailCreator
        {
            get
            {
                return emailCreator;
            }
            set
            {
                emailCreator = value;
                RaisePropertyChanged("EmailCreator");
            }
        }
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        private string emailAssignee;
        public string EmailAssignee
        {
            get
            {
                return emailAssignee;
            }
            set
            {
                emailAssignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }
        private int taskId;
        public int TaskId
        {
            get { return taskId; }
            set
            {
                this.taskId = value;
                RaisePropertyChanged("TaskId");
            }
        }
        private SolidColorBrush backGroundColor;
        public SolidColorBrush BackGroundColor
        {
            get { return backGroundColor; }
            set
            {
                this.backGroundColor = value;
                RaisePropertyChanged("BackGroundColor");
            }
        }
        private SolidColorBrush fontColor;
        public SolidColorBrush FontColor
        {
            get { return fontColor; }
            set
            {
                this.fontColor = value;
                RaisePropertyChanged("FontColor");
            }
        }

        //Constructor------------------------------------------------------------
        public TaskModel(BackendController controller, string emailAssignee, DateTime creationTime, DateTime dueDate, string title, string description, int taskId) : base(controller)
        {
            this.EmailAssignee = emailAssignee;
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.TaskId = taskId;
            this.Description = description;
            this.FontColor = this.findFontColor(creationTime, dueDate);
            // this.BackGroundColor = this.findBackGroundColor(emailCreator, emailAssignee);
            //  this.BackGroundColor = new SolidColorBrush(Colors.Red);

        }
        public TaskModel(BackendController controller, Task task) : this(controller, task.emailAssignee, task.CreationTime, task.DueDate, task.Title, task.Description, task.Id)
        {

        }
        //Methods------------------------------------------------------------
        
        
        private SolidColorBrush findFontColor(DateTime creationTime, DateTime dueDate)
        {

            double diffrence = (dueDate - creationTime).TotalDays; // all the time
            double diffrence12 = (DateTime.Now - creationTime).TotalDays; // past time

            if (dueDate.CompareTo(DateTime.Now) < 0)
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                if (diffrence * 0.75 < diffrence12)
                    return new SolidColorBrush(Colors.Orange);
                else
                    return new SolidColorBrush(Colors.Black);
            }
        }
        private SolidColorBrush findBackGroundColor(string emailCreator,string emailAssignee)
        {
            return new SolidColorBrush(emailCreator.Equals(EmailAssignee) ? Colors.Blue : Colors.White);
        }
    }
}