using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserLoginHistory> UserLoginHistory { get; set; } = null!;
        public DbSet<RefreshToken>RefreshTokens { get; set; } = null!;
        public DbSet<SecurityLoginAttempt> SecurityLoginAttempts { get; set; } = null!;
    }
}
