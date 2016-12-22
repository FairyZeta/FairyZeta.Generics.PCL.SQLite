using System.Collections.Generic;
using System.Threading.Tasks;

namespace FairyZeta
{
    /// <summary> [IF] FZ／データベースマネージャー
    /// <para> => データベース操作（ins, del など）を定義する。 </para>
    /// </summary>
    public interface IDbEntityManager
    {
        /*--- Property/Field Definitions ------------------------------------------------------------------------------------------------------------------------------*/

        /*--- Method: public ------------------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> 全てのエンティティを取得します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <returns> エンティティリスト </returns>
        Task<List<T>> GetEntitysAsync<T>() where T : new();

        /// <summary> エンティティの追加または更新を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 更新するエンティティ列挙 </param>
        /// <returns> 処理結果コードリスト </returns>
        Task<List<int>> InsertOrUpdateEntityAsync<T>(IEnumerable<T> pEntitys) where T : new();

        /// <summary> エンティティの追加または更新を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 更新するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        Task<int> InsertOrUpdateEntityAsync<T>(T pEntity) where T : new();

        /// <summary> エンティティの削除を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 削除するエンティティ列挙 </param>
        /// <returns> 処理結果コードリスト </returns>
        Task<List<int>> DeleteEntityAsync<T>(IEnumerable<T> pEntitys) where T : new();

        /// <summary> エンティティの削除を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 削除するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        Task<int> DeleteEntityAsync<T>(T pEntity) where T : new();

    }
}
