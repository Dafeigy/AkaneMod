using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using UnityEngine;

namespace AkaneMod
{
    [BepInPlugin("cn.cybershit.plugin.AkaneMod", "AkaneMod","1.0.0")]
    public class AkaneMod : BaseUnityPlugin
    {
        void Start()
        {
            Logger.LogInfo("Hello wolrd from Logger."); // Better use this to print log.
        }
        
    }
}
