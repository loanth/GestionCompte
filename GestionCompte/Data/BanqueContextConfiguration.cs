using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GestionCompte.PostgreSQL
{
    using GestionCompte.Model;
    using Microsoft.EntityFrameworkCore;
    using System.Collections;


        public class BanqueContextConfiguration : DbContext
        {
            public DbSet<CompteBancaire> Compte { get; set; }
            public DbSet<Transaction> Transactions { get; set; }
            


            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                // Bien mettre les valeurs qui correpondent à votre connexion

                optionsBuilder.UseNpgsql("Host=localhost;Database=CompteBancaire;Username=postgres;Password=root");
            }



            private static BanqueContextConfiguration instance;

            public static BanqueContextConfiguration getInstance()
            {
                if (instance == null)
                {
                    instance = new BanqueContextConfiguration();
                }

                return instance;
            }
        }
    
}
