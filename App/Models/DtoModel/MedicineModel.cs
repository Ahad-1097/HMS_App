using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.DtoModel
{
    public class MedicineModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int StockQuantity { get; set; }
    }
}
