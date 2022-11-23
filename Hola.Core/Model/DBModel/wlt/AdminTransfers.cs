namespace Hola.Core.Model.DBModel.wlt
{
    public class AdminTransfers
    {
        public int ID { set; get; }
        public string SourceUserId { set; get; }
        public string DestinationUserId { set; get; }
        public long CurrencyId { set; get; }
        public decimal Amount { set; get; }
        public int Direction { set; get; }
        public string InvoiceN { set; get; }
        public string Notes { set; get; }
        public string DestPhoneN { set; get; }
        public string SourceCardHolderName { set; get; }
        public string DestinationCardHolderName { set; get; }
        public string SourcePhoneN { set; get; }
    }
}