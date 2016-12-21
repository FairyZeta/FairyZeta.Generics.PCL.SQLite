using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyZeta.Generics.PCL.SQLite.TEST.Entity
{
    public class PersonEntity : EntityBase
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
