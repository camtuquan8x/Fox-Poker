using UnityEngine;
using System.Collections;

namespace Puppet.Service{
	[PrefabAttribute(Name = "Prefabs/Dialog/DialogSettings", Depth = 7, IsAttachedToCamera = true, IsUIPanel = true)]
	public class DialogSettingsView : BaseDialog<DialogSetting,DialogSettingsView> {
		#region Unity Editor
		public UILabel lbUserName, lbVersion;
		public GameObject btnLogout,toggleAutoSit,toggleAutoBuy,togglePlayNotification,toggleGameNotification;
		#endregion
		public override void ShowDialog (DialogSetting data)
		{
			base.ShowDialog (data);
			InitData (data);
		}
		protected override void OnEnable(){
			base.OnEnable();
			UIEventListener.Get (btnLogout).onClick += onBtnLogoutClick;
			UIEventListener.Get (toggleAutoSit).onClick += onToggleAutoSit;
			UIEventListener.Get (toggleAutoBuy).onClick += onToggleAutoBuy;
			UIEventListener.Get (togglePlayNotification).onClick += onTogglePlayNotification;
			UIEventListener.Get (toggleGameNotification).onClick += onToggleGameNotification;
			
		}
		protected override void  OnDisable(){
			base.OnDisable();
			UIEventListener.Get (btnLogout).onClick -= onBtnLogoutClick;
			UIEventListener.Get (toggleAutoSit).onClick -= onToggleAutoSit;
			UIEventListener.Get (toggleAutoBuy).onClick -= onToggleAutoBuy;
			UIEventListener.Get (togglePlayNotification).onClick -= onTogglePlayNotification;
			UIEventListener.Get (toggleGameNotification).onClick -= onToggleGameNotification;
		}
		void InitData (DialogSetting data)
		{
			this.lbUserName.text = data.userName;
			this.lbVersion.text = data.version;

		}

		void onBtnLogoutClick (GameObject go)
		{
			
		}

		void onToggleAutoSit (GameObject go)
		{
			if (go.GetComponent<UISlider> ().value == 0)
				go.GetComponent<UISlider> ().value = 1;
			else
				go.GetComponent<UISlider> ().value = 0;

		}

		void onToggleAutoBuy (GameObject go)
		{
			if (go.GetComponent<UISlider> ().value == 0)
				go.GetComponent<UISlider> ().value = 1;
			else
				go.GetComponent<UISlider> ().value = 0;
		}

		void onTogglePlayNotification (GameObject go)
		{
			if (go.GetComponent<UISlider> ().value == 0)
				go.GetComponent<UISlider> ().value = 1;
			else
				go.GetComponent<UISlider> ().value = 0;
		}

		void onToggleGameNotification (GameObject go)
		{
			if (go.GetComponent<UISlider> ().value == 0)
				go.GetComponent<UISlider> ().value = 1;
			else
				go.GetComponent<UISlider> ().value = 0;
		}
	}
	public class DialogSetting : AbstractDialogData {
		public string userName;
		public string version;
		public DialogSetting(string userName,string version) : base(){
			this.userName = userName;
			this.version = version;
		}
		public override void ShowDialog ()
		{
			DialogSettingsView.Instance.ShowDialog (this);
		}
	}
}
