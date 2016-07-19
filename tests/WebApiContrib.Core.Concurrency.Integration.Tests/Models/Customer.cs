using System.Runtime.Serialization;

namespace WebApiContrib.Core.Concurrency.Integration.Tests.Models
{
    [DataContract]
    public class Customer
    {
        [DataMember(Name = "customer_id")]
        public string CustomerId { get; set; }

        [DataMember(Name = "firstname")]
        public string FirstName { get; set; }
    }
}
