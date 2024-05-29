using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStore.Windows
{
    /// <summary>
    /// Логика взаимодействия для BasketWindow.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        public List<Product> products = new List<Product>();

        private List<PointOfIssue> _address = new List<PointOfIssue>();
        TrainingDataBaseEntities db = new TrainingDataBaseEntities();
        public int idSelectAddress = 0;
        public BasketWindow(List<Product> _product)
        {
            InitializeComponent();
            List<PointOfIssue> address = new List<PointOfIssue>();
            address.AddRange(db.PointOfIssues.OrderBy(adr => adr.PointStreet).ToList());


            _address = address
                .GroupBy(x => x.PointStreet)
                .Select(x => x.First())
                .ToList();

            cbPickupPoint.ItemsSource = _address;

            products = _product;
            if (products == null)
            {
                return;
            }
            else {
                lvBasket.ItemsSource = products;
                tbInfo.Text = "На данный момент в корзине " + products.Count + " товара(ов)";
            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        { 
            var selectedDel = (sender as Button).DataContext as Product;
            products.Remove(selectedDel);
            lvBasket.ItemsSource = products.ToList();
            tbInfo.Text = "На данный момент в корзине " + products.Count + " товара(ов)";

            if (products.Count <= 0)
            {
                CatalogWindow catalog = new CatalogWindow();
                catalog.Show();
                this.Close();
            }
        }

        private async void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            //var selectTextBox = (sender as Button).DataContext as TextBox;
            //if (selectTextBox.Text == "")
            //{
            //    MessageBox.Show("Необходимо заполнить данные");
            //}

            double sumPrice = 0;
            products.ForEach(x => sumPrice += x.ProductPrice);
            Random random = new Random();
            var code = random.Next(100, 999);
            var date = DateTime.Now.ToString();

            Order order = new Order()
            {
                OrderDate = date,
                OrderSumPrice = sumPrice,
                OrderAmount = 1,
                OrderCode = code,
                OrderPointOfIssue = idSelectAddress == 0 ? 1 : idSelectAddress,
            };
            db.Orders.Add(order);
            db.SaveChanges();

            var getNumberOrder = db.Orders.Where(x => x.OrderDate == date).FirstOrDefault().OrderNumber;
            MessageBox.Show(getNumberOrder.ToString());

            products.ForEach(x =>
            {
                OrderProduct orderProduct = new OrderProduct()
                {
                    IdOrder = getNumberOrder,
                    IdProduct = x.ProductNumber,
                    CountProduct = 2,
                };

                db.OrderProducts.Add(orderProduct);
                db.SaveChanges();
            });

            //Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = "Document"; // Default file name
            //dlg.DefaultExt = ".doc"; // Default file extension
            //dlg.Filter = "Text documents (.doc)|*.doc"; // Filter files by extension
            //dlg.ValidateNames = true;

            //// Show save file dialog box
            //Nullable<bool> result = dlg.ShowDialog();


            //if (result == true)
            //{
            //    StreamWriter sw = new StreamWriter(File.Create(dlg.FileName), Encoding.Unicode);
            //    sw.WriteLine("Чек");
            //    sw.WriteLine();
            //    sw.WriteLine("Дата заказа: " + date);
            //    sw.WriteLine();
            //    products.ForEach(x =>

            //    {
            //        sw.WriteLine("Наименование товара: " + x.ProductName);
            //        sw.WriteLine("Описание товара: " + x.ProductDescription);
            //        sw.WriteLine("Производитель товара: " + x.ProductManufacture);
            //        sw.WriteLine("Цена товара: " + x.ProductPrice);
            //        sw.WriteLine("Количество товара: " + 2);
            //        sw.WriteLine();
            //    });

            //    sw.WriteLine("\bКод получения: " + code.ToString());
            //    sw.Dispose();
            //}

            //CatalogWindow catalogWindow = new CatalogWindow();
            //catalogWindow.Show();
            //this.Close();

            TemplatePdfCheck template = new TemplatePdfCheck(order, products);

        }

        private void tbCountProduct_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbPickupPoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectAddress = cbPickupPoint.SelectedValue as PointOfIssue;
            idSelectAddress = selectAddress.IdPoint;
        }

        private void tbCountProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selectedDel = (sender as TextBox).DataContext as Product;
            var countProduct = sender as TextBox;
            if (countProduct.Text == "0")
            {
                products.Remove(selectedDel);
            }
            lvBasket.ItemsSource = products.ToList();
            tbInfo.Text = "На данный момент в корзине " + products.Count + " товара(ов)";
        }
    }
}
