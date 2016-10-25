﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Restaurant.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Models
{
    public class Order
    {
        private JsonSerializer _serializer = new JsonSerializer()
        {
            Formatting = Formatting.None,
            StringEscapeHandling = StringEscapeHandling.Default,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private dynamic _jsonOrder;

        public int TableNumber
        {
            get { return Getter.TryGet<int>(() => { return _jsonOrder.tableNumber; }); }
            set { _jsonOrder.tableNumber = value; }
        }

        public int TimeToCookMs
        {
            get { return Getter.TryGet<int>(() => { return _jsonOrder.timeToCookMs; }); }
            set { _jsonOrder.timeToCookMs = value; }
        }

        public decimal Tax
        {
            get { return Getter.TryGet<decimal>(() => { return _jsonOrder.tax; }); }
            set { _jsonOrder.tax = value; }
        }

        public decimal Total
        {
            get { return Getter.TryGet<decimal>(() => { return _jsonOrder.total; }); }
            set { _jsonOrder.total = value; }
        }

        public bool Paid
        {
            get { return Getter.TryGet<bool>(() => { return _jsonOrder.paid; }); }
            set { _jsonOrder.paid = value; }
        }
        
        public IReadOnlyList<OrderItem> Items
        {
            get { return ((JArray)(_jsonOrder.items ?? new JArray())).Select(item => new OrderItem(item.ToString())).ToList(); }
            set { _jsonOrder.items = JToken.FromObject(value, _serializer); }
        }

        public IReadOnlyList<string> Ingredients
        {
            get { return ((JArray)(_jsonOrder.ingredients ?? new JArray())).Select(x => (string)x).ToList(); }
            set { _jsonOrder.ingredients = JToken.FromObject(value, _serializer); }
        }
        
        public Order()
        {
            _jsonOrder = JObject.Parse("{}");
        }

        public Order(string json)
        {
            _jsonOrder = JObject.Parse(json);
        }

        public void AddItem(OrderItem orderItem)
        {
            var items = Items.ToList();
            items.Add(orderItem);
            Items = items;
        }

        public void AddItems(List<OrderItem> orderItems)
        {
            var items = Items.ToList();
            items.AddRange(orderItems);
            Items = items;
        }

        public void AddIngredient(string ingridient)
        {
            var ingridients = Ingredients.ToList();
            ingridients.Add(ingridient);
            Ingredients = ingridients;
        }

        public void AddIngredients(List<string> orderIngridients)
        {
            var ingridients = Ingredients.ToList();
            ingridients.AddRange(orderIngridients);
            Ingredients = ingridients;
        }

        public string ToJsonString()
        {
            return JObject.FromObject(this, _serializer).ToString();
        }
    }
}
