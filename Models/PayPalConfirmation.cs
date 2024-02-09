namespace DECKMASTER.Models
{
    public class PayPalConfirmation
    {
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        public string PayerName { get; set; }
        public string PayerEmail { get; set; }

    }

}
