﻿using BarberShop.Entity;

namespace BarberShop.DTO
{
    public class ProductOrderDto
    {
        public int proOrderID { get; set; }
        public int proOrderQuantity { get; set; }
        public int orderID { get; set; }
        public int proID { get; set; }
    }
}
