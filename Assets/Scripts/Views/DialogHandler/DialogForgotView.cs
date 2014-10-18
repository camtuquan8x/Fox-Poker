using UnityEngine;
using System.Collections;
using System;


namespace Puppet.Service
{
	[PrefabAttribute(Name = "Prefabs/Dialog/DialogForgotPassword", Depth = 7, IsAttachedToCamera = true, IsUIPanel = true)]
	public class DialogForgotView : BaseDialog<DialogForgot,DialogForgotView> {
		public UIInput inputValue;
		
		public override void ShowDialog(DialogForgot data)
		{
			base.ShowDialog(data);
			if (inputValue != null && string.IsNullOrEmpty(data.startValue) == false)
				inputValue.value = data.startValue;
		}
		
		protected override void OnPressButton(bool? pressValue, DialogForgot data)
		{
			if (data.onDoneInput != null)
				data.onDoneInput(pressValue, inputValue == null ? string.Empty : inputValue.value);
		}
	}
	public class DialogForgot : AbstractDialogData
	{
		public DialogForgot(Action<bool?, string> onDoneInput): base(){
			this.onDoneInput = onDoneInput;
		}
		public string startValue;
		public Action<bool?, string> onDoneInput;
		public override void ShowDialog()
		{
			DialogForgotView.Instance.ShowDialog(this);
		}
	}
}
