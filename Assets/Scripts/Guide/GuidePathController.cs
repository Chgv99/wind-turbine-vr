using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Guide
{
    public class GuidePathController : MonoBehaviour
    {
        [SerializeField] LineRenderer lr;

        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log(gameObject.name + "is awake");
            lr = GetComponent<LineRenderer>();
            lr.positionCount = transform.childCount;

            GameObject cylinder = Resources.Load("WaypointCylinder") as GameObject;

            int index = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Debug.Log(child.gameObject.name);
                //if (child.gameObject.name != "GuideWaypoint") continue;
                lr.SetPosition(index++, child.position);

                child.gameObject.SetActive(false);

                if (i == 0) Instantiate(cylinder, child);
                if (i == transform.childCount - 1) Instantiate(cylinder, child);
            }
        }

        public void Enable()
        {
            lr.enabled = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public void Disable()
        {
            lr.enabled = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
