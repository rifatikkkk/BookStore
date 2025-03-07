//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookStore
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderProducts = new HashSet<OrderProduct>();
        }
    
        public int OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public double OrderSumPrice { get; set; }
        public int OrderAmount { get; set; }
        public int OrderCode { get; set; }
        public int OrderPointOfIssue { get; set; }

        //public int GetDeliveryTime =>
        //    OrderProducts.Any(p => p.Product.OnSavingCount < 3)
        //    ? 6
        //    : 3;

        public virtual PointOfIssue PointOfIssue { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        //public string GetOrderParts
        //{
        //    get
        //    {
        //        string result = String.Empty;
        //        result += $"Название\tЦена за товар\tКоличество\tСкидка\tЦена всех товаров\n";
        //        OrderProducts.ForEach(p =>
        //        {
        //            result += $"{p.Product.Name}\t\t{p.Product.Cost}р\t\t{p.Quality}\t\t{p.Product.Discount}%\t\t{p.Product.CostWithDiscount * p.Quality}р\n";
        //        });
        //        return result;
        //    }
        //}
    }
}
