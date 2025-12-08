using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using PetStoreTests.Models;
using PetTest;
using RestSharp;
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
    }
}
