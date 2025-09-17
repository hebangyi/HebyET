using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [ClientWorldEventHandler]
    public class CreateUnitEntityEvent1_ClientUnitEntityGameObject: AClientWorldEvent<ClientUnitEntityGameObject>
    {
        protected override async ETTask Run(ClientWorld world, ClientUnitEntityGameObject args)
        {
            await GameObjectHelper.CreateGameObjectIns(args.UnitEntity.ClientWorld(), args.UnitEntity);
        }
        
    }
}
