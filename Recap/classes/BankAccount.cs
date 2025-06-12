public class BankAccount
{
    private double Balance;

    public double GetBalance()
    {
        return Balance;
    }

    public void Deposit(double amount)
    {
        if (amount > 0)
        {
            Balance += amount;
        }
    }
}