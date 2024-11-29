using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCompte.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int CompteBancaireId { get; set; }
        
        private decimal montant;
        private DateTime dateTransaction;
        private string typeTransaction;
        private CompteBancaire compteBancaire;
        private bool debite;
        private bool pointer;

        public Transaction(decimal montant, DateTime dateTransaction, string typeTransaction, bool debite, bool pointer)
        {

            this.montant = montant;
            this.dateTransaction = dateTransaction.Date;
            this.typeTransaction = typeTransaction;
            this.debite = debite;
            this.pointer = pointer;
        }

        public decimal Montant { get => montant; set => montant = value; }
        public DateTime DateTransaction { get => dateTransaction; set => dateTransaction = value.Date; }

        public string TypeTransaction { get => typeTransaction; set => typeTransaction = value; }

        public bool Debite { get => debite; set => debite = value; }

        public bool Pointer { get => pointer; set => pointer = value; }
        public CompteBancaire CompteBancaire { get => compteBancaire; set => compteBancaire = value; }

        public void AfficherDetailsTransaction()
        {
            
            Console.WriteLine("Montant : " + montant);
            Console.WriteLine("Date Transaction : " + dateTransaction);
            Console.WriteLine("Type Transaction : " + typeTransaction);
        }
    }
}
