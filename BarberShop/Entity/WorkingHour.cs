﻿using System.ComponentModel.DataAnnotations;

namespace BarberShop.Entity
{
    public class WorkingHour
    {
        [Key]
        public int workingHourID { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }


        #region QH

        // OM Store
        public ICollection<Store> stores { get; set; } = new HashSet<Store>();
        #endregion
    }
}
