using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Entity
{
    public class Services
    {
        [Key]
        public int serID { get; set; }
        public string? serName { get; set; }
        public string? serDescription { get; set; }
        public float serPrice { get; set; }


        #region QH

        // MO ServiceCate
        public int serCateID { get; set; }
        [ForeignKey("serCateID")]
        public ServiceCategory ServiceCategory { get; set; } = null!;

        // OM
        // BookingSer
        // serMana
        public ICollection<ServiceManagement> ServiceManagement { get; set; } = new HashSet<ServiceManagement>();
        #endregion
    }
}
