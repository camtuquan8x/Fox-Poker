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
                return "ĐÓNG";
            else if (button == true)
                return "ĐỒNG Ý";
            else
                return "KHÔNG ĐỒNG Ý";
        }

        public AbstractDialogData()
        {
            ButtonName = GetButtonName;
        }

        public abstract void ShowDialog();
    }

    public class DialogConfirm : AbstractDialogData
    {
        public DialogConfirm(string title, string content, Action<bool?> callback)
            : base()
        {
            this.Title = title;
            this.Content = content;
            this.ButtonCallback = callback;
        }

        public override void ShowDialog()
        {
            DialogConfirmView.Instance.ShowDialog(this);
        }
    }

    public class DialogMessage : AbstractDialogData
    {
        public DialogMessage(string title, string content, Action<bool?> callback)
            : base()
        {
            this.Title = title;
            this.Content = content;
            this.ButtonCallback = callback;
        }

        public override void ShowDialog()
        {
            DialogMessageView.Instance.ShowDialog(this);
        }
    }
    public class DialogInput : AbstractDialogData
    {
        public string startValue;
        public Action<bool?, string> onDoneInput;

        public override void ShowDialog()
        {
            DialogInputView.Instance.ShowDialog(this);
        }
    }
}
