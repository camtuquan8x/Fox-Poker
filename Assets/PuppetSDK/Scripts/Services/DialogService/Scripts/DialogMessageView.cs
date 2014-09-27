using Puppet;
using Puppet.Service;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet.Service
{
    [PrefabAttribute(Name = "Prefabs/Dialog/DialogMessage", Depth = 10, IsAttachedToCamera = true, IsUIPanel = true)]
    public class DialogMessageView : BaseDialog<DialogMessage, DialogMessageView>
    {
    }
}
