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

namespace BookStore.Windows
{
    /// <summary>
    /// Interaction logic for TemplatePdfCheck.xaml
    /// </summary>
    public partial class TemplatePdfCheck : Window
    {
        private Order _order;
        private TrainingDataBaseEntities _context = new TrainingDataBaseEntities();
        public TemplatePdfCheck(Order order, List<Product> products)
        {
            InitializeComponent();
            var resultInfo = "";
            products.ForEach(p =>
            {
                resultInfo += p.ProductName + p.ProductDescription + "\n";
            });
            _order = order;
            DataContext = new
            {
                Date = DateTime.Now,
                OrderNumber = _order.OrderNumber,
                Random = new Random().Next(100, 999),
                About = resultInfo,
                Issue = _order.PointOfIssue.PointCity + " " + _order.PointOfIssue.PointStreet + " " + _order.PointOfIssue.PointHouseNumber,
                Code = new Random().Next(100, 999),
                DateDelivery = _order.OrderDate,
                FullCost = _order.OrderSumPrice,
            };
            SavePdf();
        }

        public void SavePdf()
        {
            PrintDialog dlg = new PrintDialog();
            if (dlg.ShowDialog() == true)
            {
                dlg.PrintVisual(gridTemplate, "&&&");
            }
            MessageBox.Show("Сохранение талона выполнено успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
