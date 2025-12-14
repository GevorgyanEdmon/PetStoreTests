using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PetStoreTests.Models;

namespace PetStoreTests.Helpers
{

    public static class TestDataHelper
    {
        public static User GenerateNewUser()
        {

            string uniqueId = Guid.NewGuid().ToString().Substring(0, 8);

            return new User
            {
                UserName = "user_" + uniqueId,
                Email = uniqueId + "@test.com"
            };
        }
    }
}
