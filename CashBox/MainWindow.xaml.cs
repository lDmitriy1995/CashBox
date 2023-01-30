using CashBox.lib;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace CashBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Database db = new Database();
        public Dictionary<string,int> Amounts = new Dictionary<string,int>();
        public List<int> Indexes = new List<int>();

        //public Database db = new Database();
        //public Dictionary<string, int> Amounts = new Dictionary<string, int>();
        //public List<int> Indexes = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            Database db = new Database();
        }

        private void one_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += "1";
        }

        private void two_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '2';
        }

        private void three_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '3';
        }

        private void four_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '4';
        }

        private void five_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '5';
        }

        private void six_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '6';
        }

        private void seven_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '7';
        }

        private void eight_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '8';
        }

        private void nine_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '9';
        }

        private void zero_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '0';
        }

        private void dot_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text += '.';
        }

        private void cancellation_Click(object sender, RoutedEventArgs e)
        {
            inputField.Text = "";
        }

        private void inputButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputField.Text))
            {
                lbMessage.Foreground = Brushes.MediumPurple;
                lbMessage.Content = "Вы ничего не ввели";
                return;
            }
            bool found = false;
            if (found == false)
            {
                foreach (Product p in db.Products.ToList())
                {
                    if (p.Id == Int32.Parse(inputField.Text))
                    {
                        int Id = Int32.Parse(inputField.Text);
                        var list = lvProductsList.Items.Cast<Product>().ToList();
                        string[] Names = new string[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            Names[i] = list[i].Name;
                        }
                        bool contain = Names.Contains(p.Name);
                        if (contain)
                            return;
                        p.Sum = p.Price;
                        lvProductsList.Items.Add(new Product(p.Name, 1, p.Price, p.Sum));
                        Amounts.Add(p.Name, 1);
                        Indexes.Add(Id);
                        lbMessage.Foreground = Brushes.LimeGreen;
                        lbMessage.Content = $"Товар с Id {inputField.Text} был добавлен в заказ";
                        found = true;
                        lbDisplayTotal.Content = TotalSum();
                        SelectedItemInfo();
                        break;
                    }
                    else
                    {
                        lbMessage.Content = "Такой товар уже есть в заказе";
                    }
                }
            }
            if (found == false)
            {
                lbMessage.Foreground = Brushes.MediumPurple;
                lbMessage.Content = "Товар с таким Id не сеществует";
            }
        }
        private void deleteButtonClick_Click(object sender, RoutedEventArgs e)
        {
            int index = lvProductsList.SelectedIndex;
            Product product = (Product)lvProductsList.SelectedItem;
            if (NotSelectedItem(index))
            {
                return;
            }
            lvProductsList.Items.Remove(lvProductsList.SelectedItem);
            Amounts.Remove(product.Name);
            Indexes.RemoveAt(index);
            lbDisplayTotal.Content = TotalSum();
        }
        public bool NotSelectedItem(int index)
        {
            if (index == -1)
            {
                lbMessage.Foreground = Brushes.MediumPurple;

                lbMessage.Content = "Щёлкните на товар, а затем на '-', '+' или '❌'";
                return true;
            }
            return false;
        }
        private void decreaseButton_Click(object sender, RoutedEventArgs e)
        {
            int index = lvProductsList.SelectedIndex;
            Product product = (Product)lvProductsList.SelectedItem;

            if (NotSelectedItem(index))
            {
                return;
            }
            if (Amounts[product.Name] > 1)
            {
                Amounts[product.Name] -= 1;
            }
            else
            {
                lbMessage.Foreground = Brushes.MediumPurple;
                lbMessage.Content = "Количесвто товаров не может быть равным нулю";
                return;
            }
            string name = product.Name;
            int amount = Amounts[product.Name];
            double price = product.Price;
            double sum = amount * price;
            lvProductsList.Items.RemoveAt(index);
            lvProductsList.Items.Insert(index, new Product(name, amount, price, sum));
            lbDisplayTotal.Content = TotalSum();
            SelectedItemInfo();
        }

        private void increaseButton_Click(object sender, RoutedEventArgs e)
        {
            var index = lvProductsList.SelectedIndex;
            Product product = (Product)lvProductsList.SelectedItem;
            if (NotSelectedItem(index))
            {
                return;
            }
            if (Amounts[product.Name] >= 1 && Amounts[product.Name] < db.Products.ToList()[index].Amount)
            {
                Amounts[product.Name] += 1;
            }
            else
            {
                lbMessage.Foreground = Brushes.MediumPurple;
                lbMessage.Content = "Товар закончился";
                return;
            }
            string name = product.Name;
            int amount = Amounts[product.Name];
            double price = product.Price;
            double sum = amount * price;
            lvProductsList.Items.RemoveAt(index);
            lvProductsList.Items.Insert(index, new Product(name, amount, price, sum));
            lbDisplayTotal.Content = TotalSum();
            SelectedItemInfo();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            lvProductsList.Items.Clear();
            Amounts.Clear();
            Indexes.Clear();
            lbDisplayTotal.Content = TotalSum();
            SelectedItemInfo();
        }
        public double TotalSum()
        {
            double sum = 0;
            foreach (Product p in lvProductsList.Items)
            {
                sum += p.Sum;
            }
            return sum;
        }
        public void SelectedItemInfo()
        {
            totalValue.Content = TotalSum();
            int index = lvProductsList.SelectedIndex;
            ////selectedItemPrice.Content = selected.Price;
            ////selectedItemSale.Content = selected.Sum;
            //selectedItemAmount.Content = Amounts[index];
        }

        private void discountButton_Click(object sender, RoutedEventArgs e)
        {
            totalValue.Content = TotalSum() * 0.9;
            lbDisplayTotal.Content = TotalSum() * 0.9;
            //var list = lvProductsList.Items.Cast<Product>().ToList();
            //foreach (var item in list)
            //{
            //    item.Price = item.Price * (Int32.Parse(inputField.Text) / 100);
            //    item.Sum = item.Price * item.Amount;
            //}
            //lvProductsList = new ListView();
            //TotalSum();
            //lvProductsList.Items.Clear();
            //lvProductsList.ItemsSource = list;
        }

        private void byCardButton_Click(object sender, RoutedEventArgs e)
        {
            db.Orders.Add(new Order(DateTime.Now, TotalSum(), "Jake"));
            for (int i = 0; i < lvProductsList.Items.Count; i++)
            {
                if (db.Products.ToList()[i].Name == Amounts.ElementAt(i).Key)
                {
                    db.Products.ToList()[i].Amount -= Amounts[db.Products.ToList()[i].Name];
                }
            }
            db.SaveChanges();
            lbMessage.Foreground = Brushes.LimeGreen;
            lbMessage.Content = "Оплата картой прошла успешно";
        }
        private void byCashButton_Click(object sender, RoutedEventArgs e)
        {
            //db.Orders.Add(new Order(DateTime.Now, TotalSum(), "Jake"));
            //var list = lvProductsList.Items.Cast<Product>().ToList();
            //for (int a = 0; a < lvProductsList.Items.Count; a++)
            //{
            //    for (int j = 0; j < Amounts.Count; j++)
            //    {
            //        if (list[a].Name == Amounts.ElementAt(j).Key)
            //        {
            //            MessageBox.Show($"Name: {db.Products.ToList()[Indexes[0]].Name}" +
            //                $"\nAmount: {db.Products.ToList()[Indexes[0]].Amount}");
            //            db.Products.ToList()[Indexes[a] - 1].Amount -= Amounts[db.Products.ToList()[j].Name];
            //            db.SaveChanges();
            //        }
            //    }
            //}
            //lbMessage.Foreground = Brushes.LimeGreen;
            //lbMessage.Content = "Оплата наличными прошла успешно";
        }
    }
}
