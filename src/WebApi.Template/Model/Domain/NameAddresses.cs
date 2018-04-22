using System;
using System.Collections.Generic;

namespace WebApi.Template.Model.Domain
{
    public partial class NameAddresses
    {

        public long NameAddressId { get; set; }
        public int NameAddressTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Unit { get; set; }
        public string Pobox { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryIso { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string LastUpdateUser { get; set; }
        public bool? EmailMarketingOptIn { get; set; }
    }
}
