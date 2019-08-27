using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;


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
            typeof(LocalToWorld));

        NativeArray<Entity> entityArray = new NativeArray<Entity>(2000, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);
        for (var i = 0; i < entityArray.Length; ++i)
        {
            Entity entity = entityArray[i];
            entityManager.SetComponentData(entity, new LevelComponent{level = 10});
            
            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
            });
        }
        entityArray.Dispose();
        
    }

    
}
