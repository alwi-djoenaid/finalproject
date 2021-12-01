using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class ApiDbContext: IdentityDbContext
    {

        public virtual DbSet<PaymentItem> Payments { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options)
        {

        }
    }
}
