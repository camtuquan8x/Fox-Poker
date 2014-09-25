using Puppet.Service;
using System;
using System.Collections.Generic;

namespace Puppet.Service
{
    public interface IDialogData
    {
        string Title { get; set; }
        string Content { get; set; }
        Action<bool?> ButtonCallback { get; set; }
        System.Func<bool?, string> ButtonName { get; set; }
        Action onDestroy { get; set; }
        void ShowDialog();
    }

    public abstract class AbstractDialogData : IDialogData
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Action<bool?> ButtonCallback { get; set; }
        public Func<bool?, string> ButtonName { get; set; }
        public Action onDestroy { get; set; }

        protected virtual string GetButtonName(bool? button)
        {
            if (button == null)
                return "Close";
            else if (button == true)
                return "Yes";
            else
                return "No";
        }

        public AbstractDialogData()
        {
            ButtonName = GetButtonName;
        }

        public abstract void ShowDialog();
    }
}

public class DataDialogConfirm : AbstractDialogData
{
    public DataDialogConfirm(string title, string content, Action<bool?> callback)
        : base()
    {
        this.Title = title;
        this.Content = content;
        this.ButtonCallback = callback;
    }

    public override void ShowDialog()
    {
        DialogConfirm.Instance.ShowDialog(this);
    }
}

public class DataDataDialogMessage : AbstractDialogData
{
    public DataDataDialogMessage(string title, string content, Action<bool?> callback)
        : base()
    {
        this.Title = title;
        this.Content = content;
        this.ButtonCallback = callback;
    }

    public override void ShowDialog()
    {
        DialogMessage.Instance.ShowDialog(this);
    }
}
