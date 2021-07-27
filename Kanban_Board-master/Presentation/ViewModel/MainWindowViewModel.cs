using Presentation.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Presentation
{
    public class MainWindowViewModel : NotifiableObject
    {
        //Properties-------------------------------------------------------------------------------------------------------------

        public BackendController Controller { get; private set; }

        
        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                RaisePropertyChanged("Username");
            }
        }
        private string password="";
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string message = "";
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                RaisePropertyChanged("Message");
            }
        }
        //Constructor-----------------------------------------------
        public MainWindowViewModel()
        {

            Controller = new BackendController();
        }
        //Methods-----------------------------------------------
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

      
    }
}
