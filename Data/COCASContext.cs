using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using COCAS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;

namespace COCAS.Models
{
    public class COCASContext : IdentityDbContext
    {
        public COCASContext (DbContextOptions<COCASContext> options)
            : base(options)
        {
        }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Department>()
                .HasAlternateKey(d => d.Name);
        }*/

        public DbSet<COCAS.Models.Course> Course { get; set; }
        public DbSet<COCAS.Models.Department> Department { get; set; }
        public DbSet<COCAS.Models.HoD> HoD { get; set; }
        public DbSet<COCAS.Models.Instructor> Instructor { get; set; }
        public DbSet<COCAS.Models.Schedule> Schedule { get; set; }
        public DbSet<COCAS.Models.Section> Section { get; set; }
        public DbSet<COCAS.Models.Student> Student { get; set; }
        public DbSet<COCAS.Models.UserType> UserType { get; set; }
        public DbSet<COCAS.Models.Form> Form { get; set; }
        public DbSet<COCAS.Models.User> User { get; set; }
        public DbSet<COCAS.Models.Request> Request { get; set; }
        public DbSet<COCAS.Models.Response> Response { get; set; }
        public DbSet<COCAS.Models.Time> Time { get; set; }
        public DbSet<COCAS.Models.Redirect> Redirect { get; set; }
    }
}
