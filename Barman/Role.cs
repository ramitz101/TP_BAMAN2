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

        public Role(string code)
        {
            Code = code;
        }

        public Role(string code, int pIdRole)
        {
            Code = code;
            IdRole = pIdRole;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

           Role m = obj as Role;

            if (m == null)
            {
                return false;
            }

            return this.IdRole == m.IdRole;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
