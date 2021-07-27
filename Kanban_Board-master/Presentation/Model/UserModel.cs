using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class UserModel : NotifiableModelObject
    {
        //Properties---------------------------------------------------------------------------------
        private string email;
        public string Email
        {
            get {
                return email;
            }
            set {
                email = value;
                RaisePropertyChanged("Email");
            }
        }
        //Constructor---------------------------------------------------------------------------------

        public UserModel(BackendController controller, string email): base(controller) {
            this.Email = email;
        }
        //Methods---------------------------------------------------------------------------------

        public BoardModel getBoard()
        {
            return new BoardModel(Controller, this);
        }
    }
}
