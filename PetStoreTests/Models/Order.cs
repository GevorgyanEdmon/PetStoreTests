using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PetStoreTests.Models
{
    public class Order
    {
        public long Id { get; set; }
        public long PetId { get; set; }
        public int Quantity { get; set; }

        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime ShipDate { get; set; }
        public string Status { get; set; }
        public bool Complete { get; set; }
    }
}
