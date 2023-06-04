using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Guide
{
    public class GuidePathController : MonoBehaviour
    {
        [SerializeField] LineRenderer lr;

        // Start is called before the first frame update
        void Start()
        {
            lr = GetComponent<LineRenderer>();
            lr.positionCount = transform.childCount;

            int index = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Debug.Log(child.gameObject.name);
                //if (child.gameObject.name != "GuideWaypoint") continue;
                lr.SetPosition(index++, child.position);
            }
        }

        public void Enable() => lr.enabled = true;

        public void Disable() => lr.enabled = false;

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
