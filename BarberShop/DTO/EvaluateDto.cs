﻿using BarberShop.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.DTO
{
    public class EvaluateDto
    {
        public int evaluateID { get; set; }
        public int rating { get; set; }
        public string? comment { get; set; }
        public DateTime lastUpdate { get; set; }
        public int customerID {  get; set; }
        public int storeID { get; set; }
    }
}
