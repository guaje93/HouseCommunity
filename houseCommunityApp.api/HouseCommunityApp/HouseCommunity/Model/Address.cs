﻿namespace HouseCommunity.Model
{
    public class Address : IEntity
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }

        public override string ToString()
        {
            return $"{City}, {Street} {Number}";
        }
    }
}
