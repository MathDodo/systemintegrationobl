using System;
using System.Collections.Generic;

namespace SystemIntegrationObligatoriskOpg
{
    [Serializable]
    public class CPRRegisterData
    {
        public string City { get; set; }
        public Status Status { get; set; }
        public int PostNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CPRNumber { get; set; }
        public string DoctorCVR { get; set; }
        public string AdressOne { get; set; }
        public string AdressTwo { get; set; }
        public string PartnerCPR { get; set; }
        public List<string> ParentsCPR { get; set; }
        public List<string> ChildrenCPR { get; set; }

        public CPRRegisterData()
        {
        }

        public CPRRegisterData(string city, Status status, int postNumber, string lastName, string firstName, string cPRNumber, string doctorCVR,
            string adressOne, string adressTwo, string partnerCPR, List<string> parentsCPR, List<string> childrenCPR)
        {
            City = city;
            Status = status;
            PostNumber = postNumber;
            LastName = lastName;
            FirstName = firstName;
            CPRNumber = cPRNumber;
            DoctorCVR = doctorCVR;
            AdressOne = adressOne;
            AdressTwo = adressTwo;
            PartnerCPR = partnerCPR;
            ParentsCPR = parentsCPR;
            ChildrenCPR = childrenCPR;
        }
    }
}