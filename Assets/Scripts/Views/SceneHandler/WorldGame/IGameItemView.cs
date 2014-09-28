﻿using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface IGameItemView
{
    void ShowImage(Texture2D texutre);
    void OnJoinRoom(DataGame data);
}

