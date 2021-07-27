using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("UserService");

        public UserService()
        {

        }
        public Response LoadData(BusinessLayer.UserController uc)
        {
            try
            {
                uc.loadata();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            log.Info("Data loaded succesfully");
            return new Response(); //Succesfull data load
        }

        public Response<User> Login(string email, string password,UserController uc)
        {
            try
            {

                uc.getUser(email).login(password);
            }
            catch (Exception e)
            {
                return new Response<User>(e.Message);
            }
            User serUser = new User(email, uc.getUser(email).getNickname());
            log.Info("Logged in succesfully");
            return new Response<User>(serUser); //Succesfull Login
        }
        public Response Logout(string email, UserController uc)
        {
            try
            {
                uc.logout(email);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            log.Info("Lougout succesfully");
            return new Response(); //Succesfull Logout
        }
        public Response DeleteData(UserController uc)
        {
            try
            {
                uc.DeleteData();
            }
            catch(Exception e)
            {
                
                return new Response(e.Message);
            }
            log.Warn("data deleted succesfully");
            return new Response();//Succesfull Deletion
        }

        public Response Register(string email, string password, string nickname, UserController uc, string emailHost)
        {
            try
            {
                uc.Register(email, nickname, password, emailHost);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            this.Login(email, password, uc);
            log.Info("Register done succesfully");
            return new Response(); //Succesfull Register
        }
        public Response Register(string email, string password, string nickname, UserController uc)
        {

            try
            {
                uc.Register(email, nickname, password);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            this.Login(email, password, uc);
            log.Info("Register done succesfully");
            return new Response(); //Succesfull Register
        }

    }
}
