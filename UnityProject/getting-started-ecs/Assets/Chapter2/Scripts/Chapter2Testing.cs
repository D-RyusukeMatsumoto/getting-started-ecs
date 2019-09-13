using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using UnityEngine.Jobs;


namespace Chapter2
{
    /// <summary>
    /// Chapter2のテストスクリプト.
    /// </summary>
    public class Chapter2Testing : MonoBehaviour
    {

        
        [SerializeField] bool useJobs = false;
        [SerializeField] Transform IeinuPrefab;
        List<Ieinu> ieinuList;        

        public class Ieinu
        {
            public Transform transform;
            public float moveY;
        }

        
        void Start()
        {
            ieinuList = new List<Ieinu>();
            for (var i = 0; i < 1000; ++i)
            {
                Transform ieinuTransform = Instantiate(IeinuPrefab, new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-5f, 5f)), Quaternion.identity);
                ieinuList.Add(
                    new Ieinu
                    {
                        transform = ieinuTransform,
                        moveY = UnityEngine.Random.Range(1f, 2f)
                    });
            }
        }
        
        
        void Update()
        {
            if (useJobs)
            {
                NativeArray<float> moveYArray = new NativeArray<float>(ieinuList.Count, Allocator.TempJob);
                TransformAccessArray transformAccessArray = new TransformAccessArray(ieinuList.Count);
                
                for (var i = 0; i < ieinuList.Count; ++i)
                {
                    moveYArray[i] = ieinuList[i].moveY;
                    transformAccessArray.Add(ieinuList[i].transform);
                }

                ReallyToughParallelJobTransforms reallyToughParallelJobTransforms = new ReallyToughParallelJobTransforms
                {
                    deltaTime = Time.deltaTime,
                    moveYArray = moveYArray,
                };
                JobHandle jobHandle = reallyToughParallelJobTransforms.Schedule(transformAccessArray);
                jobHandle.Complete();                

                for (var i = 0; i < ieinuList.Count; ++i)
                {
                    ieinuList[i].moveY = moveYArray[i];
                }
                
                moveYArray.Dispose();
                transformAccessArray.Dispose();
            }
            else
            {
                foreach (var ieinu in ieinuList)
                {
                    ieinu.transform.position += new Vector3(0, ieinu.moveY * Time.deltaTime);
                    if (5f < ieinu.transform.position.y)
                    {
                        ieinu.moveY = -math.abs(ieinu.moveY);
                    }
                    else if (ieinu.transform.position.y < -5f)
                    {
                        ieinu.moveY = +math.abs(ieinu.moveY);
                    }

                    float value = 0f;
                    for (var i = 0; i < 1000; ++i)
                    {
                        value = math.exp10(math.sqrt(value));
                    }
                }
            
            }
        }
    }


    [BurstCompile]
    public struct ReallyToughParallelJobTransforms : IJobParallelForTransform
    {

        public NativeArray<float> moveYArray;
        [ReadOnly] public float deltaTime;
        
        public void Execute(
            int index,
            TransformAccess transform)
        {
            transform.position += new Vector3(0, moveYArray[index] * deltaTime, 0f);
            if (5f < transform.position.y)
            {
                moveYArray[index] = -math.abs(moveYArray[index]);
            }
            else if (transform.position.y < -5f)
            {
                moveYArray[index] = +math.abs(moveYArray[index]);
            }
            
            float value = 0f;
            for (var i = 0; i < 1000; ++i)
            {
                value = math.exp10(math.sqrt(value));
            }
        }
        
        
    }
    
    
    
    
    
    
    
}




















































