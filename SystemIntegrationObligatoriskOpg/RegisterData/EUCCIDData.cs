using System;

namespace SystemIntegrationObligatoriskOpg
{
    [Serializable]
    public class EUCCIDData
    {
        public string City { get; set; }
        public string County { get; set; }
        public string EUCCID { get; set; }
        public Gender Gender { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string StreetNumber { get; set; }
        public string BirthCountry { get; set; }
        public string CurrentCountry { get; set; }
        public string AppartmentNumber { get; set; }

        public EUCCIDData()
        {
        }

        public EUCCIDData(string city, string county, string eUCCID, Gender gender, string lastName, string firstName, string streetNumber,
            string birthCountry, string currentCountry, string appartmentNumber)
        {
            City = city;
            County = county;
            EUCCID = eUCCID;
            Gender = gender;
            LastName = lastName;
            FirstName = firstName;
            StreetNumber = streetNumber;
            BirthCountry = birthCountry;
            CurrentCountry = currentCountry;
            AppartmentNumber = appartmentNumber;
        }
    }
}