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
    public class E2ETests : BaseTest
    {
        [TestMethod]
        public void UserCanRegisterAndBuyPet()
        {
            var createuser = new User
            { 
                UserName = "Edmon",
                Password = "34505",
            };
            var request = new RestRequest(Endpoints.User, Method.Post);
            request.AddBody(createuser);
            var response = client.Execute<User>(request);
            var getrequest = new RestRequest("user/login", Method.Get);
            getrequest.AddQueryParameter("username", "Edmon");
            getrequest.AddQueryParameter("password", "34505");
            var getresponse = client.Execute(getrequest);
            Assert.IsTrue(getresponse.Content.Contains("logged in"));
            var newPet = new Pet
            {
                Id = 3,
                Name = "Sanya",
                Status = "available"
            };
            var postRequest = new RestRequest(Endpoints.Pet, Method.Post);
            postRequest.AddBody(newPet);
            var postResponse = client.Execute<Pet>(postRequest);
            long createdId = postResponse.Data.Id;
            Assert.IsTrue(postResponse.Data.Id > 0);
            var orderPayload = new Order
            {
                Id = 0,
                PetId = createdId,
                Quantity = 1,
                ShipDate = DateTime.Now,
                Status = "placed",
                Complete = true
            };
            var getpetrequset = new RestRequest($"{Endpoints.Store}/order", Method.Post);
            getpetrequset.AddBody(orderPayload);
            var postgetresponse = client.Execute<Order>(getpetrequset);
            Assert.AreEqual(HttpStatusCode.OK, postgetresponse.StatusCode);
            StringAssert.Contains("placed", postgetresponse.Data.Status);
        }
    }
}
