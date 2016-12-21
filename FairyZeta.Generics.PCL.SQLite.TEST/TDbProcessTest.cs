using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using FairyZeta.Generics.PCL.SQLite.TEST.Entity;

namespace FairyZeta.Generics.PCL.SQLite.TEST
{
    /// <summary> DB操作テスト
    /// <para> => TGenericSQLiteManager DB生成および操作 テスト</para>
    /// </summary>
    [TestClass]
    public class TDbProcessTest : SQLiteTestBase
    {
        /*--- Property/Field Definitions ------------------------------------------------------------------------------------------------------------------------------*/

        private TestContext testContextInstance;

        /*--- Constructers --------------------------------------------------------------------------------------------------------------------------------------------*/

        public TDbProcessTest()
        {
        }

        /*--- Method: Initialization ----------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /*--- Method: public ------------------------------------------------------------------------------------------------------------------------------------------*/

        #region 追加のテスト属性
        //
        // テストを作成する際には、次の追加属性を使用できます:
        //
        // クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region --- S0001 ~ S0010 ファイル操作テスト ---
        
        /// <summary> コネクション生成テスト
        /// <para> => 正常系 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0001_Async()
        {
            var result = base.CreateManager();

            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0001.db3");

            base.OutputLogs(result);
        }

        /// <summary> コネクション生成テスト
        /// <para> => フォルダ名なし </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0002_Async()
        {
            var result = base.CreateManager();

            await result.CreateDbFileAndConnectionAsync(string.Empty, "TestDb_S0002.db3");

            base.OutputLogs(result);
        }

        /// <summary> コネクション生成テスト
        /// <para> => ファイル名なし </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0003_Async()
        {
            var result = base.CreateManager();

            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", string.Empty);

            base.OutputLogs(result);
        }

        /// <summary> コネクションテスト
        /// <para> => ファイルとコネクションを生成して、削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0004_Async()
        {
            var result = base.CreateManager();

            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0004.db3");

            await result.DeleteDbFileAndConnectionAsync();

            base.OutputLogs(result);
        }

        /// <summary> コネクションテスト
        /// <para> => なにも生成せず、削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0005_Async()
        {
            var result = base.CreateManager();
            
            await result.DeleteDbFileAndConnectionAsync();

            base.OutputLogs(result);
        }

        #endregion

        #region --- S0011 ~ S0020 テーブル追加テスト ---

        /// <summary> DB操作テスト
        /// <para> => テーブル１つ追加 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0011_Async()
        {
            var result = base.CreateManager();
            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0011.db3");

            await result.AddTableAsync(base.GetTestEntityType());

            base.OutputLogs(result);
        }

        /// <summary> DB操作テスト
        /// <para> => テーブル３つ追加 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0012_Async()
        {
            var result = base.CreateManager();
            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0012.db3");

            await result.AddTableAsync(base.GetTestEntityTypes());

            base.OutputLogs(result);
        }

        /// <summary> DB操作テスト
        /// <para> => テーブル１つを２回追加 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0013_Async()
        {
            var result = base.CreateManager();
            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0013.db3");

            await result.AddTableAsync(base.GetTestEntityType());
            await result.AddTableAsync(base.GetTestEntityType());

            base.OutputLogs(result);
        }

        #endregion

        #region --- S0021 ~ S0030 テーブル削除テスト ---

        /// <summary> DB操作テスト
        /// <para> => テーブル１つを追加して削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0021_Async()
        {
            var result = base.CreateManager();
            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0021.db3");

            await result.AddTableAsync(base.GetTestEntityType());
            await result.DeleteTableAsync<PersonEntity>();

            base.OutputLogs(result);
        }

        /// <summary> DB操作テスト
        /// <para> => テーブル１つを追加して２回削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0022_Async()
        {
            var result = base.CreateManager();
            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0022.db3");

            await result.AddTableAsync(base.GetTestEntityType());
            await result.DeleteTableAsync<PersonEntity>();
            await result.DeleteTableAsync<PersonEntity>();

            base.OutputLogs(result);
        }

        /// <summary> DB操作テスト
        /// <para> => テーブル３つを追加して、そのうち１つを削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0023_Async()
        {
            var result = base.CreateManager();
            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0023.db3");

            await result.AddTableAsync(base.GetTestEntityTypes());
            await result.DeleteTableAsync<TodoEntity>();

            base.OutputLogs(result);
        }

        /// <summary> DB操作テスト
        /// <para> => テーブル３つを追加して、そのうち１つを２回削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0024_Async()
        {
            var result = base.CreateManager();
            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0024.db3");

            await result.AddTableAsync(base.GetTestEntityTypes());
            await result.DeleteTableAsync<UserEntity>();
            await result.DeleteTableAsync<UserEntity>();

            base.OutputLogs(result);
        }

        /// <summary> DB操作テスト
        /// <para> => テーブル１つを追加して、存在しないテーブルを削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_S0025_Async()
        {
            var result = base.CreateManager();
            await result.CreateDbFileAndConnectionAsync("SQLite_DbProcessTest", "TestDb_S0025.db3");

            await result.AddTableAsync(base.GetTestEntityType());
            await result.DeleteTableAsync<UserEntity>();

            base.OutputLogs(result);
        }

        #endregion

        #region --- F0001 ~ 異常系テスト ---


        /// <summary> DB操作テスト
        /// <para> => コネクション生成しないで、テーブル１つ追加 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_F0011_Async()
        {
            var result = base.CreateManager();

            try
            {
                await result.AddTableAsync(base.GetTestEntityType());
            }
            catch
            {
                base.OutputLogs(result);
            }
        }

        /// <summary> DB操作テスト
        /// <para> => コネクション生成しないで、テーブル３つ追加 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_F0012_Async()
        {
            var result = base.CreateManager();

            try
            {
                await result.AddTableAsync(base.GetTestEntityTypes());
            }
            catch
            {
                base.OutputLogs(result);
            }
        }

        /// <summary> DB操作テスト
        /// <para> => コネクションを生成しないで、テーブルを削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TDbProcess_F0021_Async()
        {
            var result = base.CreateManager();

            try
            {
                await result.DeleteTableAsync<PersonEntity>();
            }
            catch
            {
                base.OutputLogs(result);
            }
        }

        #endregion

        /*--- Method: protected ---------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: internal ----------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: private -----------------------------------------------------------------------------------------------------------------------------------------*/

    }
}
