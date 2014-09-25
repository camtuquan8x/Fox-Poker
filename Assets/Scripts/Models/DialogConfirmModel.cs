using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DialogConfirmModel : IDialog
{
    public DialogConfirmModel(string title, string content, Action<bool?> onButtonClick) {
        this.Title = title;
        this.Content = content;
        this.OnButtonClick = onButtonClick;
    }
    private Action<bool?> onButtonClick;

    public Action<bool?> OnButtonClick
    {
        get { return onButtonClick; }
        set { onButtonClick = value; }
    }
}

