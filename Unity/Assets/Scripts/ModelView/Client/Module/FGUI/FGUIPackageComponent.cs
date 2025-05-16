using System;
using System.Collections.Generic;
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

        public void Awake()
        {
            this.PermanentPackages.Clear();
            this.LoadedPackages.Clear();
            Instance = this;
        }
        
        public async ETTask AddPackageSync(string type)
        {
            var textAsset = this.LoadedPackages.GetValueOrDefault(type);
            if (textAsset != null)
            {
                return;
            }
            
            var descPath = ABPathHelper.GetFGUIDescPath($"{type}/{type}_fui");
            textAsset = await ResourcesComponent.Instance.LoadAssetAsync<TextAsset>(descPath);

            if (textAsset == null)
            {
                return;
            }

            PermanentPackages.Add(type);
            LoadedPackages[type] = textAsset;
            UIPackage.AddPackage(textAsset.bytes, type, LoadPackageInternal);
        }
        
        public void RemovePackage(string type)
        {
            PermanentPackages.Remove(type);
            LoadedPackages.Remove(type);
        }
        
        
        private void LoadPackageInternal(string name, string extension, Type type, PackageItem item)
        {
        }
    }
}