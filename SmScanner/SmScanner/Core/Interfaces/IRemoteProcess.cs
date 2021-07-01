using System;

namespace SmScanner.Core.Interfaces
{
    public interface IRemoteProcess : IDisposable, IRemoteMemoryReader, IRemoteMemoryWriter, IProcessReader
    {
        public Smdkd.SmProcessInfo UnderlayingProcess { get; }
    }
}
