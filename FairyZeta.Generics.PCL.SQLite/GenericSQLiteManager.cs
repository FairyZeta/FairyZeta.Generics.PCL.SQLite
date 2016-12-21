using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyZeta.Generics.PCL.SQLite
{
    /// <summary> FZ.GN／PCL.汎用SQLiteマネージャー
    /// <para> => SQLiteのデータ入出力に必要な基本機能を提供します。 </para>
    /// </summary>
    public partial class GenericSQLiteManager : ISQLiteManager
    {
        /*--- Property/Field Definitions ------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> SQLite内テーブルリスト
        /// </summary>
        public List<Type> TableList { get; private set; }

        /// <summary> SQLite操作記録
        /// </summary>
        public List<SQLiteOperationLog> LogList { get; private set; }

        /// <summary> DBへの接続設定
        /// </summary>
        public SQLiteConnection Connection { get; private set; }

        /// <summary> ログ記録の設定
        /// </summary>
        public bool IsRecordLog { get; set; }

        /// <summary> このマネージャーがが対象としているファイル情報
        /// </summary>
        public IFile DbFile { get; private set; }

        /*--- Constructers --------------------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> FZ.GN／PCL.汎用SQLiteマネージャー／コンストラクタ
        /// </summary>
        /// <param name="pTables"> このSQLiteマネージャーで使用するテーブル列挙 </param>
        /// <param name="pIsRecordLog"> 操作ログをリストに記録する場合 <code>True</code> </param>
        public GenericSQLiteManager(IEnumerable<Type> pTables, bool pIsRecordLog = false)
        {
            this.LogList = new List<SQLiteOperationLog>();
            this.IsRecordLog = pIsRecordLog;

            this.TableList = new List<Type>();
            if (pTables != null)
            {
                foreach (var item in pTables)
                {
                    this.AddLog(this.AddTableType(item));
                }
            }

            if (this.TableList.Count() == 0)
            {
                this.AddLog(this.CreateLog(SQLiteOperationResult.Warning, "a entity target does not exist."));
            }
        }

        /*--- Method: Initialization ----------------------------------------------------------------------------------------------------------------------------------*/

        /*--- Method: public ------------------------------------------------------------------------------------------------------------------------------------------*/

        #region --- DataBase Process ---

        /// <summary> DBファイル生成とコネクション生成を実行します。
        /// </summary>
        /// <param name="pDbFolderPath"> DBフォルダへのパス </param>
        /// <param name="pDbFileName"> DBファイルの名称 </param>
        /// <param name="pAutoTableCreate"> ファイルとコネクション生成後、自動テーブル生成の設定 </param>
        /// <returns> 操作結果ログ </returns>
        public virtual async Task<List<SQLiteOperationLog>> CreateDbFileAndConnectionAsync(string pDbFolderPath, string pDbFileName, bool pAutoTableCreate = true)
        {
            List<SQLiteOperationLog> results = new List<SQLiteOperationLog>();

            // パラメータチェック
            if (string.IsNullOrWhiteSpace(pDbFolderPath))
            {
                results.Add(this.CreateLog(SQLiteOperationResult.Failure, "<Param Error> DbFolderPath Empty."));
            }
            else if (string.IsNullOrEmpty(pDbFileName))
            {
                results.Add(this.CreateLog(SQLiteOperationResult.Failure, "<Param Error> DbFileName Empty."));
            }
            else
            {
                // ルートフォルダを取得する
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                // フォルダチェック
                if (await rootFolder.CheckExistsAsync(pDbFolderPath) == ExistenceCheckResult.NotFound)
                {
                    await rootFolder.CreateFolderAsync(pDbFolderPath, CreationCollisionOption.FailIfExists);
                    results.Add(await this.CreateLogAsync(SQLiteOperationResult.Success, string.Format("Create new folder success. {0}", pDbFolderPath)));
                }

                // DBファイル存在チェック
                var dbFullPath = System.IO.Path.Combine(pDbFolderPath, pDbFileName);
                if (await rootFolder.CheckExistsAsync(dbFullPath) == ExistenceCheckResult.NotFound)
                {
                    // 存在しなかった場合、新たにDBファイルを作成
                    this.DbFile = await rootFolder.CreateFileAsync(dbFullPath, CreationCollisionOption.ReplaceExisting);
                    results.Add(await this.CreateLogAsync(SQLiteOperationResult.Success, string.Format("Create new DbFile success. {0}", pDbFileName)));
                }
                else
                {
                    // 存在した場合、ファイルオープン
                    this.DbFile = await rootFolder.CreateFileAsync(dbFullPath, CreationCollisionOption.OpenIfExists);
                    results.Add(await this.CreateLogAsync(SQLiteOperationResult.Success, string.Format("DbFile open success. {0}", pDbFileName)));
                }

                //コネクションを作成する
                this.Connection = new SQLiteConnection( this.DbFile.Path);
                results.Add(await this.CreateLogAsync(SQLiteOperationResult.Success, "Create Connection success. "));
            }

            await this.AddLogAsync(results);

            return results;
        }
        
        /// <summary> DBファイル内のテーブル生成を実行します。
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int[]> CreateTableAsync()
        {
            return await this.TableList.Select(async item =>
                await Task.Run(() => Connection.CreateTable(item))
            ).WhenAll();
        }

        /// <summary> DBファイル内に新しいテーブルを追加します。
        /// </summary>
        /// <param name="pAddTableTypes"> 追加するテーブル列挙 </param>
        /// <returns>  </returns>
        public virtual async Task<int[]> AddTableAsync(IEnumerable<Type> pAddTableTypes)
        {
            return await pAddTableTypes.Select(async item =>
                await this.AddTableAsync(item)
            ).WhenAll();
        }

        /// <summary> DBファイル内に新しいテーブルを追加します。
        /// </summary>
        /// <param name="pAddTableTypes"> 追加するテーブルタイプ </param>
        /// <returns> テーブルの生成に成功した場合 = 0, テーブルがすでに存在していた場合 = 1 </returns>
        public virtual async Task<int> AddTableAsync(Type pAddTableType)
        {
            return await Task.Run(() =>
            {
                List<SQLiteOperationLog> results = new List<SQLiteOperationLog>();

                results.Add(this.AddTableType(pAddTableType));

                if (results[0].SQLiteOperationResult != SQLiteOperationResult.Exists)
                {
                    this.Connection.CreateTable(pAddTableType);
                }
                return 0;
            });
        }


        /// <summary> DBファイル内のテーブルを削除します。
        /// </summary>
        /// <typeparam name="T"> 削除するテーブル型 </typeparam>
        /// <returns></returns>
        public virtual async Task<int> DeleteTableAsync<T>() where T : new()
        {
            await Task.Run(() => this.Connection.DropTable<T>());

            return 0;
        }

        #endregion

        #region --- Entity Process ---

        /// <summary> [IF.IDbEntityManager] 全てのエンティティを取得し、リストで返却します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <returns> エンティティリスト </returns>
        public virtual async Task<List<T>> GetEntitysAsync<T>() where T : new()
        {
            return await Task<T>.Run(() => this.GetEntitys<T>());
        }

        /// <summary> 全てのエンティティを取得し、リストで返却します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <returns> エンティティリスト </returns>
        public virtual List<T> GetEntitys<T>() where T : new()
        {
            return new List<T>(Connection.Table<T>().ToList());
        }

        /// <summary> [IF.IDbEntityManager] エンティティの追加を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 追加するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        public virtual async Task<int> InsertEntityAsync<T>(T pEntity) where T : new()
        {
            return await Task.Run(() => this.InsertEntity<T>(pEntity));
        }

        /// <summary> エンティティの追加を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 追加するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        public virtual int InsertEntity<T>(T pEntity) where T : new()
        {
            return this.InsertOrUpdateEntity<T>(pEntity);
        }

        /// <summary> [IF.IDbEntityManager] エンティティの更新を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 更新するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        public virtual async Task<int> UpdateEntityAsync<T>(T pEntity) where T : new()
        {
            return await Task.Run(() => this.UpdateEntity<T>(pEntity));
        }

        /// <summary> エンティティの更新を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 更新するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        public virtual int UpdateEntity<T>(T pEntity) where T : new()
        {
            return this.InsertOrUpdateEntity<T>(pEntity);
        }

        /// <summary> [IF.IDbEntityManager] エンティティの削除を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 削除するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        public virtual async Task<int> DeleteEntityAsync<T>(T pEntity) where T : new()
        {
            return await Task<T>.Run(() => this.DeleteEntity(pEntity));
        }

        /// <summary> エンティティの削除を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 削除するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        public virtual int DeleteEntity<T>(T pEntity) where T : new()
        {
            if (Connection.Table<T>().FirstOrDefault(e => e.Equals(pEntity)) == null)
                return 1;

            Connection.Delete(pEntity);

            return 0;
        }

        /// <summary> エンティティの追加または更新を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 更新するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        public virtual async Task<int> InsertOrUpdateEntityAsync<T>(T pEntity) where T : new()
        {
            return await Task<int>.Run(() => this.InsertOrUpdateEntity(pEntity));
        }

        /// <summary> エンティティの追加または更新を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 更新するエンティティ </param>
        /// <returns> 処理結果コード </returns>
        public virtual int InsertOrUpdateEntity<T>(T pEntity) where T : new()
        {
            if (Connection.Table<T>().FirstOrDefault(e => e.Equals(pEntity)) == null)
            {
                Connection.Insert(pEntity);
                return 0;
            }
            else
            {
                Connection.Update(pEntity);
                return 0;
            }
        }

        #endregion

        /*--- Method: protected ---------------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> 自身のテーブルリストにエンティティタイプを追加します。
        /// </summary>
        /// <param name="pAddTableTypes"> 追加するテーブルタイプ </param>
        /// <returns> 追加結果 </returns>
        protected virtual SQLiteOperationLog AddTableType(Type pAddEntityType)
        {
            if (this.TableList.Any(o => o.GetType() == pAddEntityType))
                return this.CreateLog(SQLiteOperationResult.Exists, string.Format("{0} => Entity Exists.", pAddEntityType.Name.ToString()));

            this.TableList.Add(pAddEntityType);

            return CreateLog(SQLiteOperationResult.Success, string.Format("{0} => Add TableList.", pAddEntityType.Name.ToString()));

        }


        protected virtual async Task<SQLiteOperationLog> CreateLogAsync(SQLiteOperationResult pSQLiteOperationResult, string pLogMsg)
        {
            return await Task.Run(() => this.CreateLog(pSQLiteOperationResult, pLogMsg));
        }

        protected virtual SQLiteOperationLog CreateLog(SQLiteOperationResult pSQLiteOperationResult, string pLogMsg)
        {
            return new SQLiteOperationLog() {
                SQLiteOperationResult = pSQLiteOperationResult,
                LogMsg = pLogMsg
            };
        }

        public virtual async Task<List<bool>> AddLogAsync(IEnumerable<SQLiteOperationLog> pSQLiteOperationLogs)
        {
            List<bool> results = new List<bool>();

            await pSQLiteOperationLogs.Select(async log =>
                await Task.Run(() => results.Add(this.AddLog(log)))
            ).WhenAll();

            return results;
        }

        protected virtual List<bool> AddLog(IEnumerable<SQLiteOperationLog> pSQLiteOperationLogs)
        {
            List<bool> results = new List<bool>();

            foreach (var log in pSQLiteOperationLogs)
            {
                results.Add(this.AddLog(log));
            }

            return results;
        }

        protected virtual async Task<bool> AddLogAsync(SQLiteOperationLog pSQLiteOperationLog)
        {
            return await Task.Run(() => this.AddLog(pSQLiteOperationLog));
        }

        protected virtual bool AddLog(SQLiteOperationLog pSQLiteOperationLog)
        {
            if (!this.IsRecordLog)
                return false;

            this.LogList.Add(pSQLiteOperationLog);

            return true;
        }
        /*--- Method: internal ----------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: private -----------------------------------------------------------------------------------------------------------------------------------------*/



    }
}
