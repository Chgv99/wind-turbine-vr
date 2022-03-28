using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGeneratorController : MonoBehaviour
{
    #region Setup objects

    [SerializeField] private GameObject joint;
    [SerializeField] private Transform beginning;
    [SerializeField] private Transform end;

    private LineRenderer lr;

    private Vector3 distance; //Vector that points from beginning to end joint
    private Vector3 offset; //Joint offset at initialization
    private float lrSize; //Line renderer nodes

    #endregion

    #region Params

    [SerializeField] bool freezeBeginning = true;
    [SerializeField] bool freezeEnd = true;
    [SerializeField] int joints = 0;
    [SerializeField] float thickness = 0.5f;
    //[SerializeField] float jointDistance = 0.5f;
    int min_joints = 5;

    #endregion

    private void OnValidate()
    {
        joints = Mathf.Clamp(joints, min_joints, int.MaxValue);
    }

    // Start is called before the first frame update
    void Start()
    {
        beginning = transform.Find("Beginning");
        end = transform.Find("End");
        lr = GetComponent<LineRenderer>();

        if (freezeBeginning) beginning.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        if (freezeEnd) end.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        distance = beginning.position - end.position;
        lr.positionCount = joints;
        lr.SetWidth(thickness, thickness);
        joints -= 2; //Beginning and end are implicit

        float magnitude = distance.magnitude / (joints + 1);
        //if (jointDistance > magnitude) magnitude = jointDistance;
        //Debug.Log("JointDistance = " + jointDistance);
        //Debug.Log("Magnitude = " + magnitude);

        offset = distance.normalized * magnitude;

        #region Joint Generation
        for (int i = 2; i < joints + 2; i++)
        {
            GameObject newJoint = Instantiate(joint, beginning.position - offset * (i - 1), Quaternion.identity, transform);
            ConfigurableJoint[] cj;
            if (i == joints + 1) //Last from new joints needs to connect to end joint
            {
                newJoint.AddComponent<ConfigurableJoint>();
                cj = newJoint.transform.GetComponents<ConfigurableJoint>();
                cj[1].connectedBody = transform.Find("End").GetComponent<Rigidbody>();
                cj[1].xMotion = ConfigurableJointMotion.Locked;
                cj[1].yMotion = ConfigurableJointMotion.Locked;
                cj[1].zMotion = ConfigurableJointMotion.Locked;
            } else {
                cj = newJoint.transform.GetComponents<ConfigurableJoint>();
            }
            cj[0].connectedBody = transform.GetChild(i-1).GetComponent<Rigidbody>();
            cj[0].xMotion = ConfigurableJointMotion.Locked;
            cj[0].yMotion = ConfigurableJointMotion.Locked;
            cj[0].zMotion = ConfigurableJointMotion.Locked;
        }
        end.SetSiblingIndex(transform.childCount);
        end.transform.GetComponent<ConfigurableJoint>().connectedBody = transform.GetChild(transform.childCount-2).GetComponent<Rigidbody>();
        #endregion
        //Debug.Break();
    }

    Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

    // Update is called once per frame
    void Update()
    {
        #region Line Rendering

        int i = 0;
        foreach (Transform joint in transform)
        {
            lr.SetPosition(i, joint.position);
            i++;
        }

        #endregion
    }
}
