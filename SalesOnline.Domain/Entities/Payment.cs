using SalesOnline.Domain.Core;
using System;

namespace SalesOnline.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }
        
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentMethod Method { get; set; }
        public string TransactionId { get; set; }
        public string PaymentDetails { get; set; }
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        BankTransfer,
        PayPal,
        Cash
    }
}
