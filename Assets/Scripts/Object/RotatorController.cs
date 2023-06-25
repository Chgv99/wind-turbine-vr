using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using WindTurbineVR.Core;

public class RotatorController : XRBaseInteractable
{
    [SerializeField] private Transform handleTransform;

    public UnityEvent<float> OnHandleRotated;

    private float currentAngle = 0.0f;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("SELECTED");
        base.OnSelectEntered(args);
        currentAngle = FindHandleAngle();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("UNSELECTED");
        base.OnSelectExited(args);
        currentAngle = FindHandleAngle();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
                RotateHandle();
        }
    }

    private void RotateHandle()
    {
        // Convert that direction to an angle, then rotation
        float totalAngle = FindHandleAngle();
        Debug.Log("current angle: " + currentAngle);
        Debug.Log("total angle: " + totalAngle);

        // Apply difference in angle to wheel
        float angleDifference = currentAngle - totalAngle;
        handleTransform.Rotate(transform.up, -angleDifference, Space.Self);

        // Store angle for next process
        currentAngle = totalAngle;
        OnHandleRotated?.Invoke(angleDifference);
    }

    private float FindHandleAngle()
    {
        float totalAngle = 0;

        // Combine directions of current interactors
        foreach (IXRSelectInteractor interactor in interactorsSelecting)
        {
            Debug.DrawRay(transform.position, transform.up, Color.cyan, 1);
            Debug.DrawRay(transform.position, transform.forward, Color.green, 1);
            //Debug.Log("interactor gameobject name: " + interactor.transform.gameObject.name);
            //Debug.Log("interactor transform position: " + interactor.transform.position.ToString());
            //Debug.DrawRay(transform.position, interactor.transform.position * Vector3.Distance(transform.position, interactor.transform.position), Color.white, 1); //GameObject.Find("SceneController").GetComponent<TurbineSceneController>().xrOrigin.position
            //Debug.Log("interactor world coords: " + interactor.transform.position.ToString());
            Vector3 localDirection = FindLocalPoint(interactor.transform.position);
            //Debug.DrawRay(transform.position, transform.InverseTransformPoint(interactor.transform.position) * Vector3.Distance(transform.position, transform.InverseTransformPoint(interactor.transform.position)), Color.yellow, 1);
            Debug.DrawRay(transform.position, localDirection, Color.yellow, 1);
            //Debug.Log("interactor local coords: " + localDirection.ToString());
            Vector3 flatDirection = new Vector3(localDirection.x, 0, localDirection.z);
            Debug.DrawRay(transform.position, flatDirection, Color.red, 1);
            //Debug.Log("interactor world flat coords: " + flatPosition.ToString());
            //Vector2 flatDirection = 
            totalAngle += ConvertToAngle(flatDirection) * FindRotationSensitivity();
        }
        Debug.Log("total angle: " + totalAngle);
        return totalAngle;
    }

    private Vector3 FindLocalPoint(Vector3 position)
    {
        // Convert the hand positions to local, so we can find the angle easier
        //return transform.InverseTransformPoint(position).normalized;
        return position - transform.position;
    }

    private float ConvertToAngle(Vector3 direction)
    {
        // Use a consistent forward direction to find the angle
        return Vector3.SignedAngle(transform.forward, direction, transform.up);
    }

    private float ConvertToAngle(Vector2 direction)
    {
        // Use a consistent up direction to find the angle
        return Vector2.SignedAngle(Vector2.up, direction);
    }

    private float FindRotationSensitivity()
    {
        // Use a smaller rotation sensitivity with two hands
        return 1.0f / interactorsSelecting.Count;
    }
}