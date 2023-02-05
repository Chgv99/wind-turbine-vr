using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using WindTurbineVR.Core;

namespace WindTurbineVR.Object
{
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

        List<Transform> jointList;

        private void OnValidate()
        {
            joints = Mathf.Clamp(joints, min_joints, int.MaxValue);
        }

        // Start is called before the first frame update
        void Start()
        {
            lr = GetComponent<LineRenderer>();

            jointList = new List<Transform>();

            #region rope ends setup
            
            if (beginning == null) beginning = transform.Find("Beginning");
            if (end == null) end = transform.Find("End");

            beginning.gameObject.layer = LayerMask.NameToLayer("RopeEnd");
            end.gameObject.layer = LayerMask.NameToLayer("RopeEnd");
            #region beginning components
            if (beginning.GetComponent<Rigidbody>() == null) beginning.AddComponent<Rigidbody>();
            if (beginning.GetComponent<ConfigurableJoint>() == null) beginning.AddComponent<ConfigurableJoint>();
            //if (beginning.GetComponent<SphereCollider>() == null) Error.LogException("Rope (" + gameObject + ") beginning has no Collider component");
            if (beginning.GetComponent<XRGrabInteractable>() == null) Error.LogException("Rope (" + gameObject + ") beginning has no XRGrabInteractable component");
            if (beginning.GetComponent<SphereCollider>() != null) beginning.GetComponent<SphereCollider>().radius = thickness / 2;
            #endregion
            #region end components
            if (end.GetComponent<Rigidbody>() == null) end.AddComponent<Rigidbody>();
            if (end.GetComponent<ConfigurableJoint>() == null) end.AddComponent<ConfigurableJoint>();
            //if (end.GetComponent<SphereCollider>() == null) Error.LogException("Rope (" + gameObject + ") end has no Collider component");
            if (end.GetComponent<XRGrabInteractable>() == null) Error.LogException("Rope (" + gameObject + ") end has no XRGrabInteractable component");
            if (end.GetComponent<SphereCollider>() != null) end.GetComponent<SphereCollider>().radius = thickness / 2;
            #endregion

            if (freezeBeginning) beginning.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            if (freezeEnd) end.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            #endregion
            
            #region error treatment
            /*if (beginning == null)
            {
                Debug.LogException(new Exception("Variable \"beginning\" not set to an instance of an object."));
                Debug.Break();
            }
            if (end == null)
            {
                Debug.LogException(new Exception("Variable \"end\" not set to an instance of an object."));
                Debug.Break();
            }*/
            #endregion

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
            jointList.Add(beginning);
            for (int i = 2; i < joints + 2; i++)
            {
                GameObject newJoint = Instantiate(joint, beginning.position - offset * (i - 1), Quaternion.identity, transform);
                newJoint.GetComponent<SphereCollider>().radius = thickness / 2;
                jointList.Add(newJoint.transform);
                newJoint.name = "Joint " + (i - 2);
                newJoint.gameObject.layer = LayerMask.NameToLayer("RopeJoint");
                ConfigurableJoint cj = newJoint.transform.GetComponent<ConfigurableJoint>();

                cj.connectedBody = transform.GetChild(i - 1).GetComponent<Rigidbody>();
                cj.xMotion = ConfigurableJointMotion.Locked;
                cj.yMotion = ConfigurableJointMotion.Locked;
                cj.zMotion = ConfigurableJointMotion.Locked;

                if (i == 2) //First new joint needs to connect to beginning joint
                {
                    cj.connectedBody = beginning.GetComponent<Rigidbody>();
                }

                if (i == joints + 1) //Last from new joints needs to connect to end joint
                {
                    newJoint.AddComponent<ConfigurableJoint>();
                    ConfigurableJoint[] cjs = newJoint.transform.GetComponents<ConfigurableJoint>();
                    cjs[1].connectedBody = transform.Find("End").GetComponent<Rigidbody>();
                    cjs[1].xMotion = ConfigurableJointMotion.Locked;
                    cjs[1].yMotion = ConfigurableJointMotion.Locked;
                    cjs[1].zMotion = ConfigurableJointMotion.Locked;
                }

                /*if (i == joints + 1) //Last new joint needs to connect to end joint
                {
                    newJoint.AddComponent<ConfigurableJoint>();
                    cj = newJoint.transform.GetComponents<ConfigurableJoint>();
                    cj[1].connectedBody = transform.Find("End").GetComponent<Rigidbody>();
                    cj[1].xMotion = ConfigurableJointMotion.Locked;
                    cj[1].yMotion = ConfigurableJointMotion.Locked;
                    cj[1].zMotion = ConfigurableJointMotion.Locked;
                }
                else
                {
                    cj = newJoint.transform.GetComponents<ConfigurableJoint>();
                }*/

            }
            jointList.Add(end);
            end.SetSiblingIndex(transform.childCount);
            end.transform.GetComponent<ConfigurableJoint>().connectedBody = transform.GetChild(transform.childCount - 2).GetComponent<Rigidbody>();
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

            // New Behaviour (takes list of stored joints)
            int i = 0;
            foreach (Transform joint in jointList)
            {
                lr.SetPosition(i, joint.position);
                i++;
            }

            /* Old behaviour (only takes current children of transform
            int i = 0;
            foreach (Transform joint in transform)
            {
                lr.SetPosition(i, joint.position);
                i++;
            }
            */
            #endregion
        }
    }
}