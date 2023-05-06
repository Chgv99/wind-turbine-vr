using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Object
{
    public class LadderController : MonoBehaviour
    {
        [SerializeField] GameObject stepPrefab;

        #region private
        float spacing = 0.3f;
        float stepCount = 20;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < stepCount; i++)
            {
                Instantiate(stepPrefab, new Vector3(transform.position.x, spacing * i, transform.position.z), Quaternion.identity, transform.Find("Steps"));
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
