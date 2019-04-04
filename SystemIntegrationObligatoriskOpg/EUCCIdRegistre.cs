using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;

namespace SystemIntegrationObligatoriskOpg
{
    public class EUCCIdRegistre : MessageQueueAccessor, IReceiver<CPRRegisterData>, IReceiver<Tuple<string, Gender, string, string, string, string>>
    {
        private List<EUCCIDData> _euData = new List<EUCCIDData>();

        public string Name => "EUCCId";

        void IReceiver<Tuple<string, Gender, string, string, string, string>>.OnReceive(object messageQueue, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)messageQueue;

            // End the asynchronous receive operation.
            Message m = mq.EndReceive(asyncResult.AsyncResult);

            var data = (Tuple<string, Gender, string, string, string, string>)m.Body;

            m.ResponseQueue.Formatter = JsonFormatter.Instance;

            var datevalue = DateTime.Now;

            var euccid = new EUCCIDData(data.Item6, "Midtjylland", datevalue.ToString("dd MM yyyy").Replace(" ", "") + "-" + RandomManager.Instance._Randy.Next(100000, 999999).ToString(), data.Item2, data.Item1, "Not yet given",
                data.Item1, data.Item3, m.Label.Split('-')[0], data.Item5);

            _euData.Add(euccid);

            MessageQueuesManager.Instance.SendMessage(euccid, m.Label, m.ResponseQueue);

            mq.BeginReceive();
        }

        void IReceiver<CPRRegisterData>.OnReceive(object messageQueue, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)messageQueue;

            // End the asynchronous receive operation.
            Message m = mq.EndReceive(asyncResult.AsyncResult);

            var cprData = (CPRRegisterData)m.Body;

            if (!_euData.Any(d => d.EUCCID == cprData.CPRNumber))
            {
                var eudata = Translator.Instance.UpdateFromCPRToEUCCID(cprData);
                _euData.Add(eudata);

                m.ResponseQueue.Formatter = JsonFormatter.Instance;

                MessageQueuesManager.Instance.SendMessage(new Tuple<string, string>(cprData.CPRNumber, eudata.EUCCID), m.ResponseQueue);

                Console.WriteLine("Got cpr data for: " + cprData.FirstName);
            }
            else
            {
                Console.WriteLine("Already updated current cpr");
            }

            mq.BeginReceive();
        }
    }
}