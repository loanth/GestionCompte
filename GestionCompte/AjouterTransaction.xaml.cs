using GestionCompte.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GestionCompte.Model;
using System.Windows.Controls.Primitives;

namespace GestionCompte
{
    /// <summary>
    /// Logique d'interaction pour AjouterTransaction.xaml
    /// </summary>
    public partial class AjouterTransaction : IObservable

    {
        private readonly BanqueContextConfiguration _transactionContext;
        public List<IObserver> Observers { get; set; }
        public AjouterTransaction(IObserver listeTransaction, IObserver accueil)
        {
            InitializeComponent();
            Observers = new List<IObserver>();
            Observers.Add(listeTransaction);
            Observers.Add(accueil);
            _transactionContext = new BanqueContextConfiguration();
        }
        void notifyObservers()
        {

            foreach (IObserver obs in Observers)
            {

                obs.update();
            }
        }
        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            // Vérification des champs
            if (string.IsNullOrWhiteSpace(textBoxNom.Text))
            {
                MessageBox.Show("Veuillez entrer un nom pour la transaction.");
                return;
            }

            if (!datePickerTransaction.SelectedDate.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner une date valide.");
                return;
            }


            if (!decimal.TryParse(textBoxMontant.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal montant))
            {
                MessageBox.Show("Veuillez entrer un montant valide avec un point comme séparateur décimal.");
                return;
            }

            // Création d'une nouvelle transaction


            string TypeTransaction = textBoxNom.Text;   
               
            
            
                // Récupérer la date sélectionnée
                DateTime selectedDate = datePickerTransaction.SelectedDate.Value;

                // Convertir la date en UTC+2 (ajouter 2 heures)
                DateTime utcDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 0, 0, 0, DateTimeKind.Utc);

                // Mettez à jour la date dans la transaction
                
            
            
            


                var nouvelleTransaction = new Transaction(montant, utcDate, TypeTransaction,ckbDebite.IsChecked.Value,ckbPointe.IsChecked.Value);
            // Ajout à la base de données
            nouvelleTransaction.CompteBancaireId = 1;
            _transactionContext.Transactions.Add(nouvelleTransaction);
            _transactionContext.SaveChanges();

            MessageBox.Show("Transaction ajoutée avec succès !");
            this.notifyObservers();
            this.Close();
        }

    }
}
