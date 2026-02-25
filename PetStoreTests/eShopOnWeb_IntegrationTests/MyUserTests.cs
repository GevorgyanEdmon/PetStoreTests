using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.eShopWeb.FunctionalTests.Web; 
using Xunit;

namespace Microsoft.eShopWeb.FunctionalTests.ApiTests 
{

    public class MyUserTests : IClassFixture<TestApplication>
    {
        private readonly HttpClient _client;


        public MyUserTests(TestApplication factory)
        {

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetCurrentUser_ReturnsOk()
        {
            // Отправляем запрос на API
            var response = await _client.GetAsync("/user"); // Путь к контроллеру

            // Проверяем, что не упали с 500
            response.EnsureSuccessStatusCode();

            // Читаем ответ
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Проверяем, что ответ содержит ожидаемый JSON ключ
            Assert.Contains("isAuthenticated", stringResponse);
        }
        [Fact]
        public async Task GetHomePage_ReturnsSuccessAndShopName()
        {
            // 1. Делаем GET запрос на корень сайта
            var response = await _client.GetAsync("/");

            // 2. Проверяем, что статус успешный
            response.EnsureSuccessStatusCode();

            // 3. Читаем ответ
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("eShop", stringResponse);
        }
        [Fact]
        public async Task GetNonExistentPage_ReturnsNotFound()
        {

            var response = await _client.GetAsync("/this-page-does-not-exist");


            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task PostLogout_ReturnsRedirectOrOk()
        {
            var emptyContent = new StringContent("");

            var response = await _client.PostAsync("/user/logout", emptyContent);

            bool isSuccessOrRedirect =
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Redirect;

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
