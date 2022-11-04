using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public static PCLogic selectedPC;
    public PointOfInterest currentPOI;
    public Rotation rotation = Rotation.Forward;
    [SerializeField]
    private bool showGizmo = false;
    [Header("Toggles")]
    public bool rotateHeadWSAD = true;
    [SerializeField]
    [ReadOnly]
    private bool _enableMouseMovement = true;
    public bool enableMouseMovement
    {
        set
        {
            ToggleMouseMovement(value);
        }
        get
        {
            return _enableMouseMovement;
        }
    }

    [Button]
    private void EnableMouseMovement()
    {
        enableMouseMovement = true;
    }
    [Button]
    private void DisableMouseMovement()
    {
        enableMouseMovement = false;
    }


    public void ToggleMouseMovement(bool? state = null)
    {
        if (state == null)
        {
            ToggleMouseMovement(!_enableMouseMovement);
        }
        else
        {
            _enableMouseMovement = state.Value;
            MouseMovementController.instance.ToggleMouseMovement(state.Value);

        }
    }

    private void Start()
    {
        currentPOI?.Interact(this);
    }
    private void Update()
    {
        if (selectedPC == null)
        {
            HandleMovement();
        }
    }
    /*todo 0 implement:
    add offset?
    todo 7 add focus poi
        todo 7 add poi generator

    todo 8 add other poi
    todo 9 test pois
    */
    public void HandleMovement()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            GoForwards();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GoLeft();

        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            GoBackwards();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            GoRight();
        }


    }
    public void MoveTo(PointOfInterest pom)
    {
        currentPOI = pom;
        transform.position = currentPOI.transform.position;
        MouseMovementController.instance.ToggleAllDirectionButtons(this);
    }
    public void Rotate(Rotation rot)
    {
        switch (rot)
        {
            case Rotation.Left:
                transform.Rotate(0, -90, 0);
                rotation = rotation.RotateLeft();
                break;
            case Rotation.Right:
                transform.Rotate(0, 90, 0);
                rotation = rotation.RotateRight();
                break;
            case Rotation.Backward:
                transform.Rotate(0, 180, 0);
                rotation = rotation.RotateRight().RotateRight();
                break;
        }
        MouseMovementController.instance.ToggleAllDirectionButtons(this);


    }

    public void GoForwards()
    {
        currentPOI.GetPoi(rotation)?.Interact(this);
    }
    public void GoLeft()
    {
        if (rotateHeadWSAD)
        {
            Rotate(Rotation.Left);
        }
        else
        {
            var poi = currentPOI.GetPoi(rotation.RotateLeft());
            if (poi != null)
            {
                rotation = rotation.RotateLeft();
                poi.Interact(this);
            }
        }
    }
    public void GoRight()
    {
        if (rotateHeadWSAD)
        {
            Rotate(Rotation.Right);
        }
        else
        {
            var poi = currentPOI.GetPoi(rotation.RotateRight());
            if (poi != null)
            {
                rotation = rotation.RotateRight();
                poi.Interact(this);
            }
        }
    }
    public void GoBackwards()
    {
        Debug.Log("Go Backwards");
        //THE QUESTION: do you rotate twice right or left to go backwards?
        var poi = currentPOI.GetPoi(rotation.RotateRight().RotateRight());
        if (poi != null)
        {
            poi.Interact(this);
        }
    }


    public void OnDrawGizmos()
    {
        if (showGizmo)
        {
            OnDrawGizmosSelected();
        }
    }
    public void OnDrawGizmosSelected()
    {
        currentPOI.OnDrawGizmosSelected();
        Handles.color = Color.white;




        Handles.ArrowHandleCap(
        0,
        transform.position,
        Quaternion.LookRotation(transform.forward, Vector3.up),
        2,
        EventType.Repaint
        );

    }
}
public enum Rotation
{

    Forward,
    Right,
    Backward,
    Left

}
public static class RotationHelper
{
    public static Rotation RotateRight(this Rotation rotation)
    {
        return (Rotation)(((int)rotation + 1) % 4);
    }
    public static Rotation RotateLeft(this Rotation rotation)
    {
        return (Rotation)(((int)rotation - 1 + 4) % 4);

    }

}
