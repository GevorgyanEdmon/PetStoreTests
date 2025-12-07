using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace PetTest
{
    public  class BaseTest
    {
        protected RestClient client;

        [TestInitialize]
        public void Setup()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            var options = new RestClientOptions("https://petstore.swagger.io/v2/")
            {
                Proxy = new WebProxy() { UseDefaultCredentials = true }
            };

            client = new RestClient(options);
        }
    }
}
