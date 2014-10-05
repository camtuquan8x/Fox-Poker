using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet.Service
{
    [PrefabAttribute(Name = "Prefabs/Dialog/DialogInput", Depth = 10, IsAttachedToCamera = true, IsUIPanel = true)]
    public class DialogInputView : BaseDialog<DialogInput, DialogInputView>
    {
        public UIInput inputValue;

        public override void ShowDialog(DialogInput data)
        {
            base.ShowDialog(data);

            if (inputValue != null && string.IsNullOrEmpty(data.startValue) == false)
                inputValue.value = data.startValue;
        }

        protected override void OnPressButton(bool? pressValue, DialogInput data)
        {
            if (data.onDoneInput != null)
                data.onDoneInput(pressValue, inputValue == null ? string.Empty : inputValue.value);
        }
    }
}
