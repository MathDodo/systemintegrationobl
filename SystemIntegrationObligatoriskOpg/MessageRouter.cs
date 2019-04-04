using Data;
using System;
using System.Messaging;
using System.Collections.Generic;

namespace SystemIntegrationObligatoriskOpg
{
    public sealed class MessageRouter : SingletonBase<MessageRouter>, IReceiver<object>
    {
        private Dictionary<Type, MessageQueue> _queues;
        private Dictionary<Type, MessageQueue> _targetQueues;

        public string Name => "Router";

        public MessageQueue this[Type targetType]
        {
            get
            {
                return _queues[targetType];
            }
        }

        private MessageRouter()
        {
            _queues = new Dictionary<Type, MessageQueue>();
            _targetQueues = new Dictionary<Type, MessageQueue>();
        }

        public void AddEUCCIDQueue(Type queueReveiveType, MessageQueue targetQueue)
        {
            _targetQueues.Add(queueReveiveType, targetQueue);
        }

        public void OnReceive(object messageQueue, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)messageQueue;

            // End the asynchronous receive operation.
            Message m = mq.EndReceive(asyncResult.AsyncResult);

            if (m.Label == Program.UPDATE_FROM_CPR_REQUEST)
            {
                MessageQueuesManager.Instance.SendMessage(m.Body, m.Label, _targetQueues[typeof(CPRRegisterData)], m.ResponseQueue);
            }
            else if (m.Label == Program.GET_GENDER_REQUEST)
            {
            }

            mq.BeginReceive();
        }

        public void ReceivingQueue(Type queueReceiveType, MessageQueue messageQueue)
        {
            _queues.Add(queueReceiveType, messageQueue);
        }
    }
}