﻿using BuildersBoleto.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildersBoleto.Infrastructure.Data
{
    public class BuildersBoletoDbContext : DbContext
    {
        public BuildersBoletoDbContext(DbContextOptions<BuildersBoletoDbContext> options) : base(options) { }

        public DbSet<BoletoEntity> Boletos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BoletoEntity>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).ValueGeneratedOnAdd();
                entity.Property(b => b.BarCode).IsRequired();
                entity.Property(b => b.OriginalAmount).HasColumnType("decimal(18,2)");
                entity.Property(b => b.Amount).HasColumnType("decimal(18,2)");
                entity.Property(b => b.DueDate).IsRequired();
                entity.Property(b => b.PaymentDate).IsRequired();
                entity.Property(b => b.InterestAmountCalculated).HasColumnType("decimal(18,2)");
                entity.Property(b => b.FineAmountCalculated).HasColumnType("decimal(18,2)");
            });
        }
    }
}
