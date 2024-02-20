using App.Models.EntityModels;
using App.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models.DbContext
{

    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Patient> Patient { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Investigation> Investigation { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<InvestigationImages> InvestigationImages { get; set; }
        public DbSet<Operation> Operation { get; set; }
        public DbSet<Discharge> Discharge { get; set; }
        public DbSet<CaseSheet> CaseSheet { get; set; }
        public DbSet<Progress> Progress { get; set; }
        public DbSet<Outcome> Outcome { get; set; }
        
    }
}
