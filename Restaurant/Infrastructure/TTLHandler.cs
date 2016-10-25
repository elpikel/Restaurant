﻿using System;
using Restaurant.Workers.Abstract;

namespace Restaurant.Infrastructure
{
    public class TTLHandler<T> : IHandler<T> where T: ITTLMessage
   {
        private readonly IHandler<T> _handler;

        public TTLHandler(IHandler<T> handler)
        {
            _handler = handler;
        }

        public void Handle(T orderPaid)
        {
            if (orderPaid.ShoulBeProcessesdBefore > DateTime.Now.Add(-TimeSpan.FromMinutes(0.5)))
            {
                _handler.Handle(orderPaid);
            }
            else
            {
                //todo
             //   Console.WriteLine($"skipping {message.Id}");
            }
        }
    }
}