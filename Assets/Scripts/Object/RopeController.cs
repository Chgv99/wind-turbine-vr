using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;

namespace WindTurbineVR.Object
{
    [RequireComponent(typeof(LineRenderer))]
    public class RopeController : MonoBehaviour
    {
        TurbineSceneController sceneController;

        [SerializeField] LineRenderer lr;

        [SerializeField] List<Transform> joints;

        [SerializeField] GameObject jointPrefab;

        [Space]
        [SerializeField] Transform beginning;
        [SerializeField] Transform end;

        #region Params
        [Space]
        [SerializeField] Color color = new Color();

        [Space]
        [SerializeField] float airDrag = 1f;

        [Space]
        [SerializeField] [Range(0.01f, 1f)] float thickness =        0.1f; // Limitado a 0.1 por limitaciones de Unity
        [SerializeField] [Range(0.5f, 15f)] float jointDensity =     1f;
        [SerializeField] [Range(0.1f, 1f)] float densityUnit =      1f;

        [Space]
        [SerializeField][Range(0.1f, 1f)] float visualFactor = 1f; //relation between rigidbody size and linewidth

        [Space]
        [SerializeField] bool showJoints = false;
        #endregion

        bool halted;

        int numberJoints;

        #region Hidden variables
        Vector3 distance; // Vector that points from beginning to end
        //Vector3 offset;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            #region Object init

            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
            sceneController.LifelineHalt.AddListener(HaltOn);
            sceneController.LifelineRelease.AddListener(HaltOff);

            jointPrefab = Resources.Load("Rope/RopeJoint") as GameObject;

            #endregion

            #region Object error treatment
            if (beginning == null) Error.LogException("Beginning object is missing. On GameObject \"" + gameObject.name + "\".");
            if (end == null) Error.LogException("End object is missing. On GameObject \"" + gameObject.name + "\".");
            #endregion

            #region Joints

            #region Joint offset calculation
            
            distance = beginning.position - end.position;
            float magnitude = distance.magnitude;

            float offsetQ = densityUnit / jointDensity;

            numberJoints = (int)Mathf.Ceil(magnitude / offsetQ);
            numberJoints -= 1;

            offsetQ = magnitude / (numberJoints + 1);

            Vector3 offset = distance.normalized * offsetQ;
            
            #endregion

            #region Joint generation
            GenerateJoints(offset);

            /*
            joints = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childT = transform.GetChild(i);
                if (childT.gameObject.tag != "RopeJoint") continue;

                if (joints.Count > 0)
                {
                    childT.GetComponent<HingeJoint>().connectedBody =
                        joints[joints.Count - 1].GetComponent<Rigidbody>();
                }

                childT.GetComponent<SphereCollider>().radius = thickness / 2;
                childT.GetComponent<MeshRenderer>().enabled = false;

                joints.Add(childT);
            }*/
            #endregion

            #region Beginning and End configuration

            ConfigureJoint(beginning.gameObject);
            ConfigureJoint(end.gameObject);

            #endregion

            #endregion

            #region LineRenderer

            lr = GetComponent<LineRenderer>();

            lr.positionCount = joints.Count;

            lr.startWidth = thickness * visualFactor;
            lr.endWidth = thickness * visualFactor;

            lr.startColor = color;
            lr.endColor = color;

            #endregion
        }

        private void OnDestroy()
        {
            sceneController.LifelineHalt.RemoveListener(HaltOn);
            sceneController.LifelineRelease.RemoveListener(HaltOff);
        }

        void Update()
        {
            #region Line Rendering
            int i = 0;
            if (halted)
            {
                lr.positionCount = 2;
                lr.SetPosition(0, joints[0].position);
                lr.SetPosition(1, joints[joints.Count - 1].position);
            } else {
                lr.positionCount = joints.Count;
                foreach (Transform joint in joints)
                {
                    //Debug.Log("i: " + i + "; joint: " + joint + "; lr: " + lr);
                    lr.SetPosition(i, joint.position);
                    i++;
                }
            }
            
            #endregion
        }

        void GenerateJoints(Vector3 offset)
        {
            joints = new List<Transform>
            {
                beginning
            };

            for (int i = 0; i < numberJoints; i++)
            {
                GameObject jointInstance = Instantiate(jointPrefab, beginning.position - offset * (i + 1), Quaternion.identity, transform);
                
                // Set rope thickness
                jointInstance.GetComponent<SphereCollider>().radius = thickness / 2; //new Vector3(thickness, thickness, thickness);

                // Show joints
                jointInstance.transform.Find("Sphere").gameObject.SetActive(showJoints);

                // Configure joint parameters
                ConfigureJoint(jointInstance);

                if (i == 0)
                {
                    jointInstance.GetComponent<CharacterJoint>().connectedBody = beginning.GetComponent<Rigidbody>();
                } else {
                    jointInstance.GetComponent<CharacterJoint>().connectedBody =
                        joints[joints.Count - 1].GetComponent<Rigidbody>();
                }

                joints.Add(jointInstance.transform);
            }
            end.GetComponent<CharacterJoint>().connectedBody = joints[joints.Count - 1].GetComponent<Rigidbody>();
            joints.Add(end);
        }

        void ConfigureJoint(GameObject joint)
        {
            joint.layer = LayerMask.NameToLayer("RopeJoint");
            joint.GetComponent<Rigidbody>().drag = airDrag;
        }

        public void HaltOn() => halted = true;

        public void HaltOff() => halted = false;
    }
}
