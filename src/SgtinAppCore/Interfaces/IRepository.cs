using System;
using System.Collections.Generic;
using System.Text;

namespace SgtinAppCore.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> SearchBySpec(string query);
    }
}
