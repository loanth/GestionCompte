using GestionCompte.Model;
using GestionCompte.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GestionCompte
{
    public partial class ListeTransaction : IObserver, IObservable
    {
        private readonly BanqueContextConfiguration _transactionContext;
        Transaction _transaction;
        public List<IObserver> Observers { get; set; }
        public ListeTransaction(IObserver obs)
        {
            InitializeComponent();
            Observers = new List<IObserver>();
            this.Observers.Add(obs);
            _transactionContext = new BanqueContextConfiguration();
            LoadTransactions();
        }
        public void update(Object? data = null)
        {
            this.LoadTransactions();
        }
        void notifyObservers()
        {

            foreach (IObserver obs in Observers)
            {

                obs.update();
            }
        }
        private void LoadTransactions()
        {
            var transactions = _transactionContext.Transactions.OrderByDescending(t => t.DateTransaction).ToList();
            listBox.ItemsSource = transactions;
        }
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Savebutton.Visibility = Visibility.Visible;
            Deletebutton.Visibility = Visibility.Visible;
            foreach (var item in listBox.Items)
            {
                var container = listBox.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                if (container != null)
                {
                    var grid = FindVisualChild<Grid>(container);
                    if (grid != null)
                    {
                        var textBlockNom = FindChild<TextBlock>(grid, "textBlockNom");
                        var textBoxNom = FindChild<TextBox>(grid, "textBoxNom");
                        var textBlockDate = FindChild<TextBlock>(grid, "textBlockDate");
                        var textBoxDate = FindChild<DatePicker>(grid, "textBoxDate");
                        var textBlockMontant = FindChild<TextBlock>(grid, "textBlockMontant");
                        var textBoxMontant = FindChild<TextBox>(grid, "textBoxMontant");
                        var ckbDebiteVue = FindChild<CheckBox>(grid, "ckbDebiteVue");
                        var ckbDebiteModif = FindChild<CheckBox>(grid, "ckbDebiteModif");
                        var ckbPointerVue = FindChild<CheckBox>(grid, "ckbPointerVue");
                        var ckbPointerModif = FindChild<CheckBox>(grid, "ckbPointerModif");

                        if (item == listBox.SelectedItem)
                        {
                            textBlockNom.Visibility = Visibility.Collapsed;
                            textBoxNom.Visibility = Visibility.Visible;

                            textBlockDate.Visibility = Visibility.Collapsed;
                            textBoxDate.Visibility = Visibility.Visible;

                            // Transférer la date de TextBlock au DatePicker si nécessaire
                            if (DateTime.TryParse(textBlockDate.Text, out DateTime date))
                            {
                                textBoxDate.SelectedDate = date;
                            }

                            textBlockMontant.Visibility = Visibility.Collapsed;
                            textBoxMontant.Visibility = Visibility.Visible;

                            ckbDebiteVue.Visibility = Visibility.Collapsed;
                            ckbDebiteModif.Visibility = Visibility.Visible;

                            ckbPointerVue.Visibility = Visibility.Collapsed;
                            ckbPointerModif.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            textBlockNom.Visibility = Visibility.Visible;
                            textBoxNom.Visibility = Visibility.Collapsed;

                            textBlockDate.Visibility = Visibility.Visible;
                            textBoxDate.Visibility = Visibility.Collapsed;

                            textBlockMontant.Visibility = Visibility.Visible;
                            textBoxMontant.Visibility = Visibility.Collapsed;

                            ckbDebiteVue.Visibility = Visibility.Visible;
                            ckbDebiteModif.Visibility = Visibility.Collapsed;

                            ckbPointerVue.Visibility = Visibility.Visible;
                            ckbPointerModif.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }

        }

        private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T childType)
                {
                    if (!string.IsNullOrEmpty(childName))
                    {
                        if (((FrameworkElement)child).Name == childName)
                        {
                            foundChild = (T)child;
                            break;
                        }
                    }
                    else
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
            }
            return foundChild;
        }

        // Ajoutez la méthode FindVisualChild ici
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T childType)
                {
                    foundChild = (T)child;
                    break;
                }
                else
                {
                    foundChild = FindVisualChild<T>(child);
                    if (foundChild != null) break;
                }
            }
            return foundChild;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (listBox.SelectedItem != null)
            {
                var selectedTransaction = (Transaction)listBox.SelectedItem;

                var container = listBox.ItemContainerGenerator.ContainerFromItem(selectedTransaction) as ListViewItem;
                if (container != null)
                {
                    var grid = FindVisualChild<Grid>(container);
                    if (grid != null)
                    {
                        var textBoxDate = FindChild<DatePicker>(grid, "textBoxDate");

                        // Vérifiez si la date sélectionnée est valide
                        if (textBoxDate.SelectedDate.HasValue)
                        {
                            // Récupérer la date sélectionnée
                            DateTime selectedDate = textBoxDate.SelectedDate.Value;

                            // Convertir la date en UTC+2 (ajouter 2 heures)
                            DateTime utcDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 0, 0, 0, DateTimeKind.Utc);

                            // Mettez à jour la date dans la transaction
                            selectedTransaction.DateTransaction = utcDate; // Enregistre uniquement la date sans l'heure
                        }
                        else
                        {
                            MessageBox.Show("Veuillez sélectionner une date valide.");
                            return;
                        }

                        
                        // Enregistrez les modifications dans la base de données
                        _transactionContext.SaveChanges();

                        // Rechargez les transactions pour mettre à jour l'affichage si nécessaire
                        LoadTransactions();

                        MessageBox.Show("Transaction sauvegardée avec succès !");
                        Savebutton.Visibility = Visibility.Collapsed;
                        Deletebutton.Visibility = Visibility.Collapsed;
                        this.notifyObservers();
                    }
                }
            }
            else
            {
                MessageBox.Show("Aucune transaction sélectionnée.");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                var selectedTransaction = (Transaction)listBox.SelectedItem;

                // Supprimer la transaction de la base de données
                _transactionContext.Transactions.Remove(selectedTransaction);
                _transactionContext.SaveChanges();

                // Rechargez les transactions pour mettre à jour l'affichage
                LoadTransactions();

                MessageBox.Show("Transaction supprimée avec succès !");
                Savebutton.Visibility = Visibility.Collapsed;
                Deletebutton.Visibility = Visibility.Collapsed;
                this.notifyObservers();
            }
            else
            {
                MessageBox.Show("Aucune transaction sélectionnée.");
            }
        
    }
    }
}
