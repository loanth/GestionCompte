using GestionCompte.Model;
using GestionCompte.PostgreSQL;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OfficeOpenXml;

namespace GestionCompte
{
    /// <summary>
    /// Logique d'interaction pour Accueil1.xaml
    /// </summary>
    public partial class Accueil1 : IObserver, IObservable
    {
        private readonly BanqueContextConfiguration _transactionContext;
        private ListeTransaction _listeTransaction;
        public List<IObserver> Observers { get; set; }
        public Accueil1()
        {
            InitializeComponent();
            _listeTransaction = new ListeTransaction(this);
            this.MainFrame.Navigate(_listeTransaction);
            Observers = new List<IObserver>();
            this.Observers.Add(_listeTransaction);
            _transactionContext = new BanqueContextConfiguration();
            LoadSolde();
        }
        public void update(Object? data = null)
        {
            LoadSolde();
        }
        void notifyObservers()
        {

            foreach (IObserver obs in Observers)
            {

                obs.update();
            }
        }

        private void ButtonAddTransaction_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new AjouterTransaction(_listeTransaction, this);
            newWindow.Show();
        }

        private void LoadSolde()
        {
            var solde = _transactionContext.Compte.Sum(c => c.Solde) +
            _transactionContext.Transactions.Sum(t => t.Montant);

            // Utilisation de "N2" pour deux décimales
            textBoxSolde.Text = solde.ToString("N2", new System.Globalization.CultureInfo("fr-FR")) + " €";

        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Récupérer les transactions de la base de données
                var transactions = _transactionContext.Transactions.ToList();

                

                // Créer un nouveau fichier Excel
                using (var package = new ExcelPackage())
                {
                    // Ajouter une nouvelle feuille de calcul
                    var worksheet = package.Workbook.Worksheets.Add("Transactions");

                    // Ajouter des en-têtes
                    worksheet.Cells[1, 1].Value = "Nom";
                    worksheet.Cells[1, 2].Value = "Date";
                    worksheet.Cells[1, 3].Value = "Montant";
                    worksheet.Cells[1, 4].Value = "Solde"; // Ajouter l'en-tête du solde

                    // Ajouter les données
                    int row = 2; // Démarrer à la deuxième ligne
                    foreach (var transaction in transactions)
                    {
                        worksheet.Cells[row, 1].Value = transaction.TypeTransaction; // Nom de la transaction
                        worksheet.Cells[row, 2].Value = transaction.DateTransaction.ToString("yyyy-MM-dd"); // Format de la date
                        worksheet.Cells[row, 3].Value = transaction.Montant; // Montant de la transaction
                        
                        row++;
                    }
                    worksheet.Cells[2, 4].Value = textBoxSolde.Text; // Montant de la transaction
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    var fileName = $"Transactions_{currentDate}.xlsx";
                    // Définir le chemin d'exportation (par exemple, sur le bureau)
                    var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Comptes", "Export", fileName);

                    // Enregistrer le fichier
                    package.SaveAs(new FileInfo(filePath));

                    MessageBox.Show($"Les transactions ont été exportées avec succès vers {filePath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exportation : {ex.Message}");
            }
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Comptes", "Depenses_mensuelles.xlsx");

            try
            {
                // Charger le fichier Excel
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    // Accéder à la première feuille de calcul
                    var worksheet = package.Workbook.Worksheets[0];

                    // Compter les lignes (excluant la première ligne si c'est un en-tête)
                    int rowCount = worksheet.Dimension.Rows;

                    // Boucle à travers les lignes (en commençant à la ligne 2 pour ignorer l'en-tête)
                    for (int row = 2; row <= rowCount; row++)
                    {
                        string typeTransaction = worksheet.Cells[row, 1].Text; // Lecture de la première colonne (TypeTransaction)
                        if (decimal.TryParse(worksheet.Cells[row, 2].Text, out decimal montant)) // Lecture de la deuxième colonne (Montant)
                        {
                            // Créer une nouvelle transaction
                            var transaction = new Transaction(
                                montant,                        // Le montant de la transaction (extrait de la cellule Excel)
                                DateTime.UtcNow.AddHours(2).Date, // La date actuelle en UTC+2, sans l'heure
                                typeTransaction,                // Le type de transaction (extrait de la cellule Excel)
                                false,
                                false

                                 // Un booléen pour indiquer si la transaction est débitée (à ajuster selon votre logique)
                            );
                            transaction.CompteBancaireId = 1;
                            // Ajouter la transaction à la base de données
                            _transactionContext.Transactions.Add(transaction);
                        }
                    }

                    // Enregistrer les modifications dans la base de données
                    _transactionContext.SaveChanges();

                    MessageBox.Show("Les transactions ont été importées avec succès.");
                    this.notifyObservers();
                    this.LoadSolde();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'importation : {ex.Message}");
            }
        }
    }
}
