using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class NotifiableModelObject : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        public NotifiableModelObject(BackendController controller)
        {
            this.Controller = controller;
        }
    }
}
