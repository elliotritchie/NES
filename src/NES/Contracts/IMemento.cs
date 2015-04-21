using System;

namespace NES.Contracts
{
    public interface IMemento
    {
        Memento<T> GetMemento<T>();
    }
}