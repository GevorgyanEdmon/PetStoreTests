using PetStoreTests.Models;
using PetTest;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PetStoreTests.Tests
{
    [TestClass]
    public class MemoryRefreshTests : BaseTest
    {
        [TestMethod]
        public void Create_and_Delete_Order()
        {
            var newOrder = new Order
            {
                Id = 0,
                PetId = 4,
                Quantity = 1,
                ShipDate = DateTime.Now,
                Status = "placed",
                Complete = true
            };
            var request = new RestRequest($"{Endpoints.Store}/order", Method.Post);
            request.AddBody(newOrder);
            var response = client.Execute<Order> (request);
            long createdId = response.Data.Id;
            var deleterequest = new RestRequest ($"{Endpoints.Store}/order/{createdId}", Method.Delete);
            var deleteresponse = client.Execute<Order> (deleterequest);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, deleteresponse.StatusCode);
        }
    }
}
