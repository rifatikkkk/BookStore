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

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BookStore.Windows
{
    /// <summary>
    /// Логика взаимодействия для CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        private TrainingDataBaseEntities _context = new TrainingDataBaseEntities();
        private List<Product> _products, _typeManufacture, _lvBask = new List<Product>();
        private string _SelectedType, _FindedName, _SelectedSort;

        private Order _currentOrder = new Order();
        public CatalogWindow()
        {
            InitializeComponent();
            _context.Configuration.ProxyCreationEnabled = false;
            _products = _context.Products.OrderBy(product => product.ProductName).ToList();
            lvListProduct.ItemsSource = _products;
            labelFound.Content = _context.Products.Count() + " of " + _context.Products.Count();

            List<Product> manufactures = new List<Product>
            {
                new Product
                {
                    ProductManufacture = "Все производители"
                }
            };
            manufactures.AddRange(_context.Products
                .OrderBy(manufactur => manufactur.ProductManufacture)
                .ToList());

            _typeManufacture = manufactures
                .GroupBy(x => x.ProductManufacture)
                .Select(x => x.First())
                .ToList();

            cbTypeManufacture.ItemsSource = _typeManufacture;
            cbTypeManufacture.SelectedIndex = 0;

            this._products = _context.Products.ToList();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void tbFindProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            _FindedName = tbFindProduct.Text;

            if (_FindedName != "")
            {
                _products = _context.Products.OrderBy(f => f.ProductName)
                    .Where(find => find.ProductName.ToLower().Contains(_FindedName)
                    || find.ProductDescription.ToLower().Contains(_FindedName)
                    || find.ProductManufacture.ToLower().Contains(_FindedName))
                    .ToList();
            }
            else
            {
                _products = _context.Products.OrderBy(t => t.ProductName).ToList();
            }

            lvListProduct.ItemsSource = _products;
            labelFound.Content = _products.Count + " of " + _context.Products.Count();
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_lvBask.Count != 0)
            {
                BasketWindow basketWindow = new BasketWindow(_lvBask.ToList());
                basketWindow.Show();
                this.Close();
            }
        }

        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            //var selected = (sender as MenuItem).DataContext as Product;
            //if (_currentOrder.OrderProducts.Any(p => p.Product.ProductNumber == selected.ProductNumber)) 
            //{
            //    var current = _currentOrder.OrderProducts.FirstOrDefault(p => p.Product.ProductNumber == selected.ProductNumber);
            //    current.Product.ProductInStock += 1;
            //}

            //else
            //{
            //    _currentOrder.OrderProducts.Add(new OrderProduct
            //    {
            //        Product = selected,
            //        CountProduct = 1,
            //    });
            //}
            

            var selected = (Product)lvListProduct.SelectedItem;
            if (lvListProduct.SelectedIndex == -1)
            {
                return;
            }
            else if (selected.ConditionStock)
            {
                MessageBox.Show("Данный товар невозможно добавить, т.к. его нет на складе! Приносим свои извинения!", "Ошибка!");
            }
            else if (_lvBask.Contains(selected))
            {
                MessageBox.Show("Данный товар уже был ранее добавлен!", "Ошибка");
            }
            else
            {
                //var itemAdd = (sender as ListItem). as Product;
                //BasketWindow basketWindow = new BasketWindow(itemAdd);
                //basketWindow.Show();
                // _listBasket.Add(lvListProduct.SelectedIndex);
                //_lvBask.Add(itemAdd);
                //lvListProduct.ItemsSource = _lvBask.ToList();
                _lvBask.Add(selected);
                _currentOrder.OrderProducts.Add(new OrderProduct
                {
                    Product = selected,
                    CountProduct = 1,
                });

                if (_lvBask != null)
                {
                    btnOrder.Content = "Сформировать заказ (" + _lvBask.Count.ToString() + ")";
                    btnOrder.Visibility = Visibility.Visible;
                }
            }
        }

        private void cbTypeManufacture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectType = cbTypeManufacture.SelectedValue as Product;
            if (selectType.ProductManufacture == "Все производители")
            {
                _products = _context.Products.OrderBy(product => product.ProductName).ToList();
                labelFound.Content = _products.Count + " of " + _context.Products.Count();
                lvListProduct.ItemsSource = _products;
                cbSortProduct.SelectedItem = null;
            }
            else
            {
                _products = _context.Products.Where(p => p.ProductManufacture == selectType.ProductManufacture).ToList();
                lvListProduct.ItemsSource = _products;
                labelFound.Content = _products.Count + " of " + _context.Products.Count();
            }
        }

        private void cbSortProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)cbSortProduct.SelectedItem;
            if (typeItem == null)
            {
                return;
            }
            else
            {
                _SelectedSort = typeItem.Content.ToString();
            }

            switch (_SelectedSort)
            {
                case "Ascending":
                    _products = _context.Products.OrderBy(a => a.ProductPrice).ToList();
                    break;

                case "Descending":
                    _products = _context.Products.OrderByDescending(a => a.ProductPrice).ToList();
                    break;

                case "Clear":
                    _products = _context.Products.OrderBy(c => c.ProductName).ToList();
                    cbSortProduct.SelectedItem = null;
                    break;
                default:
                    MessageBox.Show("Ohh");
                    break;

            }
            labelFound.Content = _products.Count + " of " + _context.Products.Count();
            lvListProduct.ItemsSource = _products;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
