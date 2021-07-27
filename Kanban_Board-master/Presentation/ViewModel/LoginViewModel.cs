using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{

    public class LoginViewModel : NotifiableObject
    {
        //Properties -------------------------------------------------------------------------------------------------------------

        private BackendController Controller;
       
        private string username ;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }
        private string password ;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
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
        //Constructor-------------------------------------------------------------------------------------------------------------

        public LoginViewModel(BackendController controller)
        {
            this.Controller = controller;
        }
        //Methods-------------------------------------------------------------------------------------------------------------
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Username, Password);
            }
            catch(Exception e)
            {
                Message = e.Message;
                return null;
            }
            
        }
    }
}
