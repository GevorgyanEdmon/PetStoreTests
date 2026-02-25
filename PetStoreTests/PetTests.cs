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
        [TestMethod]
        public void GetPetsByStatus_ShouldReturnOnlyAvailable()
        { 
         var request = new RestRequest($"{Endpoints.Pet}/findByStatus", Method.Get);
            request.AddQueryParameter("status", "available");
            var response = client.Execute<List<Pet>>(request);
            Assert.AreEqual (HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.Data.Count > 0);
            foreach (var pet in response.Data)
                Assert.AreEqual("available", pet.Status, $"Ошибка! Питомец {pet.Id} имеет неверный статус.");
        }
        [TestMethod]
        public void UploadImage_ShouldReturnSuccess()
        {
            long petId = 1;
            var request = new RestRequest($"{Endpoints.Pet}/{petId}/uploadImage", Method.Post);
            request.AddFile("file", "C:\\Users\\Gigiboba\\source\\repos\\PetStoreTests\\PetStoreTests\\test_image.jpg");
            var responce = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
            StringAssert.Contains(responce.Content, "File uploaded");
        }
        [TestMethod]
        public void Logout_ShouldReturnOk()
        {
            var requsest = new RestRequest($"{Endpoints.User}/logout", Method.Get);
            var response = client.Execute(requsest);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod]
        public void GetStoreInventory_ShouldReturnOk()
        {
            var requsest = new RestRequest($"{Endpoints.Store}/inventory", Method.Get);
            var response = client.Execute(requsest);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod]
        public void DeleteOrder_ShouldReturnOk()
        {
            var neworder = new Order
            {
                Id = 0,
                PetId = 4,
                Quantity = 1,
                ShipDate = DateTime.Now,
                Status = "placed",
                Complete = true
            };
            var requrest = new RestRequest($"{Endpoints.Store}/order", Method.Post);
            requrest.AddBody(neworder);
            var response = client.Execute<Order>(requrest);
            long realId = response.Data.Id;
            var deleterequest = new RestRequest($"{Endpoints.Store}/order/{realId}", Method.Delete);
            var deleteresponce = client.Execute(deleterequest);
            Assert.AreEqual(HttpStatusCode.OK, deleteresponce.StatusCode);
        }
        [TestMethod]
        public void GetSoldPets_ShouldVerifyList()
        {
            var request = new RestRequest(Endpoints.Pet + "/findByStatus", Method.Get);
            request.AddQueryParameter("status", "sold");
            var response = client.Execute<List<Pet>>(request);
            Assert.IsTrue(response.Data.Count > 0);
            foreach (var pet in response.Data)
            {
                // Проверяем, что имя не null и не пустота
                Assert.IsFalse(string.IsNullOrEmpty(pet.Status));
                Assert.AreEqual("sold", pet.Status);
            }
        }
    }
}
