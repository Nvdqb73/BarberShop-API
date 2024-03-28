using System.ComponentModel.DataAnnotations;

namespace BarberShop.Entity
{
    public class ServiceCategory
    {
        [Key]
        public int serCateID { get; set; }
        public string? serCateName { get; set; }
        public string? description { get; set; }


        #region QH
        public ICollection<Services> services { get; set; } = new HashSet<Services>();
        #endregion
    }
}
