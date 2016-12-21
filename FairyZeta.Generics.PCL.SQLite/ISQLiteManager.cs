using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FairyZeta
{
    /// <summary> [IF] FZ／SQLiteマネージャー
    /// <para> => DBファイルへの操作（コネクション管理, テーブル操作 など）を定義する。 </para>
    /// </summary>
    public interface ISQLiteManager : IDbEntityManager
    {
        /*--- Property/Field Definitions ------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> DBファイル生成とコネクション生成を実行します。
        /// </summary>
        /// <param name="pDbFolderPath"> カレントフォルダからDBフォルダへのパス </param>
        /// <param name="pDbFileName"> DBファイルの名称 </param>
        /// <returns> 操作結果 </returns>
        Task<List<SQLiteOperationLog>> CreateDbFileAndConnectionAsync(string pDbFolderPath, string pDbFileName);

        /// <summary> DBファイル内に新しいテーブルを追加します。
        /// </summary>
        /// <param name="pAddTableType"> 追加するテーブルの型 </param>
        /// <param name="pConnectionCheck"> テーブル操作前にコネクションを確認する場合<code>True</code> </param>
        /// <returns> 操作結果 </returns>
        Task<List<SQLiteOperationLog>> AddTableAsync(Type pAddTableType, bool pConnectionCheck);

        /// <summary> DBファイル内のテーブルを削除します。
        /// </summary>
        /// <typeparam name="T"> 削除するテーブルの型 </typeparam>
        /// <returns> 操作結果 </returns>
        Task<List<SQLiteOperationLog>> DeleteTableAsync<T>() where T : new();

        /// <summary> このマネージャーが管理しているDBファイルとコネクションを削除します。
        /// </summary>
        /// <returns> 操作結果 </returns>
        Task<List<SQLiteOperationLog>> DeleteDbFileAndConnectionAsync();

        /*--- Method: public ------------------------------------------------------------------------------------------------------------------------------------------*/


    }
}
