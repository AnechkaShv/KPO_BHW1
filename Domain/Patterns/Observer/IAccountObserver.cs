namespace Domain.Patterns.Observer;

public interface IAccountObserver
{
    void HandleBalanceChange(object sender, EventArgs e);
}