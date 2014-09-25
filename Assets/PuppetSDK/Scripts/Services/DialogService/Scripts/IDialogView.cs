using System;
using System.Collections.Generic;

namespace Puppet.Service
{
    public interface IDialogView<T> where T : IDialogData
    {
        void ShowDialog(T data);
    }
}
