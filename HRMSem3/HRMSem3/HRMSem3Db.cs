using System;
using Microsoft.EntityFrameworkCore;
namespace HRMSem3ListApi
{
    public class HRMSem3Db : DbContext
    {
        public HRMSem3Db(DbContextOptions<HRMSem3Db> options) : base(options) { }

        public DbSet<HRMSem3> HRMSem3s => Set<HRMSem3>();
    }
}
