using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Chapter1
{
    
    public class MoverSystem : ComponentSystem
    {
    
    
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref MoveSpeedComponent moveSpeedComponent) =>
            {
                translation.Value.y += moveSpeedComponent.moveSpeed * Time.deltaTime;
                if (5f < translation.Value.y)
                    moveSpeedComponent.moveSpeed = -math.abs(moveSpeedComponent.moveSpeed);

                if (translation.Value.y < -5f)
                    moveSpeedComponent.moveSpeed = +math.abs(moveSpeedComponent.moveSpeed);

            });    
        }
    }
}

