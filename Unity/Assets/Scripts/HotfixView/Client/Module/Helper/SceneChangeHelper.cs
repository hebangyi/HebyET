

using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace ET.Client
{
    public static partial class SceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene root, UnitySceneEnum unitySceneEnum, params object[] ParamList)
        {
            try
            {
                UnitySceneManagerComponent.UnitySceneChangeContext unitySceneChangeContext = new ();
                unitySceneChangeContext.unitySceneEnum = unitySceneEnum;
                unitySceneChangeContext.ParamList = ParamList;
                
                UnitySceneManagerComponent.Instance.UnitySceneChangeQueue.Enqueue(unitySceneChangeContext);
            }
            catch (Exception e)
            {
                Log.Error("切换场景中途异常");
                Log.Error(e);

                SceneChangeTo(root, UnitySceneEnum.Lobby).Coroutine();
            }
            
        }
        
        
        

    }
}