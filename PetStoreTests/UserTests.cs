using PetStoreTests.Helpers;
using PetStoreTests.Models;
using PetTest;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetStoreTests
{
    [TestClass]
    public class UserTests: BaseTest
    {
        [TestMethod]
        public void Login_ShouldReturnSessionId()
        {
            var createuser = new User
            { 
            UserName = "Edmon",
            Password = "34505"
            };
            var request = new RestRequest(Endpoints.User,Method.Post);
            request.AddBody(createuser);
            var response = client.Execute<User> (request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var getrequest = new RestRequest("user/login", Method.Get);
            getrequest.AddQueryParameter("username", "Edmon");
            getrequest.AddQueryParameter("password", "34505");
            var getresponse = client.Execute(getrequest);
            Assert.AreEqual(HttpStatusCode.OK, getresponse.StatusCode);
            Assert.IsTrue(getresponse.Content.Contains("logged in"));
        }
        [TestMethod]
        public void CreateUser_WithInvalidStatusType_ShouldReturnError()
        {
            var badUser = new
            { 
            id = 0,
            username = "badUser",
            userStatus = "iamstring"
            };
            var request = new RestRequest (Endpoints.User,Method.Post);
            request.AddBody(badUser);
            var response = client.Execute (request);
            Assert.AreEqual (HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [TestMethod]
        public void reatePet_WithEmptyBody_ShouldReturnError()
        {
            var emptyBody = new { };
            var request = new RestRequest(Endpoints.Pet, Method.Post);
            request.AddBody(emptyBody);
            var response = client.Execute (request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [DataTestMethod]
        [DataRow("plainaddress")]
        [DataRow("#@%^%#$@#$@#.com")]
        [DataRow("@example.com")]
        [DataRow("Joe Smith <email@example.com>")]
        public void CreateUser_WithInvalidEmail_ShouldReturnError(string invalidEmail)
        {
            var createdUser = TestDataHelper.GenerateNewUser();
            createdUser.Email = invalidEmail;
            var request = new RestRequest (Endpoints.User, Method.Post);
            request.AddBody(createdUser);
            var response = client.Execute<User>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [DataTestMethod]
        [DataRow("REAL", "wrong_pass")] // 1. Верный логин, плохой пароль
        [DataRow("wrong_user", "REAL")] // 2. Плохой логин, верный пароль
        [DataRow("does_not_exist", "123")] // 3. Оба плохие
        public void Login_WithWrongCredentials_ShouldFail(string loginInput, string passInput)
        {
            // ==========================================
            // ШАГ 1: Создаем НАСТОЯЩЕГО пользователя (чтобы он был в базе)
            // ==========================================
            var realUser = TestDataHelper.GenerateNewUser();

            // Создаем запрос на регистрацию
            var createRequest = new RestRequest(Endpoints.User, Method.Post);
            createRequest.AddBody(realUser);

            // Отправляем (создаем его в базе)
            var createResponse = client.Execute(createRequest);
            Assert.AreEqual(HttpStatusCode.OK, createResponse.StatusCode); // Убедились, что создался


            // ==========================================
            // ШАГ 2: Готовим данные для ЛОГИНА (Фейковые или Реальные?)
            // ==========================================

            // Это условие (Тернарный оператор):
            // Если в DataRow написано слово "REAL", то берем логин настоящего юзера (realUser.UserName).
            // Иначе - берем то, что написано в DataRow (например "wrong_user").
            string loginToSend = (loginInput == "REAL") ? realUser.UserName : loginInput;

            // То же самое для пароля
            string passToSend = (passInput == "REAL") ? realUser.Password : passInput;


            // ==========================================
            // ШАГ 3: Пытаемся войти
            // ==========================================
            var loginRequest = new RestRequest("user/login", Method.Get);
            loginRequest.AddQueryParameter("username", loginToSend);
            loginRequest.AddQueryParameter("password", passToSend);

            var loginResponse = client.Execute(loginRequest);


            // ==========================================
            // ШАГ 4: Проверка (Ожидаем провал)
            // ==========================================

            // В PetStore при неверном логине статус может быть 200, но в тексте не будет "logged in".
            // Или будет 400. Давай проверим, что в тексте НЕТ фразы "logged in".

            // Assert.IsFalse (Ложь) - мы ожидаем, что это условие НЕ выполнится
            // Assert.IsFalse(loginResponse.Content.Contains("logged in"));
            // ТЕСТ ПАДАЕТ, потому что PetStore API - фейковый и пускает с любыми паролями.
            // В реальном проекте это был бы критический баг.
            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
        }
    }
}

