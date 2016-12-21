using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FairyZeta.Generics.PCL.SQLite.TEST
{
    /// <summary> エンティティ操作テスト
    /// <para> => TGenericSQLiteManager エンティティ操作 テスト</para>
    /// </summary>
    [TestClass]
    public class TEntityProcessTest : SQLiteTestBase
    {
        /*--- Property/Field Definitions ------------------------------------------------------------------------------------------------------------------------------*/

        private TestContext testContextInstance;

        /*--- Constructers --------------------------------------------------------------------------------------------------------------------------------------------*/

        public TEntityProcessTest()
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
        public void TEntityProcess_S0001()
        {

        }

        #region --- DataBase Process ---

        #endregion

        #region --- Entity Process ---


        #endregion

        /*--- Method: protected ---------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: internal ----------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: private -----------------------------------------------------------------------------------------------------------------------------------------*/

    }
}
