using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public interface IView
    {
        void ShowError(string message);
        void ShowConfirm(string message,Action<bool?> action);
    }

