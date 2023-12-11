namespace MoneyTransformer.Models
{
    public class BankWalletJoin
    {
        public Bank bank { get; set; }
        public Useraccount user { get; set; }
        public Wallet Wallet { get; set; }
    }
}
