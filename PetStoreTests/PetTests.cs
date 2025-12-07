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
    public class PetTests : BaseTest
    {
        [TestMethod]
        public void CreatePet_ShouldReturnSameName()
        { 
            var createPet = new Pet
            {
                Id = 0,
                Name = "Rex",
                Status = "available"
            };
            var request = new RestRequest(Endpoints.Pet, Method.Post);
            request.AddBody(createPet);
            var response = client.Execute<Pet>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(createPet.Name, response.Data.Name);
        }
        [TestMethod]
        public void CreateAndGetPet_ShouldReturnCorrectData()
        {
            var newPet = new Pet
            {
                Id = 0,
                Name = "Rex",
                Status = "available"
            };
            var postRequest = new RestRequest(Endpoints.Pet, Method.Post);
            postRequest.AddBody(newPet);
            var postResponse = client.Execute<Pet>(postRequest);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            long createdId = postResponse.Data.Id;
            var getRequest = new RestRequest($"{Endpoints.Pet}/{createdId}", Method.Get);
            var getResponse = client.Execute<Pet>(getRequest);
            Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.AreEqual(newPet.Name, getResponse.Data.Name);

        }
    }
}
