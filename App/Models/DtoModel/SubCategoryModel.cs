using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.DtoModel
{
    public class SubCategoryModel
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }

        public string SubCategoryTitle { get; set; }
    }
}
