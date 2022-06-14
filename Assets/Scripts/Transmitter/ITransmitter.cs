using System;
using System.Collections.Generic;

public interface ITransmitter
{
    public event Action<IResource> Installed;
    public event Action Transferred;

    public bool IsFree { get; }

    public void Transfer(Queue<IResource> resources);
}
