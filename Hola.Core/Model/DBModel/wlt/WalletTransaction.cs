using System;

namespace Hola.Core.Model.DBModel.wlt
{
    public class WalletTransaction
    {
        public long Id { set; get; }
        public Guid WalletId { set; get; }
        public decimal Amount { set; get; }
        public decimal BlockedAmount { set; get; }
        public int TransactionType { set; get; }
        public Guid SourceID { set; get; }
        public int SourceType { set; get; }
        public int Direction { set; get; }
        public decimal PREBalance { set; get; }
        public decimal POSBalance { set; get; }
        public decimal PREBlockedBalance { set; get; }
        public decimal POSBlockedBalance { set; get; }
        public string Comments { set; get; }
    }
}