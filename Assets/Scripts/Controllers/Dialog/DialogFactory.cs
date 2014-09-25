using Puppet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DialogFactory
{
    public static List<IDialog> dialogs = new List<IDialog>();
    private static bool isShowedDialog = false;
    public static void QueueOrShowDialog(IDialog dialogModel)
    {
        if (isShowedDialog){
            Logger.Log("=====> DialogFactory === Chay vao day khong"  );
            dialogs.Add(dialogModel);
        }
        else
        {
            isShowedDialog = true;
            ShowDialog(dialogModel);
        }
    }
    public static void ApplicationDestroy() {
        dialogs = new List<IDialog>();
        isShowedDialog = false;
    }
    private static void ShowDialog(IDialog dialogModel)
    {
        if (dialogModel is DialogConfirmModel) {
            DialogConfirmModel model = dialogModel as DialogConfirmModel;
            ShowConfirmDialog(model);
        }
    }
    private static void ShowConfirmDialog(DialogConfirmModel model)
    {
        DialogConfirm.Instance.ShowConfirm(model, OnShowOtherDialog);
    }
    private static void OnShowOtherDialog()
    {
        if (dialogs.Count > 0) {
            IDialog model = dialogs[0];
            ShowDialog(model);
            dialogs.RemoveAt(0);
        }
        else
        {
            isShowedDialog = false;
        }
    }
}

