using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SystemIntegrationObligatoriskOpg
{
    public class HospitalSystem : MessageQueueAccessor, IReceiver<EUCCIDData>
    {
        private EUCCIdRegistre registre;

        public string Name => "HospitalName";

        public HospitalSystem(EUCCIdRegistre registre)
        {
            this.registre = registre;
        }

        public void SendMessage(string familyName, Gender gender, string birthCountry, string familyAdress, string apartmentNumber, string city)
        {
            var data = new Tuple<string, Gender, string, string, string, string>(familyName, gender, birthCountry, familyAdress, apartmentNumber, city);

            MessageQueuesManager.Instance.SendMessage(data, "Denmark-uniqueIDWhichIsSavedToSeeIfResponseIsForCorrectBirth", registre[typeof(Tuple<string, Gender, string, string, string, string>)], this[typeof(EUCCIDData)]);
        }

        public void OnReceive(object messageQueue, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)messageQueue;

            // End the asynchronous receive operation.
            Message m = mq.EndReceive(asyncResult.AsyncResult);

            var data = (EUCCIDData)m.Body;

            Console.WriteLine("Your child got the id: " + data.EUCCID);

            mq.BeginReceive();
        }
    }
}