using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Rendering;


namespace Chapter1
{
    public class Testing : MonoBehaviour
    {

        [SerializeField] Mesh mesh;
        [SerializeField] Material material;
    
    
        void Start()
        {
            EntityManager entityManager = World.Active.EntityManager;

            EntityArchetype entityArchetype = entityManager.CreateArchetype(
                typeof(LevelComponent),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(MoveSpeedComponent));

            NativeArray<Entity> entityArray = new NativeArray<Entity>(10000, Allocator.Temp);
            entityManager.CreateEntity(entityArchetype, entityArray);
            for (var i = 0; i < entityArray.Length; ++i)
            {
                Entity entity = entityArray[i];
                entityManager.SetComponentData(entity, 
                    new LevelComponent
                    {
                        level = 10
                    });
                entityManager.SetComponentData(entity, 
                    new MoveSpeedComponent
                    {
                        moveSpeed = UnityEngine.Random.Range(1f, 2f)
                    });
                entityManager.SetComponentData(entity, 
                    new Translation
                    {
                        Value = new float3(UnityEngine.Random.Range(-8f, 8f),UnityEngine.Random.Range(-5f, 5f), 0)
                    });
            
                entityManager.SetSharedComponentData(entity, new RenderMesh
                {
                    mesh = mesh,
                    material = material,
                });
            }
            entityArray.Dispose();
        
        }

    
    }
    
}

