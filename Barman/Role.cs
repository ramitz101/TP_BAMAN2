using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class Role
    {
        public virtual int? IdRole { get; set; }
        public virtual string Code { get; set; }

        public Role()
        {
            IdRole = null;
            Code = String.Empty;
        }
    }
}
