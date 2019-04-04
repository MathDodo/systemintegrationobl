using System;
using System.Collections.Generic;
using System.Messaging;
using Data;

namespace SystemIntegrationObligatoriskOpg
{
    public class CPRRegistre : MessageQueueAccessor, IReceiver<Tuple<string, string>>, IReceiver<Tuple<string, Gender>>
    {
        private List<CPRRegisterData> _register;

        public string Name => "Cpr";
        public int Count => _register.Count;

        public CPRRegisterData this[int index]
        {
            get
            {
                return _register[index];
            }
        }

        public CPRRegistre()
        {
            _register = new List<CPRRegisterData>();

            _register.Add(new CPRRegisterData("Odder", Status.Unmarried, 8300, "Hansen", "Peter", "271067-1113",
                "10150817", "Tværveje n 7", "Over Randlev", "", new List<string>(), new List<string>()));

            _register.Add(new CPRRegisterData("Vejle", Status.Married, 8700, "Nielsen", "Anne Rønne", "190202-1118",
                "10165462", "Løvestræde 9, 2. 8", "c/o Fam. Dencker", "180663-1239", new List<string>(), new List<string>()));
        }

        void IReceiver<Tuple<string, string>>.OnReceive(object messageQueue, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)messageQueue;

            // End the asynchronous receive operation.
            Message m = mq.EndReceive(asyncResult.AsyncResult);

            var body = (Tuple<string, string>)m.Body;

            _register.Find(c => c.CPRNumber == body.Item1).CPRNumber = body.Item2;

            mq.BeginReceive();
        }

        public void SendMessages(EUCCIdRegistre idRegistre)
        {
            Console.WriteLine("Sending request for updating cpr resistre");

            for (int i = 0; i < _register.Count; i++)
            {
                MessageQueuesManager.Instance.SendMessage(_register[i], Program.UPDATE_FROM_CPR_REQUEST, MessageRouter.Instance[typeof(object)], this[typeof(Tuple<string, string>)]);
            }
        }

        void IReceiver<Tuple<string, Gender>>.OnReceive(object messageQueue, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)messageQueue;

            // End the asynchronous receive operation.
            Message m = mq.EndReceive(asyncResult.AsyncResult);

            mq.BeginReceive();
        }
    }
}