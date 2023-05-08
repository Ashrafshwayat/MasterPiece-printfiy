//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MaserPieceProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.orderItems = new HashSet<orderItem>();
            this.Payments = new HashSet<Payment>();
        }
    
        public int OrderId { get; set; }
        public Nullable<double> OrderPrice { get; set; }
        public Nullable<bool> isCheckout { get; set; }
        public string Id { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Phone { get; set; }
        public string City { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string PaymentMethod { get; set; }
        public Nullable<bool> Isaccepted { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orderItem> orderItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual Order Orders1 { get; set; }
        public virtual Order Order1 { get; set; }
    }
}
