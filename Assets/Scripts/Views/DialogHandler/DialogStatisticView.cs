// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Puppet.Service;
using Puppet;


[PrefabAttribute(Name = "Prefabs/Dialog/UserInfo/DialogStatistics", Depth = 8, IsAttachedToCamera = true, IsUIPanel = true)]
public class DialogStatisticView : BaseDialog<DialogStatistic,DialogStatisticView>
{

}
public class DialogStatistic : AbstractDialogData{

	public DialogStatistic() : base(){
	
	}
	public override void ShowDialog ()
	{
		DialogStatisticView.Instance.ShowDialog (this);
	}
}

