using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOGenerator
{
    class UserWorkItem
    {
        internal Delegate Task { get; private set; }
        internal object[] State { get; private set; }

        internal UserWorkItem(Delegate task, object[] state)
        {
            Task = task;
            State = state;
        }
    }
}
