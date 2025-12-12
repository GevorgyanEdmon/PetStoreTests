using System;
using System.Collections.Generic;
using System.Text;

namespace PetStoreTests
{
    [TestClass]
    public class DatabaseTests
    {
        // Запускается один раз перед всеми тестами в этом классе
        [ClassInitialize]
        public static void ClassSetup(TestContext context)
        {
            // 1. Создаем чистую базу
            DbHelper.InitializeDb();
        }

        [TestMethod]
        public void TestDatabaseInsertAndSelect()
        {
            // 2. Вставляем данные (INSERT)
            DbHelper.AddUser(1, "Edmon", "active");

            // 3. Читаем данные (SELECT)
            string statusFromDb = DbHelper.GetUserStatus("Edmon");

            // 4. Проверяем
            Assert.AreEqual("active", statusFromDb);
        }
    }
}
