using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class RegisterViewModel : NotifiableObject
    {
        //Properties-----------------------------------------------------------------------------------------

        public BackendController Controller { get; private set; }
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
        private string password;
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
        private string nickname ;
        public string Nickname
        {
            get
            {
                return nickname;
            }
            set
            {
                nickname = value;
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
        private string emailHost;
        public string EmailHost
        {
            get
            {
                return emailHost;
            }
            set
            {
                emailHost = value;
            }
        }
        //Constrctor-----------------------------------------------------------------------------------------
        public RegisterViewModel(BackendController controller)
        {
            this.Controller = controller;
        }
        //Methods-----------------------------------------------------------------------------------------

        public UserModel Register()
        {
            Message = "";
            if (EmailHost==null)
            {
                try
                {
                    return Controller.Register(Username, Password, Nickname);
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    return null;
                }
            }
            else
            {
                try
                {
                    return Controller.Register(Username, Password, Nickname, EmailHost);
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    return null;
                }
            }
            return null;
            
        }

    }
}
