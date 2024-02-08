using HomeProjectCore.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using mvc4.Models;

namespace HomeProjectCore.AppData
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ClientModel> ClientModel { get; set; }
        public DbSet<InvestorModel> InvestorTable { get; set; }
        public DbSet<FundModel> FundTable { get; set; }
    }
}
