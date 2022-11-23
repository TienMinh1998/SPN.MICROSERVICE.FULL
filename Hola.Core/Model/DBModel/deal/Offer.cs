namespace Hola.Core.Model.DBModel.deal
{
    public class Offer
    {
        public double OfferId { set; get; }
        public string CreatorId { set; get; }
        public string Type { set; get; }
        public string Currency { set; get; }
        public string BankAccount { set; get; }
        public decimal AvailableAmount { set; get; }
        public decimal Amount { set; get; }
        public decimal MinimumAmount { set; get; }
        public long WalletSourceId { set; get; }
        public bool IsDeleted { set; get; }
        public decimal InitialAmount { set; get; }
        public string CreatorPhone { set; get; }
    }
}