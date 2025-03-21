using H3_BankThing.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Data
{
    /// <summary>
    /// Represents the application’s database context, used for interacting with the database.
    /// Inherits from <see cref="DbContext"/> and provides access to the <see cref="BankAccount"/> entities.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">Options for configuring the <see cref="DbContext"/>.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the collection of <see cref="BankAccount"/> entities in the database.
        /// </summary>
        public DbSet<BankAccount> Accounts { get; set; } = null!;
    }
}
