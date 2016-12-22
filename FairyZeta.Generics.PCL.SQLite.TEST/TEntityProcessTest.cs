using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using FairyZeta.Generics.PCL.SQLite.TEST.Entity;
using System.Diagnostics;

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

        /// <summary> エンティティ操作テスト
        /// <para> => 1件のエンティティを3テーブルに登録 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TEntityProcess_S0001_Async()
        {
            // DB準備
            GenericSQLiteManager sqliteManager = new GenericSQLiteManager(true);    // ログ出力するため true
            await sqliteManager.CreateDbFileAndConnectionAsync("SQLite_EntityProcessTest", "TestDb_S0001.db3");
            await sqliteManager.AddTableAsync(new List<Type>() { typeof(PersonEntity), typeof(TodoEntity), typeof(UserEntity) });

            // テスト開始
            await sqliteManager.InsertOrUpdateEntityAsync<PersonEntity>(new PersonEntity() { Name = "Taro", Age = 20 });
            await sqliteManager.InsertOrUpdateEntityAsync<TodoEntity>(new TodoEntity() { Detail = "TestTodo1", CreateDate = DateTime.Now});
            await sqliteManager.InsertOrUpdateEntityAsync<UserEntity>(new UserEntity() { UserID = "FairyZeta1", Pass = "Password", Mail = @"info@fairyzeta.com"});
            
            // テスト結果出力
            base.OutputLogs(sqliteManager);
        }

        /// <summary> エンティティ操作テスト
        /// <para> => 3件のエンティティを3テーブルに登録 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TEntityProcess_S0002_Async()
        {
            // DB準備
            var sqliteManager = base.CreateManager();
            await sqliteManager.CreateDbFileAndConnectionAsync("SQLite_EntityProcessTest", "TestDb_S0002.db3");
            await sqliteManager.AddTableAsync(base.GetTestEntityTypes());

            // テスト開始
            List<PersonEntity> personList = new List<PersonEntity>()
            {
                new PersonEntity() { Name = "Taro", Age = 25 },
                new PersonEntity() { Name = "次郎", Age = 20 },
                new PersonEntity() { Name = "saburo", Age = 15 }
            };

            List<TodoEntity> todoList = new List<TodoEntity>()
            {
                new TodoEntity() { Detail = "TestTodo1", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "11112221142", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "日本語も入れてみる", CreateDate = DateTime.Now }
            };

            List<UserEntity> userList = new List<UserEntity>()
            {
                new UserEntity() { UserID = "FairyZeta1", Pass = "Password", Mail = @"info@fairyzeta.com" },
                new UserEntity() { UserID = "FZ", Pass = "PPPPPP", Mail = @"aaaa@fairyzeta.com" },
                new UserEntity() { UserID = "fairyzeta.com", Pass = "Hoooooo", Mail = @"win@fairyzeta.com" }
            };

            await sqliteManager.InsertOrUpdateEntityAsync<PersonEntity>(personList);
            await sqliteManager.InsertOrUpdateEntityAsync<TodoEntity>(todoList);
            await sqliteManager.InsertOrUpdateEntityAsync<UserEntity>(userList);

            // テスト結果出力
            base.OutputLogs(sqliteManager);
        }

        /// <summary> エンティティ操作テスト
        /// <para> => 3件のエンティティを3テーブルに登録後、エンティティ取得 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TEntityProcess_S0011_Async()
        {
            // DB準備
            var sqliteManager = base.CreateManager();
            await sqliteManager.CreateDbFileAndConnectionAsync("SQLite_EntityProcessTest", "TestDb_S0011.db3");
            await sqliteManager.AddTableAsync(base.GetTestEntityTypes());

            // テスト開始
            List<PersonEntity> personList = new List<PersonEntity>()
            {
                new PersonEntity() { Name = "Taro", Age = 25 },
                new PersonEntity() { Name = "次郎", Age = 20 },
                new PersonEntity() { Name = "saburo", Age = 15 }
            };

            List<TodoEntity> todoList = new List<TodoEntity>()
            {
                new TodoEntity() { Detail = "TestTodo1", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "11112221142", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "日本語も入れてみる", CreateDate = DateTime.Now }
            };

            List<UserEntity> userList = new List<UserEntity>()
            {
                new UserEntity() { UserID = "FairyZeta1", Pass = "Password", Mail = @"info@fairyzeta.com" },
                new UserEntity() { UserID = "FZ", Pass = "PPPPPP", Mail = @"aaaa@fairyzeta.com" },
                new UserEntity() { UserID = "fairyzeta.com", Pass = "Hoooooo", Mail = @"win@fairyzeta.com" }
            };

            await sqliteManager.InsertOrUpdateEntityAsync<PersonEntity>(personList);
            await sqliteManager.InsertOrUpdateEntityAsync<TodoEntity>(todoList);
            await sqliteManager.InsertOrUpdateEntityAsync<UserEntity>(userList);

            var persons = await sqliteManager.GetEntitysAsync<PersonEntity>();
            var todos = await sqliteManager.GetEntitysAsync<TodoEntity>();
            var users = await sqliteManager.GetEntitysAsync<UserEntity>();

            // テスト結果出力
            base.OutputLogs(sqliteManager);

            foreach (var person in persons)
                Debug.WriteLine(string.Format("<TestMethod> Get Entity Person: Name = {0}", person.Name));


            foreach (var todo in todos)
                Debug.WriteLine(string.Format("<TestMethod> Get Entity Todo: Detail = {0}", todo.Detail));

            foreach (var user in users)
                Debug.WriteLine(string.Format("<TestMethod> Get Entity User: UserID = {0}", user.UserID));
        }


        /// <summary> エンティティ操作テスト
        /// <para> => 3件のエンティティを3テーブルに登録後、１つずつエンティティ削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TEntityProcess_S0021_Async()
        {
            // DB準備
            var sqliteManager = base.CreateManager();
            await sqliteManager.CreateDbFileAndConnectionAsync("SQLite_EntityProcessTest", "TestDb_S0021.db3");
            await sqliteManager.AddTableAsync(base.GetTestEntityTypes());

            // テスト開始
            List<PersonEntity> personList = new List<PersonEntity>()
            {
                new PersonEntity() { Name = "Taro", Age = 25 },
                new PersonEntity() { Name = "次郎", Age = 20 },
                new PersonEntity() { Name = "saburo", Age = 15 }
            };

            List<TodoEntity> todoList = new List<TodoEntity>()
            {
                new TodoEntity() { Detail = "TestTodo1", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "11112221142", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "日本語も入れてみる", CreateDate = DateTime.Now }
            };

            List<UserEntity> userList = new List<UserEntity>()
            {
                new UserEntity() { UserID = "FairyZeta1", Pass = "Password", Mail = @"info@fairyzeta.com" },
                new UserEntity() { UserID = "FZ", Pass = "PPPPPP", Mail = @"aaaa@fairyzeta.com" },
                new UserEntity() { UserID = "fairyzeta.com", Pass = "Hoooooo", Mail = @"win@fairyzeta.com" }
            };

            await sqliteManager.InsertOrUpdateEntityAsync<PersonEntity>(personList);
            await sqliteManager.InsertOrUpdateEntityAsync<TodoEntity>(todoList);
            await sqliteManager.InsertOrUpdateEntityAsync<UserEntity>(userList);

            await sqliteManager.DeleteEntityAsync<PersonEntity>(personList[0]);
            await sqliteManager.DeleteEntityAsync<TodoEntity>(todoList[0]);
            await sqliteManager.DeleteEntityAsync<UserEntity>(userList[0]);

            // テスト結果出力
            base.OutputLogs(sqliteManager);
        }

        /// <summary> エンティティ操作テスト
        /// <para> => 3件のエンティティを3テーブルに登録後、存在しないエンティティ削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TEntityProcess_S0022_Async()
        {
            // DB準備
            var sqliteManager = base.CreateManager();
            await sqliteManager.CreateDbFileAndConnectionAsync("SQLite_EntityProcessTest", "TestDb_S0022.db3");
            await sqliteManager.AddTableAsync(base.GetTestEntityTypes());

            // テスト開始
            List<PersonEntity> personList = new List<PersonEntity>()
            {
                new PersonEntity() { Name = "Taro", Age = 25 },
                new PersonEntity() { Name = "次郎", Age = 20 },
                new PersonEntity() { Name = "saburo", Age = 15 }
            };

            List<TodoEntity> todoList = new List<TodoEntity>()
            {
                new TodoEntity() { Detail = "TestTodo1", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "11112221142", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "日本語も入れてみる", CreateDate = DateTime.Now }
            };

            List<UserEntity> userList = new List<UserEntity>()
            {
                new UserEntity() { UserID = "FairyZeta1", Pass = "Password", Mail = @"info@fairyzeta.com" },
                new UserEntity() { UserID = "FZ", Pass = "PPPPPP", Mail = @"aaaa@fairyzeta.com" },
                new UserEntity() { UserID = "fairyzeta.com", Pass = "Hoooooo", Mail = @"win@fairyzeta.com" }
            };

            await sqliteManager.InsertOrUpdateEntityAsync<PersonEntity>(personList);
            await sqliteManager.InsertOrUpdateEntityAsync<TodoEntity>(todoList);
            await sqliteManager.InsertOrUpdateEntityAsync<UserEntity>(userList);

            await sqliteManager.DeleteEntityAsync<PersonEntity>(new PersonEntity() { Name = "Name", Age = 10});
            await sqliteManager.DeleteEntityAsync<TodoEntity>(new TodoEntity() { Detail = "Detail" });
            await sqliteManager.DeleteEntityAsync<UserEntity>(new UserEntity() { UserID = "UserID"});

            // テスト結果出力
            base.OutputLogs(sqliteManager);
        }

        /// <summary> エンティティ操作テスト
        /// <para> => 3件のエンティティを3テーブルに登録後、意図的にIDを被らせてエンティティ削除 </para>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TEntityProcess_S0023_Async()
        {
            // DB準備
            var sqliteManager = base.CreateManager();
            await sqliteManager.CreateDbFileAndConnectionAsync("SQLite_EntityProcessTest", "TestDb_S0023.db3");
            await sqliteManager.AddTableAsync(base.GetTestEntityTypes());

            // テスト開始
            List<PersonEntity> personList = new List<PersonEntity>()
            {
                new PersonEntity() { Name = "Taro", Age = 25 },
                new PersonEntity() { Name = "次郎", Age = 20 },
                new PersonEntity() { Name = "saburo", Age = 15 }
            };

            List<TodoEntity> todoList = new List<TodoEntity>()
            {
                new TodoEntity() { Detail = "TestTodo1", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "11112221142", CreateDate = DateTime.Now },
                new TodoEntity() { Detail = "日本語も入れてみる", CreateDate = DateTime.Now }
            };

            List<UserEntity> userList = new List<UserEntity>()
            {
                new UserEntity() { UserID = "FairyZeta1", Pass = "Password", Mail = @"info@fairyzeta.com" },
                new UserEntity() { UserID = "FZ", Pass = "PPPPPP", Mail = @"aaaa@fairyzeta.com" },
                new UserEntity() { UserID = "fairyzeta.com", Pass = "Hoooooo", Mail = @"win@fairyzeta.com" }
            };

            await sqliteManager.InsertOrUpdateEntityAsync<PersonEntity>(personList);
            await sqliteManager.InsertOrUpdateEntityAsync<TodoEntity>(todoList);
            await sqliteManager.InsertOrUpdateEntityAsync<UserEntity>(userList);

            await sqliteManager.DeleteEntityAsync<PersonEntity>(new PersonEntity() { Name = "Name", Age = 10, ID = 1 });
            await sqliteManager.DeleteEntityAsync<TodoEntity>(new TodoEntity() { Detail = "Detail", ID = 1 });
            await sqliteManager.DeleteEntityAsync<UserEntity>(new UserEntity() { UserID = "UserID", ID = 1 });

            // テスト結果出力
            base.OutputLogs(sqliteManager);
        }

        /*--- Method: protected ---------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: internal ----------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: private -----------------------------------------------------------------------------------------------------------------------------------------*/

    }
}
