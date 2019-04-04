using Data;
using System;
using System.Collections.Generic;
using System.Messaging;

namespace SystemIntegrationObligatoriskOpg
{
    public sealed class Translator : SingletonBase<Translator>
    {
        private Dictionary<Type, MessageQueue> _messageQueues;

        public string Name => "Translator";

        public MessageQueue this[Type targetQueue]
        {
            get
            {
                return _messageQueues[targetQueue];
            }
        }

        private Translator()
        {
            _messageQueues = new Dictionary<Type, MessageQueue>();
        }

        public EUCCIDData UpdateFromCPRToEUCCID(CPRRegisterData data)
        {
            var cprArray = data.CPRNumber.Split('-');

            Gender gender = int.Parse(cprArray[1]) % 2 == 0 ? Gender.Female : Gender.Male;

            string year = cprArray[0].Substring(cprArray[0].Length - 2, 2);

            string current = DateTime.Now.Year.ToString();

            current = current.Substring(current.Length - 2, 2);

            string euccid = "";

            if (int.Parse(year) <= int.Parse(current))
            {
                cprArray[0] = cprArray[0].Remove(cprArray[0].Length - 2) + "20" + year;

                euccid = cprArray[0] + "-" + cprArray[1] + RandomManager.Instance._Randy.Next(10, 100).ToString();
            }
            else
            {
                cprArray[0] = cprArray[0].Remove(cprArray[0].Length - 2) + "19" + year;

                euccid = cprArray[0] + "-" + cprArray[1] + RandomManager.Instance._Randy.Next(10, 100).ToString();
            }

            var addressOneSplit = data.AdressOne.Split(',');
            var addressTwoSplit = data.AdressTwo.Split(',');

            if (addressOneSplit.Length > 1)
            {
                if (addressTwoSplit.Length > 1)
                {
                    return new EUCCIDData(data.City, DKDataSim.Instance.GetCounty(data.PostNumber), euccid, gender, data.LastName, data.FirstName, addressOneSplit[0] + "-" + addressTwoSplit[0],
                        DKDataSim.Instance.GetBirthCountry(data.CPRNumber), "Denmark", addressOneSplit[1] + "-" + addressTwoSplit[1]);
                }

                return new EUCCIDData(data.City, DKDataSim.Instance.GetCounty(data.PostNumber), euccid, gender, data.LastName, data.FirstName, addressOneSplit[0] + "-" + addressTwoSplit[0],
                    DKDataSim.Instance.GetBirthCountry(data.CPRNumber), "Denmark", addressOneSplit[1]);
            }

            if (addressTwoSplit.Length > 1)
            {
                return new EUCCIDData(data.City, DKDataSim.Instance.GetCounty(data.PostNumber), euccid, gender, data.LastName, data.FirstName, data.AdressOne + "-" + addressTwoSplit[0],
                    DKDataSim.Instance.GetBirthCountry(data.CPRNumber), "Denmark", "None-" + addressTwoSplit[1]);
            }

            return new EUCCIDData(data.City, DKDataSim.Instance.GetCounty(data.PostNumber), euccid, gender, data.LastName, data.FirstName, data.AdressOne + "-" + data.AdressTwo,
                DKDataSim.Instance.GetBirthCountry(data.CPRNumber), "Denmark", "None");
        }

        public void ReceivingQueue(Type queueReceiveType, MessageQueue messageQueue)
        {
            _messageQueues.Add(queueReceiveType, messageQueue);
        }
    }
}