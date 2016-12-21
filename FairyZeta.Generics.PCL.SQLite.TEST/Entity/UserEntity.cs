using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyZeta.Generics.PCL.SQLite.TEST.Entity
{
    public class UserEntity : EntityBase
    {
        public string UserID { get; set; }

        public string Mail { get; set; }

        public string Pass { get; set; }
    }
}
