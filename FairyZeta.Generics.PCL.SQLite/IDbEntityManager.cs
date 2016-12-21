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
        
        /// <summary> エンティティの追加を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 追加するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        Task<int> InsertEntityAsync<T>(T pEntity) where T : new();

        /// <summary> エンティティの更新を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 更新するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        Task<int> UpdateEntityAsync<T>(T pEntity) where T : new();

        /// <summary> エンティティの削除を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 削除するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        Task<int> DeleteEntityAsync<T>(T pEntity) where T : new();
    }
}
