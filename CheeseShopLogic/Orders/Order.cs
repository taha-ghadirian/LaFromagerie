﻿using CheeseShopLogic.CheeseBoxes;
using CheeseShopLogic.Infrastructure;
using CheeseShopLogic.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheeseShopLogic.Orders
{
    public class Order
    {
        private Order(Guid id, DateTime dateOrdered, ICheeseBoxAssembly cheeseBoxAssembly, User orderingUser, 
            CheeseBox cheeseBoxOrdered, DeliveryMethod deliveryMethod)
        {
            Id = id;
            DateOrdered = dateOrdered;
            Status = Status.Received;
            _message = "We have received your order and are busy preparing your cheese box for delivery.";
            _cheeseBoxAssembly = cheeseBoxAssembly;
            _orderingUser = orderingUser;
            _cheeseBoxOrdered = cheeseBoxOrdered;
            _deliveryMethod = deliveryMethod;
        }

        public Guid Id { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime DateDelivered { get; set; }
        public Status Status { get; set; }
        private string _message { get; set; }
        private ICheeseBoxAssembly _cheeseBoxAssembly { get; set; }
        private User _orderingUser { get; set; }
        private CheeseBox _cheeseBoxOrdered { get; set; }
        private DeliveryMethod _deliveryMethod { get; set; }

        public static Order Create(Guid id, DateTime dateOrdered, ICheeseBoxAssembly cheeseBoxAssembly, 
            User orderingUser, CheeseBox cheeseBoxOrdered, DeliveryMethod deliveryMethod)
        {
            var newOrder = new Order(id, dateOrdered, cheeseBoxAssembly, orderingUser, cheeseBoxOrdered, deliveryMethod);
            return newOrder;
        }

        public string PerformCheeseBoxAssembly()
        {
            var cheeseAssemblyMessage = _cheeseBoxAssembly.AssembleCheeseBox();
            return cheeseAssemblyMessage;
        }

        public void Dispatch()
        {
            Status = Status.Dispatched;
            _message = $"Hey {_orderingUser.Name}, we have dispatched your {_cheeseBoxOrdered.Name} cheese box. Exciting!";
        }

        public void MarkAsDelivered()
        {
            Status = Status.Delivered;
            DateDelivered = DateTime.Now;
            _message = $"Hey {_orderingUser.Name}, we have delivered your {_cheeseBoxOrdered.Name} cheese box. Bon appétit!";
        }

        public string GetStatusMessage()
        {
            return _message;
        }

        public decimal GetTotalCost()
        {
            var priceOfCheeseBoxOrdered = _cheeseBoxOrdered.CalculateTotalPrice();
            var totalCost = priceOfCheeseBoxOrdered + CalculateDeliveryCharge();
            return totalCost;
        }

        public decimal CalculateDeliveryCharge()
        {
            switch (_deliveryMethod)
            {
                case DeliveryMethod.NextDay: return 5.0m;
                case DeliveryMethod.Standard: return 1.0m;
                case DeliveryMethod.Free: return 0m;
                default: return 1.0m;
            }
        }
    }
}
