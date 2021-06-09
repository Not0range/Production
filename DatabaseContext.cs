using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

namespace Production
{
    class DatabaseContext : DbContext
    {
        public DbSet<Models.Account> Accounts { get; set; }

        public DbSet<Models.Order> Orders { get; set; }

        public DbSet<Models.Part> Parts { get; set; }

        public DbSet<Models.PartOperation> PartOperations { get; set; }

        public DbSet<Models.OrderOperation> OrderOperations { get; set; }


        public DatabaseContext() : base(Program.ConnectionString) { }
    }
}
