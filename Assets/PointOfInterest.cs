using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [NaughtyAttributes.ShowNonSerializedField]
    private static bool special = false;
    [Button]
    private void ToggleOn()
    {
        special = true;
    }
    [Button]

    private void ToggleOff()
    {
        special = false;
    }
    [Header("W")]
    public PointOfInterest forward;
    [Header("A")]
    public PointOfInterest left;
    [Header("S")]
    public PointOfInterest backward;
    [Header("D")]
    public PointOfInterest right;
    [SerializeField]
    private bool showGizmo = false;

    [HorizontalLine]
    public POIType type;
    public PointOfInterest GetPoi(Rotation rotation) =>
        rotation switch
        {
            Rotation.Forward => forward,
            Rotation.Left => left,
            Rotation.Backward => backward,
            Rotation.Right => right

        };




    public void Interact(PlayerController player)
    {
        switch (type)
        {
            case POIType.Move:
                player.MoveTo(this);
                break;
            case POIType.Focus:
                //todo 9 focus
                break;
            case POIType.Interact:
                //todo 9 Interact
                break;
            default:
                break;
        }
    }
    public void OnValidate()
    {
        if (!special)
        {
            if (forward == this)
            {
                forward = null;
            }
            if (left == this)
            {
                left = null;
            }
            if (backward == this)
            {
                backward = null;
            }
            if (right == this)
            {
                right = null;
            }



            if (forward != null)
            {
                if (forward.backward == null)
                {
                    forward.backward = this;
                }
                else if (forward.backward != this)
                {
                    Debug.Log($"POI {gameObject.name} is joined with {forward.gameObject.name} but {forward.gameObject.name} is not joined with {gameObject.name}. It i joined with {forward.backward.gameObject.name}!");
                }
            }
            if (left != null)
            {
                if (left.right == null)
                {
                    left.right = this;
                }
                else if (left.right != this)
                {
                    Debug.Log($"POI {gameObject.name} is joined with {left.gameObject.name} but {left.gameObject.name} is not joined with {gameObject.name}. It i joined with {left.right.gameObject.name}!");
                }



            }
            if (backward != null)
            {
                if (backward.forward == null)
                {
                    backward.forward = this;
                }
                else if (backward.forward != this)
                {
                    Debug.Log($"POI {gameObject.name} is joined with {backward.gameObject.name} but {backward.gameObject.name} is not joined with {gameObject.name}. It i joined with {backward.forward.gameObject.name}!");
                }
            }
            if (right != null)
            {
                if (right.left == null)
                {
                    right.left = this;
                }
                else if (right.left != this)
                {
                    Debug.Log($"POI {gameObject.name} is joined with {right.gameObject.name} but {right.gameObject.name} is not joined with {gameObject.name}. It i joined with {right.left.gameObject.name}!");
                }
            }
        }
    }
    public void OnDrawGizmos()
    {
        if (showGizmo)
        {
            OnDrawGizmosSelected();
        }
    }
    private float thickness = 2f;
    public void OnDrawGizmosSelected()
    {

        DrawArrow(forward?.transform, Handles.zAxisColor, "forward");
        DrawArrow(backward?.transform, Handles.zAxisColor, "backward");

        DrawArrow(right?.transform, Handles.xAxisColor, "right");
        DrawArrow(left?.transform, Handles.xAxisColor, "left");

        // Handles.DrawLine(transform.position, left.transform.position, thickness);

    }
    GUIStyle textStyle = GUIStyle.none;

    public void DrawArrow(Transform target, Color color, string label = "")
    {
        if (target == null)
        {
            return;
        }
        Handles.color = color;
        Gizmos.color = color;

        Handles.DrawLine(transform.position, target.position);


        Gizmos.DrawSphere(target.position, 0.1f);
        Gizmos.DrawSphere(transform.position, 0.1f);

        Handles.ArrowHandleCap(
        0,
        transform.position,
        Quaternion.LookRotation((target.position - transform.position), Vector3.up),
        Mathf.Min(3, (target.position - transform.position).magnitude - 0.3f),
        EventType.Repaint
        );
        textStyle.fontStyle = FontStyle.Bold;
        //change color of text in GuiStyle
        textStyle.normal.textColor = color;

        Handles.Label(target.position + new Vector3(0, 1f), label, textStyle);

        //  Debug.Log((target.position - transform.position).magnitude);
    }
}
public enum POIType
{
    Move,
    Focus,
    Interact
}
