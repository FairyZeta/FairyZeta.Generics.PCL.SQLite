using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace FairyZeta.Generics.PCL.SQLite
{
    /// <summary> FZ.GN／PCL.汎用SQLiteマネージャー
    /// <para> => SQLiteのデータ入出力に必要な基本機能を提供します。 </para>
    /// </summary>
    public partial class GenericSQLiteManager : ISQLiteManager
    {
        /*--- Property/Field Definitions ------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> DBへの接続設定
        /// </summary>
        public SQLiteConnection Connection { get; protected set; }

        /// <summary> ログ記録の設定
        /// <para> => <code>true</code>でログの記録を実行、パフォーマンスを気にする場合は<code>false</code> </para>
        /// </summary>
        public bool IsRecordLog { get; set; }

        /// <summary> このマネージャーが対象としているファイル情報
        /// </summary>
        public IFile DbFile { get; protected set; }

        /// <summary> コネクション再生成の自動実行設定
        /// </summary>
        public bool IsAutoRecreateConnection { get; set; }

        /// <summary> SQLite内テーブルリスト
        /// </summary>
        protected List<Type> TableList { get; set; }

        /// <summary> SQLite操作記録
        /// </summary>
        protected List<SQLiteOperationLog> LogList { get; set; }

        /*--- Constructers --------------------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> FZ.GN／PCL.汎用SQLiteマネージャー／コンストラクタ
        /// </summary>
        /// <param name="pIsRecordLog"> 操作ログをリストに記録する場合<code>true</code>, パフォーマンスを重視する場合は<code>false</code> (デフォルト) </param>
        /// <param name="pAutoRecreateConnection"> コネクション再生成の自動実行を行い場合<code>true</code>, しない場合は<code>false</code> </param>
        public GenericSQLiteManager(bool pIsRecordLog = false, bool pAutoRecreateConnection = true)
        {
            this.LogList = new List<SQLiteOperationLog>();
            this.TableList = new List<Type>();

            this.IsRecordLog = pIsRecordLog;
            this.IsAutoRecreateConnection = pAutoRecreateConnection;
        }

        /*--- Method: Initialization ----------------------------------------------------------------------------------------------------------------------------------*/

        /*--- Method: public ------------------------------------------------------------------------------------------------------------------------------------------*/

        #region --- Util Process ---

        #region Method: GetEntryTableType - このSQLiteマネージャーが扱っているテーブルの型をコピーして返却します。

        /// <summary> このSQLiteマネージャーが扱っているテーブルの型をコピーして返却します。
        /// </summary>
        /// <returns> <code>Type</code>リスト </returns>
        public virtual async Task<List<Type>> GetEntryTableTypeAsync()
        {
            return await Task.Run(() => this.GetEntryTableType());
        }

        /// <summary> このSQLiteマネージャーが扱っているテーブルの型をコピーして返却します。
        /// </summary>
        /// <returns> <code>Type</code>リスト </returns>
        public virtual List<Type> GetEntryTableType()
        {
            return new List<Type>(this.TableList);
        }

        #endregion

        #region Method: GetLogs - 操作記録のコピーを返却します。

        /// <summary> 操作記録のコピーを返却します。
        /// </summary>
        /// <returns> <code>SQLiteOperationLog</code>リスト </returns>
        public virtual async Task<List<SQLiteOperationLog>> GetLogsAsync()
        {
            return await Task.Run(() => this.GetLogs());
        }

        /// <summary> 操作記録のコピーを返却します。
        /// </summary>
        /// <returns> <code>SQLiteOperationLog</code>リスト </returns>
        public virtual List<SQLiteOperationLog> GetLogs()
        {
            return new List<SQLiteOperationLog>(this.LogList.OrderBy(log => log.LogTime));
        }

        #endregion

        #region Method: ClearLogs - 操作記録を初期化します。

        /// <summary> 操作記録を初期化します。
        /// </summary>
        /// <returns> 正常に初期化完了した場合<code>True</code>, それ以外は<code>False</code> </returns>
        public virtual async Task<bool> ClearLogsAsync()
        {
            return await Task.Run(() => this.ClearLogs());
        }

        /// <summary> 操作記録を初期化します。
        /// </summary>
        /// <returns> 正常に初期化完了した場合<code>True</code>, それ以外は<code>False</code> </returns>
        public virtual bool ClearLogs()
        {
            this.LogList.Clear();

            return true;
        }

        #endregion

        #endregion

        #region --- DataBase Process ---

        /// <summary> DBファイル生成とコネクション生成を実行します。
        /// </summary>
        /// <param name="pDbFolderPath"> カレントフォルダからDBフォルダへのパス </param>
        /// <param name="pDbFileName"> DBファイルの名称 </param>
        /// <returns> 操作結果 </returns>
        public virtual async Task<List<SQLiteOperationLog>> CreateDbFileAndConnectionAsync(string pDbFolderPath, string pDbFileName)
        {
            // フォルダを生成する
            List<SQLiteOperationLog> results = await this.CreateDbFileAsync(pDbFolderPath, pDbFileName);
            // コネクションを作成する
            if (this.DbFile != null)
            {
                results.Add(await this.CreateConnectionAsync(this.DbFile.Path));
            }
            else
            {
                results.Add(await this.CreateLogAsync(SQLiteOperationResult.Warning, "<Warning> DbFile Info Null."));
            }

            await this.AddLogAsync(results);

            return results;
        }

        /// <summary> このマネージャーが管理しているDBファイルとコネクションを削除します。
        /// </summary>
        /// <returns> 操作結果 </returns>
        public virtual async Task<List<SQLiteOperationLog>> DeleteDbFileAndConnectionAsync()
        {
            var results = new List<SQLiteOperationLog>();

            results.Add(await this.DeleteConnectionAsync());
            results.Add(await this.DeleteDbFileAsync());

            this.TableList.Clear();

            await this.AddLogAsync(results);

            return results;
        }

        /// <summary> DBファイル内に新しいテーブルを追加します。
        /// </summary>
        /// <param name="pAddTableTypes"> 追加するテーブルの型列挙 </param>
        /// <param name="pConnectionCheck"> テーブル操作前にコネクションを確認する場合<code>True</code> </param>
        /// <returns> 操作結果 </returns>
        public virtual async Task<List<SQLiteOperationLog>> AddTableAsync(IEnumerable<Type> pAddTableTypes, bool pConnectionCheck = true)
        {
            List<SQLiteOperationLog> results = new List<SQLiteOperationLog>();

            if (pConnectionCheck && !await this.CheckSQLiteConnectionAsync())
            {
                throw new NullReferenceException("SQLite Connection Check NG.");
            }
            else
            {
                foreach (var item in pAddTableTypes)
                {
                    var addResult = await this.AddTableAsync(item, false);
                    foreach (var result in addResult)
                        results.Add(result);
                }
            }

            return results;
        }

        /// <summary> DBファイル内に新しいテーブルを追加します。
        /// </summary>
        /// <param name="pAddTableType"> 追加するテーブルの型 </param>
        /// <param name="pConnectionCheck"> テーブル操作前にコネクションを確認する場合<code>True</code> </param>
        /// <returns> 操作結果 </returns>
        public virtual async Task<List<SQLiteOperationLog>> AddTableAsync(Type pAddTableType, bool pConnectionCheck = true)
        {
            List<SQLiteOperationLog> results = new List<SQLiteOperationLog>();

            if (pConnectionCheck && !await this.CheckSQLiteConnectionAsync())
            {
                throw new NullReferenceException("SQLite Connection Check NG.");
            }
            else
            {
                await Task.Run(() =>
                {
                    results.Add(this.AddTableType(pAddTableType));

                    if (results[0].SQLiteOperationResult != SQLiteOperationResult.Exists)
                    {
                        var createResult = this.Connection.CreateTable(pAddTableType);
                        results.Add(this.CreateLog(SQLiteOperationResult.Success, string.Format("{0} Table Created.", pAddTableType.Name.ToString())));
                    }

                });
            }

            await this.AddLogAsync(results);

            return results;
        }

        /// <summary> DBファイル内のテーブルを削除します。
        /// </summary>
        /// <typeparam name="T"> 削除するテーブル型 </typeparam>
        /// <param name="pConnectionCheck"> テーブル操作前にコネクションを確認する場合<code>True</code> </param>
        /// <returns> 操作結果 </returns>
        public virtual async Task<List<SQLiteOperationLog>> DeleteTableAsync<T>(bool pConnectionCheck = true) where T : new()
        {
            List<SQLiteOperationLog> results = new List<SQLiteOperationLog>();

            if (pConnectionCheck && !await this.CheckSQLiteConnectionAsync())
            {
                throw new NullReferenceException("SQLite Connection Check NG.");
            }
            else
            {
                await Task.Run(() =>
                {
                    results.Add(this.DeleteTableType(typeof(T)));

                    // 削除の場合、使用テーブル登録されていない可能性がある為、強制的に削除を実行する
                    var dropResult = this.Connection.DropTable<T>();
                    results.Add(this.CreateLog(SQLiteOperationResult.Success, string.Format("{0} Table Deleted.", typeof(T).Name.ToString())));
                    
                });
            }

            await this.AddLogAsync(results);

            return results;
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
            var results = new List<T>(Connection.Table<T>().ToList());

            if (this.IsRecordLog)
            {
                this.AddLog(this.CreateLog(SQLiteOperationResult.Success, string.Format("{0} => Get {1} Entitys.", typeof(T).Name, results.Count())));
            }

            return results;
        }

        /// <summary> [IF.IDbEntityManager] エンティティの追加または更新を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 更新するエンティティ列挙 </param>
        /// <returns> 処理結果コードリスト </returns>
        public virtual async Task<List<int>> InsertOrUpdateEntityAsync<T>(IEnumerable<T> pEntitys) where T : new()
        {
            List<int> resultCodes = new List<int>();
            
            foreach (var entity in pEntitys)
                resultCodes.Add(await this.InsertOrUpdateEntityAsync<T>(entity));
            
            return resultCodes;
        }

        /// <summary> [IF.IDbEntityManager] エンティティの追加または更新を実行します。
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
            int resultCode = 0;

            if (this.Connection.Table<T>().Any(e => e.Equals(pEntity)))
            {
                resultCode = this.Connection.Update(pEntity);
                if (this.IsRecordLog)
                    this.AddLog(this.CreateLog(SQLiteOperationResult.Success, string.Format("{0} => Updated Entity, Result Code = {1})", typeof(T).Name, resultCode)));
            }
            else
            {
                resultCode = this.Connection.Insert(pEntity);
                if (this.IsRecordLog)
                    this.AddLog(this.CreateLog(SQLiteOperationResult.Success, string.Format("{0} => Inserted Entity, Result Code = {1})", typeof(T).Name, resultCode)));
            }

            return resultCode;
        }

        /// <summary> [IF.IDbEntityManager] エンティティの削除を実行します。
        /// </summary>
        /// <typeparam name="T"> エンティティの型 </typeparam>
        /// <param name="pEntity"> 削除するエンティティ列挙 </param>
        /// <returns> 処理結果コードリスト </returns>
        public virtual async Task<List<int>> DeleteEntityAsync<T>(IEnumerable<T> pEntitys) where T : new()
        {
            List<int> resultCodes = new List<int>();
            
            foreach (var entity in pEntitys)
                resultCodes.Add(await this.DeleteEntityAsync<T>(entity));
           
            return resultCodes;
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
            int resultCode = this.Connection.Delete(pEntity);
            
            if (this.IsRecordLog)
                this.AddLog(this.CreateLog(SQLiteOperationResult.Success, string.Format("{0} => Deleted Entity, Result Code = {1})", typeof(T).Name, resultCode)));

            return resultCode;

        }

        #endregion

        /*--- Method: protected ---------------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> DBファイル生成を実行します。
        /// </summary>
        /// <param name="pDbFolderPath"> カレントフォルダからDBフォルダへのパス </param>
        /// <param name="pDbFileName"> DBファイルの名称 </param>
        /// <returns> 操作結果 </returns>
        protected virtual async Task<List<SQLiteOperationLog>> CreateDbFileAsync(string pDbFolderPath, string pDbFileName)
        {
            List<SQLiteOperationLog> results = new List<SQLiteOperationLog>();

            // パラメータチェック
            if (string.IsNullOrEmpty(pDbFileName))
            {
                results.Add(this.CreateLog(SQLiteOperationResult.Failure, "<Param Error> DbFileName Empty."));
                // Todo: SQLite => 続行が危険な気がするから、ここ例外投げちゃおうかな…
            }
            else
            {
                // ルートフォルダを取得する
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                // フォルダチェック
                if (!string.IsNullOrWhiteSpace(pDbFolderPath) && await rootFolder.CheckExistsAsync(pDbFolderPath) == ExistenceCheckResult.NotFound)
                {
                    await rootFolder.CreateFolderAsync(pDbFolderPath, CreationCollisionOption.FailIfExists);
                    results.Add(await this.CreateLogAsync(SQLiteOperationResult.Success, string.Format("Create New Folder Success. => {0}", Path.Combine(rootFolder.Path, pDbFolderPath))));
                }

                // DBファイル存在チェック
                var dbFullPath = Path.Combine(pDbFolderPath, pDbFileName);
                if (await rootFolder.CheckExistsAsync(dbFullPath) == ExistenceCheckResult.NotFound)
                {
                    // 存在しなかった場合、新たにDBファイルを作成
                    this.DbFile = await rootFolder.CreateFileAsync(dbFullPath, CreationCollisionOption.ReplaceExisting);
                    results.Add(await this.CreateLogAsync(SQLiteOperationResult.Success, string.Format("Create New {0} Success. => {1}", pDbFileName, Path.Combine(rootFolder.Path, dbFullPath))));
                }
                else
                {
                    // 存在した場合、ファイルオープン
                    this.DbFile = await rootFolder.CreateFileAsync(dbFullPath, CreationCollisionOption.OpenIfExists);
                    results.Add(await this.CreateLogAsync(SQLiteOperationResult.Success, string.Format("{0} Open Success. => {1}", pDbFileName, Path.Combine(rootFolder.Path, dbFullPath))));
                }
            }

            return results;
        }

        /// <summary> コネクション生成を実行します。
        /// </summary>
        /// <param name="pDbFilePath"> DBファイルへのパス </param>
        /// <returns> 操作結果 </returns>
        protected virtual async Task<SQLiteOperationLog> CreateConnectionAsync(string pDbFilePath)
        {
            if (string.IsNullOrWhiteSpace(pDbFilePath))
                return await this.CreateLogAsync(SQLiteOperationResult.Failure, "<Create Connection Failure> DbFilePath Empty.");

            this.Connection = new SQLiteConnection(pDbFilePath);
            return await this.CreateLogAsync(SQLiteOperationResult.Success, "Create Connection Success. ");

            // Todo: DateTime型を正しく扱うためには、以下のようにコネクションを生成する必要がある。 ここでは WinRT 指定不可能の為、overrideを検討する事
            // this.Connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), pDbFilePath, false);
            // 参考 => http://stackoverflow.com/questions/39076337/how-sqlite-net-pcl-insert-datetime-into-sqlite
        }

        /// <summary> 自身のテーブルリストにエンティティタイプを追加します。
        /// </summary>
        /// <param name="pAddTableTypes"> 追加するテーブルタイプ </param>
        /// <returns> 追加結果 </returns>
        protected virtual SQLiteOperationLog AddTableType(Type pAddEntityType)
        {
            if (this.TableList.Any(o => o == pAddEntityType))
                return this.CreateLog(SQLiteOperationResult.Exists, string.Format("{0} => Entity Exists.", pAddEntityType.Name.ToString()));

            this.TableList.Add(pAddEntityType);

            return CreateLog(SQLiteOperationResult.Success, string.Format("{0} => Add TableList.", pAddEntityType.Name.ToString()));
        }

        /// <summary> 自身のテーブルリストからエンティティタイプを削除します。
        /// </summary>
        /// <param name="pAddTableTypes"> 削除するテーブルタイプ </param>
        /// <returns> 追加結果 </returns>
        protected virtual SQLiteOperationLog DeleteTableType(Type pAddEntityType)
        {
            if (this.TableList.Any(o => o == pAddEntityType))
            {
                this.TableList.Remove(pAddEntityType);
                return CreateLog(SQLiteOperationResult.Success, string.Format("{0} => Delete TableList.", pAddEntityType.Name.ToString()));
            }
            else
            {
                return this.CreateLog(SQLiteOperationResult.NotFound, string.Format("{0} => Table Entry Not Exists.", pAddEntityType.Name.ToString()));
            }

        }

        /// <summary> コネクションの状態をチェックし、bool値を返却します。
        /// </summary>
        /// <returns> コネクションが正常な場合は<code>True</code>, それ以外は<code>False</code> </returns>
        protected virtual async Task<bool> CheckSQLiteConnectionAsync()
        {
            if (this.Connection == null)
            {
                this.AddLog(this.CreateLog(SQLiteOperationResult.Failure, "<Connection NG> Connection Null."));

                if (this.IsAutoRecreateConnection && this.DbFile != null)
                {
                    var recreateConnectionResult = await this.CreateConnectionAsync(this.DbFile.Path);
                    await this.AddLogAsync(recreateConnectionResult);

                    if (recreateConnectionResult.SQLiteOperationResult == SQLiteOperationResult.Success)
                    {
                        await this.AddLogAsync(await this.CreateLogAsync(SQLiteOperationResult.Success, "Connection Auto Recreated OK."));
                        return true;
                    }
                }

                return false;
                
            }

            this.AddLog(this.CreateLog(SQLiteOperationResult.Success, "<Connection OK> Condition Green."));

            return true;
        }

        /// <summary> DBファイルの削除を実行します。
        /// </summary>
        /// <returns> 操作結果 </returns>
        protected virtual async Task<SQLiteOperationLog> DeleteDbFileAsync()
        {
            if (this.DbFile == null)
            {
                return this.CreateLog(SQLiteOperationResult.Failure, "DbFile is Null.");
            }
            else
            {
                string fName = DbFile.Name;
                string fPath = DbFile.Path;

                await this.DbFile.DeleteAsync();
                this.DbFile = null;

                return this.CreateLog(SQLiteOperationResult.Success, string.Format("{0} File Deleted. => {1}", fName, fPath));
            }
        }

        /// <summary> コネクションをクローズし、削除を実行します。
        /// </summary>
        /// <returns> 操作結果 </returns>
        protected virtual async Task<SQLiteOperationLog> DeleteConnectionAsync()
        {
            if (this.Connection == null)
            {
                return this.CreateLog(SQLiteOperationResult.Failure, "SQLite Connection is Null.");
            }
            else
            {
                await Task.Run(() =>
                    {
                        this.Connection.Close();
                        this.Connection = null;
                });

                return this.CreateLog(SQLiteOperationResult.Success, "SQLite Connection Clode and Deleted.");
            }
        }

        /// <summary> 操作記録を生成します。
        /// </summary>
        /// <param name="pSQLiteOperationResult"> SQLiteの操作結果 </param>
        /// <param name="pLogMsg"> 出力メッセージ </param>
        /// <returns> 操作記録 </returns>
        protected virtual async Task<SQLiteOperationLog> CreateLogAsync(SQLiteOperationResult pSQLiteOperationResult, string pLogMsg)
        {
            return await Task.Run(() => this.CreateLog(pSQLiteOperationResult, pLogMsg));
        }

        /// <summary> 操作記録を生成します。
        /// </summary>
        /// <param name="pSQLiteOperationResult"> SQLiteの操作結果 </param>
        /// <param name="pLogMsg"> 出力メッセージ </param>
        /// <returns> 操作記録 </returns>
        protected virtual SQLiteOperationLog CreateLog(SQLiteOperationResult pSQLiteOperationResult, string pLogMsg)
        {
            return new SQLiteOperationLog() {
                SQLiteOperationResult = pSQLiteOperationResult,
                LogMsg = pLogMsg
            };
        }

        /// <summary> 操作記録を追加します。
        /// </summary>
        /// <param name="pSQLiteOperationLogs"> 追加する操作記録の列挙 </param>
        /// <returns> 追加に成功した場合<code>true</code>, それ以外は<code>false</code> </returns>
        public virtual async Task<List<bool>> AddLogAsync(IEnumerable<SQLiteOperationLog> pSQLiteOperationLogs)
        {
            List<bool> results = new List<bool>();

            foreach (var log in pSQLiteOperationLogs)
            {
                await Task.Run(() => results.Add(this.AddLog(log)));
            }

            return results;
        }

        /// <summary> 操作記録を追加します。
        /// </summary>
        /// <param name="pSQLiteOperationLogs"> 追加する操作記録の列挙 </param>
        /// <returns> 追加に成功した場合<code>true</code>, それ以外は<code>false</code> </returns>
        protected virtual List<bool> AddLog(IEnumerable<SQLiteOperationLog> pSQLiteOperationLogs)
        {
            List<bool> results = new List<bool>();

            foreach (var log in pSQLiteOperationLogs)
            {
                results.Add(this.AddLog(log));
            }

            return results;
        }

        /// <summary> 操作記録を追加します。
        /// </summary>
        /// <param name="pSQLiteOperationLogs"> 追加する操作記録 </param>
        /// <returns> 追加に成功した場合<code>true</code>, それ以外は<code>false</code> </returns>
        protected virtual async Task<bool> AddLogAsync(SQLiteOperationLog pSQLiteOperationLog)
        {
            return await Task.Run(() => this.AddLog(pSQLiteOperationLog));
        }

        /// <summary> 操作記録を追加します。
        /// </summary>
        /// <param name="pSQLiteOperationLogs"> 追加する操作記録 </param>
        /// <returns> 追加に成功した場合<code>true</code>, それ以外は<code>false</code> </returns>
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
