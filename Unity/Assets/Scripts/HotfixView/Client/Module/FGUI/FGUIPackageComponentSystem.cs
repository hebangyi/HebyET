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
            if (type == typeof(Texture))
            {
                string path = ABPathHelper.GetFGUIPicPath($"{item.owner.name}/{name}{extension}");
                LoadPicture(item, path).Coroutine();
            }
            /*else if (type == typeof(AudioClip))
            {
                //目前不再使用这种音效播放方式
                return;
                string path = ABPathHelper.GetFGUIAudioPathWithoutEx($"{item.owner.name}/{item.name}{extension}");

                AudioClip audioClip = resourcesComponent.LoadFuiAudioClip(path);
                if (audioClip == null)
                {
                    return;
                }

                item.owner.SetItemAsset(item, audioClip, DestroyMethod.Custom);
            }*/
        }

        public static async ETTask LoadPicture(PackageItem item, string path)
        {
            var texture = await ResourcesComponent.Instance.LoadAssetAsync<Texture>(path);
            item.owner.SetItemAsset(item, texture, DestroyMethod.Custom);
        }
    }
}