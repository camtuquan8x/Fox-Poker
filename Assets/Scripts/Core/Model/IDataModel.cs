using System;
using System.Collections.Generic;

namespace Esimo
{
    public interface IDataModel
    {
        Dictionary<string, object> ToDictionary();

        string ToString();
    }
}
