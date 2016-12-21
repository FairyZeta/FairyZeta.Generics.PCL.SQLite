using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyZeta.Generics.PCL.SQLite.TEST.Entity;
using System.Diagnostics;

namespace FairyZeta.Generics.PCL.SQLite.TEST
{
    public abstract class SQLiteTestBase
    {

        /*--- Property/Field Definitions ------------------------------------------------------------------------------------------------------------------------------*/

        /*--- Constructers --------------------------------------------------------------------------------------------------------------------------------------------*/

        /*--- Method: Initialization ----------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: public ------------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: protected ---------------------------------------------------------------------------------------------------------------------------------------*/

        protected virtual List<Type> GetTestEntityTypes()
        {
            return new List<Type>() { typeof(PersonEntity), typeof(TodoEntity), typeof(UserEntity) };
        }

        protected virtual Type GetTestEntityType()
        {
            return typeof(PersonEntity);
        }

        protected virtual GenericSQLiteManager CreateManager()
        {
            return new GenericSQLiteManager(true);
        }


        protected virtual void OutputLogs(GenericSQLiteManager pGenericSQLiteManager)
        {
            foreach (var item in pGenericSQLiteManager.GetLogs())
            {
                Debug.WriteLine(item.GetLogFormat());
            }
        }

        #region --- DataBase Process ---

        #endregion

        #region --- Entity Process ---


        #endregion

        /*--- Method: internal ----------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: private -----------------------------------------------------------------------------------------------------------------------------------------*/

    }
}
