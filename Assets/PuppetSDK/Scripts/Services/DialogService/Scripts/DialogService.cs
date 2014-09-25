using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet;
using Puppet.Service;

public class DialogService : Singleton<DialogService>
{
    List<IDialogData> listDialog = new List<IDialogData>();
    IDialogData currentDialog;

    protected override void Init() {}

    public void ShowDialog(IDialogData dialog)
    {
        listDialog.Add(dialog);
        CheckAndShow();
    }

    void CheckAndShow()
    {
        if(currentDialog == null && listDialog.Count > 0)
        {
            currentDialog = listDialog[0];
            currentDialog.ShowDialog();
            currentDialog.onDestroy = () =>
            {
                listDialog.RemoveAt(0);
                currentDialog = null;
                //Show Hide Animation
                Invoke("CheckAndShow", 0.3f);
            };
        }
    }
}
