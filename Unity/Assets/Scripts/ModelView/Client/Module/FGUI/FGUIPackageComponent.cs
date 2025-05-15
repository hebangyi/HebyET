using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class FGUIPackageComponent: Entity, IAwake
    {
        public const string FUI_PACKAGE_DIR = "Assets/Data/FGUI";
        
        private readonly Dictionary<string, int> _permanentPackages = new Dictionary<string, int>();
        
        private readonly Dictionary<string, AssetRequest> _loadedPackages = new Dictionary<string, AssetRequest>();
        
        
        
    }
}