using System;

public interface IObjectInteractive
{
    public event Action<IObjectInteractive> EndedInterection;
    public void Interection(Player player);
}
