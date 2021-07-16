using bankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace bankApi.DAL
{
    public class BankApiContext : DbContext
    {
        public BankApiContext(DbContextOptions<BankApiContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }

}