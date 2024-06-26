﻿using Microsoft.EntityFrameworkCore;
using Rental.Management.Final.Models;

namespace Rental.Management.Final.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<RentalProperty> RentalProperties { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<RentalContract> RentalContracts { get; set; }
        public DbSet<ContractPayment> ContractPayments { get; set; }
    }
}
