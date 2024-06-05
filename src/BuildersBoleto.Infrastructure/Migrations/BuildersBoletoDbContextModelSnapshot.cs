using BuildersBoleto.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildersBoleto.Infrastructure.Migrations
{
    [DbContext(typeof(BuildersBoletoDbContext))]
    partial class BuildersBoletoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("Boleto.Domain.Entities.BoletoEntity", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<decimal>("Amount")
                    .HasColumnType("decimal(18,2)");

                b.Property<string>("BarCode")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<DateTime>("DueDate")
                    .HasColumnType("TEXT");

                b.Property<decimal>("FineAmountCalculated")
                    .HasColumnType("decimal(18,2)");

                b.Property<decimal>("InterestAmountCalculated")
                    .HasColumnType("decimal(18,2)");

                b.Property<decimal>("OriginalAmount")
                    .HasColumnType("decimal(18,2)");

                b.Property<DateTime>("PaymentDate")
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Boletos");
            });
#pragma warning restore 612, 618
        }
    }
}
