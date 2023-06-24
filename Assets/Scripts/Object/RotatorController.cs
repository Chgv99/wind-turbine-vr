using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

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

        // Apply difference in angle to wheel
        float angleDifference = currentAngle - totalAngle;
        handleTransform.Rotate(transform.forward, -angleDifference, Space.Self);

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
            Vector2 direction = FindLocalPoint(interactor.transform.position);
            totalAngle += ConvertToAngle(direction) * FindRotationSensitivity();
        }

        return totalAngle;
    }

    private Vector2 FindLocalPoint(Vector3 position)
    {
        // Convert the hand positions to local, so we can find the angle easier
        return transform.InverseTransformPoint(position).normalized;
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