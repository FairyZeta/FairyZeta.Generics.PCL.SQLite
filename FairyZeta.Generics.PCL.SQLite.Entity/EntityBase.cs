using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FairyZeta.Generics.PCL.SQLite.TEST.Entity
{
    public abstract class EntityBase
    {
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }

    }
}
