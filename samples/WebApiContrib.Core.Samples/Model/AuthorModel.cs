using System;

namespace WebApiContrib.Core.Samples.Model
{
    public class AuthorModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int IQ { get; set; }
        public object Signature { get; set; }

        public AuthorAddress Address { get; set; }
    }

    public class AuthorAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
