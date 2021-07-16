using System;
using System.ComponentModel.DataAnnotations;

namespace bankApi.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; }
        public decimal TransactionAmout { get; set; }
        public TransStatus TransactionStatus { get; set; }
        public bool IsSucessful => TransactionStatus.Equals(TransStatus.Success);
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars { get; set; }
        public TranType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }

        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 17)}";
        }
    }
    public enum TransStatus
    {
        Failed,
        Success,
        Error
    }

    public enum TranType
    {
        Deposit,
        Withdrawal,
        Transfer
    }

}