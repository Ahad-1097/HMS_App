using App.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models.ViewModel
{
    public class DashboardViewModel
    {
        public int  TotalPatient { get; set; }
        public int TotalDocter { get; set; }

        public int TotalNewPatient { get; set; }

        public int TotalNewDicharge { get; set; }
        public  List<UserWithRoleViewModel> UserList { get; set; }
        public List<UserWithRoleViewModel> DocterList { get; set; }
    }
}
