using System.Collections.Generic;
using System.Threading;
using Restaurant.Models;
using Restaurant.Workers.Abstract;

namespace Restaurant.Infrastructure
{
    public class MFDispatcher<T> : IHandle<T>
    {
        private readonly IEnumerable<QueuedHandler<T>> _queuedHandlers;

        public MFDispatcher(IEnumerable<QueuedHandler<T>> queuedHandlers)
        {
            _queuedHandlers = queuedHandlers;
        }

        public void Handle(T message)
        {
            while (true)
            {
                var managedToDispatch = false;

                foreach (var queuedHandler in _queuedHandlers)
                {
                    if (queuedHandler.QueueLength < 5)
                    {
                        managedToDispatch = true;
                        queuedHandler.Handle(message);
                    }
                }

                if (!managedToDispatch)
                {
                    Thread.Sleep(200);
                    continue;
                }

                break;
            }
        }
    }
}