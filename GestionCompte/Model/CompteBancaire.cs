using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCompte.Model
{
    public class CompteBancaire
    {
        public int CompteBancaireId { get; set; }
        
        private int numeroCompte;
        private decimal solde;
        private List<Transaction>? transactions;

        
        public CompteBancaire(int numeroCompte,decimal solde)
        {
            this.numeroCompte = numeroCompte;
            this.solde = solde;
            this.transactions = new List<Transaction>();
        }

       public int NumeroCompte { get => numeroCompte; set => numeroCompte = value; }

        public decimal Solde { get => solde; set => solde = value; }
        public List<Transaction>? Transactions { get => transactions; set => transactions = value; }

        public void Depot(decimal montant)
        {
            solde += montant;
        }

        public void Retrait(decimal montant)
        {
            if (montant > solde)
            {
                throw new ArgumentException("Le montant du retrait dépasse le solde.");
            }
            solde -= montant;
        }

        public decimal AfficherSolde()
        {
            return solde;
        }

       

        public void AjouterTransaction(Transaction transaction)
        {
            if (transaction.Montant < 0)
            {
                Retrait(-transaction.Montant);
            }
            else
            {
                Depot(transaction.Montant);
            }
        }
    }

    
}
