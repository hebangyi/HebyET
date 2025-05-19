using System;
using System.Collections.Generic;
using System.Reflection;
using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIPackageComponent))]
    [FriendOf(typeof(FGUIPackageComponent))]
    public static partial class FGUIPackageComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FGUIPackageComponent self)
        {
            FGUIPackageComponent.Instance = self;
            self.PermanentPackages.Clear();
            self.LoadedPackages.Clear();
        }

        public static async ETTask TryAddPackageAsync(this FGUIPackageComponent self, string package)
        {
            var textAsset = self.LoadedPackages.GetValueOrDefault(package);
            if (textAsset != null)
            {
                return;
            }

            var descPath = ABPathHelper.GetFGUIDescPath($"{package}/{package}_fui");
            textAsset = await ResourcesComponent.Instance.LoadAssetAsync<TextAsset>(descPath);

            if (textAsset == null)
            {
                return;
            }

            self.PermanentPackages.Add(package);
            self.LoadedPackages[package] = textAsset;
            UIPackage.AddPackage(textAsset.bytes, package, self.LoadPackageInternal);
        }

        public static void RemovePackage(this FGUIPackageComponent self, string type)
        {
            self.PermanentPackages.Remove(type);
            self.LoadedPackages.Remove(type);
        }

        private static void LoadPackageInternal(this FGUIPackageComponent self, string name, string extension, Type type, PackageItem item)
        {
        }
    }
}