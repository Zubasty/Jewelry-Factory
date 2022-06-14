using System;

public interface IObjectInteractive
{
    public event Action<IObjectInteractive> EndedInterection;
    public bool CanInterection(Player player);
    public void Interection(Player player);
}
