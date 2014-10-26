using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet;
using Puppet.Service;
using UnityEngine;

namespace Puppet.Service
{
    public class DialogService : Singleton<DialogService>
    {
        List<IDialogData> listDialog = new List<IDialogData>();
        IDialogData currentDialog;

        protected override void Init() { }

        public void ShowDialog(IDialogData dialog)
        {
            StartCoroutine(_ShowDialog(dialog));
        }

        IEnumerator _ShowDialog(IDialogData dialog)
        {

            if (dialog is DialogPromotion)
                yield return new WaitForSeconds(0.5f);
            if (dialog.IsMessageDialog == false)
            {
                dialog.ShowDialog();
            }
            else
            {
                listDialog.Add(dialog);
                while (PuApp.Instance.changingScene)
                    yield return new WaitForEndOfFrame();
                CheckAndShow();
            }

        }

        void CheckAndShow()
        {
            if (currentDialog == null && listDialog.Count > 0)
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

        public bool IsShowing(IDialogData dialog)
        {
            return currentDialog == dialog;
        }
    }
}
