using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Entities;

public record Address
{
    public string Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string Country { get; init; }
    public string ZipCode { get; init; }

    public Address() { }

    public Address(string street, string city, string state, string country, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }

    public override string ToString() => $"{Street}, {City}, {State} {ZipCode}, {Country}";
}
