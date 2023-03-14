using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class JobHandler : MonoBehaviour
{
    // Start is called before the first frame update
    struct VelocityJob : IJob
    {
        // Jobs declare all data that will be accessed in the job
        // By declaring it as read only, multiple jobs are allowed to access the data in parallel
        [ReadOnly]
        public Vector3 velocity;

        // The code actually running on the job
        public void Execute()
        {

        }
    }


}
