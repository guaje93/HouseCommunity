using System.Collections.Generic;

namespace HouseCommunity.Model
{
    public class HousingDevelopment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
    }
}
