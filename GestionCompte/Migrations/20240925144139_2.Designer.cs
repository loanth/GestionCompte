﻿// <auto-generated />
using System;
using GestionCompte.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestionCompte.Migrations
{
    [DbContext(typeof(BanqueContextConfiguration))]
    [Migration("20240925144139_2")]
    partial class _2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GestionCompte.Model.CompteBancaire", b =>
                {
                    b.Property<int>("CompteBancaireId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CompteBancaireId"));

                    b.Property<int>("NumeroCompte")
                        .HasColumnType("integer");

                    b.Property<decimal>("Solde")
                        .HasColumnType("numeric");

                    b.Property<int>("TransactionId")
                        .HasColumnType("integer");

                    b.HasKey("CompteBancaireId");

                    b.ToTable("Compte");
                });

            modelBuilder.Entity("GestionCompte.Model.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionId"));

                    b.Property<int>("CompteBancaireId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateTransaction")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Montant")
                        .HasColumnType("numeric");

                    b.Property<string>("TypeTransaction")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TransactionId");

                    b.HasIndex("CompteBancaireId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("GestionCompte.Model.Transaction", b =>
                {
                    b.HasOne("GestionCompte.Model.CompteBancaire", "CompteBancaire")
                        .WithMany("Transactions")
                        .HasForeignKey("CompteBancaireId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompteBancaire");
                });

            modelBuilder.Entity("GestionCompte.Model.CompteBancaire", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
