using ManTyres.COMMON.Utils.Interfaces;
using System;

namespace ManTyres.COMMON.Utils
{


	/// <summary>
	/// Useful key for creating groups based on address of specific class.
	/// Parameters are expected to be not null or white space (ArgumentException is thrown in this case)
	/// </summary>
	public class GroupingKeyAddress : IGroupingKey
    {
        public GroupingKeyAddress(string countryCode, string city, string postalCode, string address)
        {
            const string errMessage = "Parameter is null, empty or white space";

            if (string.IsNullOrWhiteSpace(countryCode))
                throw new ArgumentException(errMessage, nameof(countryCode));
            CountryCode = countryCode;

            if(string.IsNullOrWhiteSpace(city))
                throw new ArgumentException(errMessage, nameof(city));
            City = city;

            if(string.IsNullOrWhiteSpace(postalCode))
                throw new ArgumentException(errMessage, nameof(postalCode));
            PostalCode = postalCode;

            if(string.IsNullOrWhiteSpace(address))
                throw new ArgumentException(errMessage, nameof(address));
            Address = address;
        }

        public string CountryCode { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
    }
}
