using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

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

        [TestMethod]
        public async Task TDbProcess_S0001()
        {
            var result = base.CreateManager(new List<Type>() { base.GetTestEntityType() });

            await result.CreateDbFileAndConnectionAsync("Test","TestDb.db3");

            base.OutputLogs(result);
        }
        

        /*--- Method: protected ---------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: internal ----------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: private -----------------------------------------------------------------------------------------------------------------------------------------*/

    }
}
