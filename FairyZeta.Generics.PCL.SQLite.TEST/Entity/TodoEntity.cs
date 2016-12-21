using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyZeta.Generics.PCL.SQLite.TEST.Entity
{
    public class TodoEntity : EntityBase
    {
        public string Detail { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpDate { get; set; }
    }
}
