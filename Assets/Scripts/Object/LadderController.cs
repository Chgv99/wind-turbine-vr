using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Object
{
    public class LadderController : MonoBehaviour
    {
        [SerializeField] GameObject stepPrefab;

        Transform cylinder_one;
        Transform cylinder_two;

        #region vars
        [SerializeField] bool truncated;
        [SerializeField] float spacing = 0.3f;
        //[SerializeField] float offset = 0f;
        [SerializeField] float stepCount = 20;
        [Space]
        [SerializeField] float width = 0.4f;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            cylinder_one = transform.Find("Cylinder");
            cylinder_two = transform.Find("Cylinder (2)");

            for (int i = 0; i < stepCount; i++)
            {
                GameObject step = Instantiate(stepPrefab, new Vector3(transform.position.x, spacing * (i + (truncated ? 1 : 0)), transform.position.z), Quaternion.identity, transform.Find("Steps"));
                step.transform.localScale = new Vector3(width, step.transform.localScale.y, step.transform.localScale.z);
                //cylinder_one.position = new Vector3(transform.position.x + width / 2, cylinder_one.position.y, cylinder_one.position.z);
                //cylinder_two.position = new Vector3(transform.position.x - width / 2, cylinder_two.position.y, cylinder_two.position.z);

                cylinder_one.localPosition = new Vector3(width / 2, 0, 0);
                cylinder_two.localPosition = new Vector3(-width / 2, 0, 0);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
