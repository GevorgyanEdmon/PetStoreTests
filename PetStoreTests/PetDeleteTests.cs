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
    public class PetDeleteTests : BaseTest
    {
        [TestMethod]
        public void DeletePet_ShouldReturnOk()
        {
            var createPet = new Pet
            {
                Id = 0,
                Name = "Rex",
                Status = "availeble"
            };
            var request = new RestRequest(Endpoints.Pet, Method.Post);
            request.AddBody(createPet);
            var response = client.Execute<Pet>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            long createdId = response.Data.Id;
            var deleterequest = new RestRequest($"{Endpoints.Pet}/{createdId}", Method.Delete);
            var deleteresponse = client.Execute(deleterequest);
            Assert.AreEqual(HttpStatusCode.OK, deleteresponse.StatusCode);
            var checkrequrest = new RestRequest($"{Endpoints.Pet}/{createdId}", Method.Get);
            var checkresponse = client.Execute(checkrequrest);
            Assert.AreEqual (HttpStatusCode.NotFound, checkresponse.StatusCode);
        }
    }
}
