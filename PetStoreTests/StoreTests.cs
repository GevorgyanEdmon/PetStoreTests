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
    public class StoreTests : BaseTest
    {
        [TestMethod]
        public void PlaceOrder_ShouldReturnPlaced()
        {
            var createOrder = new Order
            {
                Id = 0,
                PetId = 1,
                Quantity = 1,
                ShipDate = DateTime.Now,
                Status = "placed",
                Complete = true
            };
            var requset = new RestRequest($"{Endpoints.Store}/order", Method.Post);
            requset.AddBody(createOrder);
            var response = client.Execute<Order>(requset);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(createOrder.Status, response.Data.Status);
        }
    }
}
