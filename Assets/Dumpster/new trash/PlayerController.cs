using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PCLogic selectedPC;
    public PointOfInterest currentPOI;
    public bool rotateHeadWSAD = true;
    public Rotation rotation = Rotation.Forward;
    [SerializeField]
    private bool showGizmo = false;
    private void Start()
    {
        currentPOI?.Interact(this);
    }
    private void Update()
    {
        HandleMovement();
    }
    /*todo 0 implement rotation:
    add movement with keys? 
    add moevement with mouse
    add offset?
    todo 7 add focus poi
        todo 7 add poi generator

    todo 8 add other poi
    todo 9 test pois
    */
    public void HandleMovement()
    {
        /* if (Input.GetKeyDown(KeyCode.W))
         {
             currentPOI.forward?.Interact(this);
         }
         else if (Input.GetKeyDown(KeyCode.A))
         {
             if (rotateHeadWSAD)
             {
                 Rotate(true);
             }
             else
             {
                 currentPOI.left?.Interact(this);
             }
         }
         else if (Input.GetKeyDown(KeyCode.S))
         {
             currentPOI.backward?.Interact(this);
         }
         else if (Input.GetKeyDown(KeyCode.D))
         {
             if (rotateHeadWSAD)
             {
                 Rotate(false);
             }
             else
             {
                 currentPOI.right?.Interact(this);
             }
         }*/
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentPOI.GetPoi(rotation)?.Interact(this);
        }
        else if (Input.GetKeyDown(KeyCode.A))
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
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //THE QUESTION: do you rotate twice right or left to go backwards?
            var poi = currentPOI.GetPoi(rotation.RotateRight().RotateRight());
            if (poi != null)
            {
                poi.Interact(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
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


    }
    public void MoveTo(PointOfInterest pom)
    {
        currentPOI = pom;
        transform.position = currentPOI.transform.position;
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
