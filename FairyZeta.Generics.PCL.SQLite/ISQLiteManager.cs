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
        /// <param name="pDbFolderPath"> DBフォルダへのパス </param>
        /// <param name="pDbFileName"> DBファイルの名称 </param>
        /// <param name="pAutoTableCreate"> ファイルとコネクション生成後、自動テーブル生成の設定
        /// <para> =><code>True</code>の場合はテーブルを自動生成し、<code>False</code>の場合は自動生成しない </para>
        /// </param>
        /// <returns> 処理結果コード </returns>
        Task<List<SQLiteOperationLog>> CreateDbFileAndConnectionAsync(string pDbFolderPath, string pDbFileName, bool pAutoTableCreate = true);

        /// <summary> DBファイル内のテーブル生成を実行します。
        /// </summary>
        /// <returns> 処理結果コード </returns>
        Task<int[]> CreateTableAsync();

        /// <summary> DBファイル内に新しいテーブルを追加します。
        /// </summary>
        /// <param name="pAddTableTypes"> 追加するテーブルの型式 </param>
        /// <returns> 処理結果コード </returns>
        Task<int> AddTableAsync(Type pAddTableType);

        /// <summary> DBファイル内のテーブルを削除します。
        /// </summary>
        /// <typeparam name="T"> 削除するテーブルの型式 </typeparam>
        /// <returns> 処理結果コード </returns>
        Task<int> DeleteTableAsync<T>() where T : new();

        /*--- Method: public ------------------------------------------------------------------------------------------------------------------------------------------*/
        

    }
}
