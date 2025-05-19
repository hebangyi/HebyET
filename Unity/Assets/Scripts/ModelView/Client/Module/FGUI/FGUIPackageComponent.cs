using System;
using System.Collections.Generic;
using System.Reflection;
using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    // 
    [ComponentOf]
    [EnableMethod]
    public class FGUIPackageComponent: Entity, IAwake
    {
        public static FGUIPackageComponent Instance;

        public const string FUI_PACKAGE_DIR = "Assets/Data/FGUI";
        
        // 常驻内存
        public readonly HashSet<string> PermanentPackages = new HashSet<string>();
        
        // TODO 内存优化
        // temp内存
        // private readonly Dictionary<string, int> _temporaryPackages = new Dictionary<string, int>();
        
        public readonly Dictionary<string, TextAsset> LoadedPackages = new Dictionary<string, TextAsset>();

    }
}