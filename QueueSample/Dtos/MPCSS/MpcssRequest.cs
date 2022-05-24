using System;
using System.Collections.Generic;
using System.Text;

namespace MpcssApp.Dtos.MPCSS
{
    public class MpcssRequest
    {
        public string CustomerId { get; set; }
        public string CustomerNumber { get; set; }
        public string DobOrRegistrationDate { get; set; }
        public string IdentificationTypeCode { get; set; }
        public string IdIssuingCountryCode { get; set; }
        public string MessageIdentificationCode { get; set; }
        public string CreatedDate { get; set; }
        public string POBox { get; set; }
        public string PostalCode { get; set; }
        public string Streetame { get; set; }
        public string BuildingNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string CityName { get; set; }
        public string TownName { get; set; }
        public string GovernorateName { get; set; }
        public string CountryCode { get; set; }
        public string ParticipantIdentificationCode{ get; set; }
        public string AdditionalInformation { get; set; }
    }
}
