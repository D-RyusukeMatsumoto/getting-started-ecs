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
            float startTime = Time.realtimeSinceStartup;


            if (useJobs)
            {
                //NativeArray<float3> positionArray = new NativeArray<float3>(ieinuList.Count, Allocator.TempJob);
                NativeArray<float> moveYArray = new NativeArray<float>(ieinuList.Count, Allocator.TempJob);
                TransformAccessArray transformAccessArray = new TransformAccessArray(ieinuList.Count);
                
                for (var i = 0; i < ieinuList.Count; ++i)
                {
                    //positionArray[i] = ieinuList[i].transform.position;
                    moveYArray[i] = ieinuList[i].moveY;
                    transformAccessArray.Add(ieinuList[i].transform);
                }

/*
                ReallyToughParallelJob reallyToughParallelJob = new ReallyToughParallelJob
                {
                    deltaTime = Time.deltaTime,
                    positionArray = positionArray,
                    moveYArray = moveYArray,
                };
                JobHandle jobHandle = reallyToughParallelJob.Schedule(ieinuList.Count, 100);
                jobHandle.Complete();
*/

                ReallyToughParallelJobTransforms reallyToughParallelJobTransforms = new ReallyToughParallelJobTransforms
                {
                    deltaTime = Time.deltaTime,
                    moveYArray = moveYArray,
                };
                JobHandle jobHandle = reallyToughParallelJobTransforms.Schedule(transformAccessArray);
                jobHandle.Complete();                


                for (var i = 0; i < ieinuList.Count; ++i)
                {
                    //ieinuList[i].transform.position = positionArray[i];
                    ieinuList[i].moveY = moveYArray[i];
                }
                
                //positionArray.Dispose();
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
/*
            if (useJobs)
            {
                NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
                for (var i = 0; i < 10; ++i)
                {
                    JobHandle jobHandle = this.ReallyToughTaskJob();
                    jobHandleList.Add(jobHandle);
                }
                JobHandle.CompleteAll(jobHandleList);
                jobHandleList.Dispose();
            }
            else
            {
                for (var i = 0; i < 10; ++i)
                {
                    this.ReallyToughTask();                
                }
            }
*/
            //Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
        }


        void ReallyToughTask()
        {
            float value = 0f;
            for (var i = 0; i < 50000; ++i)
            {
                value = math.exp10(math.sqrt(value));
            }
        }


        JobHandle ReallyToughTaskJob()
        {
            ReallyToughJob job = new ReallyToughJob();
             return job.Schedule();
        }
        
        
    }


    [BurstCompile]
    public struct ReallyToughJob : IJob
    {
        public void Execute()
        {
            float value = 0f;
            for (var i = 0; i < 50000; ++i)
            {
                value = math.exp10(math.sqrt(value));
            }
        }
    }


    [BurstCompile]
    public struct ReallyToughParallelJob : IJobParallelFor
    {

        public NativeArray<float3> positionArray;
        public NativeArray<float> moveYArray;
        [ReadOnly] public float deltaTime;
        
        
        public void Execute(
            int index)
        {
            positionArray[index] += new float3(0, moveYArray[index] * deltaTime, 0f);
            if (5f < positionArray[index].y)
            {
                moveYArray[index] = -math.abs(moveYArray[index]);
            }
            else if (positionArray[index].y < -5f)
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




















































