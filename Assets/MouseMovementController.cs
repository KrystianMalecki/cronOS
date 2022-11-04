using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseMovementController : MonoBehaviour
{
    public static MouseMovementController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public Canvas mouseControls;
    [Header("Buttons")]
    public Image forwardButton;
    public Image backwardButton;
    public Image leftButton;
    public Image rightButton;
    public void ToggleMouseMovement(bool? state = null)
    {
        if (state == null)
        {
            ToggleMouseMovement(!mouseControls.enabled);
        }
        else
        {
            if (state.Value)
            {
                mouseControls.enabled = true;
            }
            else
            {
                mouseControls.enabled = false;
            }
        }
    }
    private void ToggleDirectionButton(Image directionButton, bool state)
    {
        directionButton.enabled = state;

    }
    public void ToggleAllDirectionButtons(PlayerController currentPlayerController)
    {
        PointOfInterest currentPOI = currentPlayerController.currentPOI;
        Rotation rotation = currentPlayerController.rotation;

        ToggleDirectionButton(forwardButton, currentPOI?.GetPoi(rotation) != null);
        ToggleDirectionButton(backwardButton, currentPOI?.GetPoi(rotation.RotateRight().RotateRight()) != null);
        if (currentPlayerController.rotateHeadWSAD)
        {
            ToggleDirectionButton(rightButton, true);
            ToggleDirectionButton(leftButton, true);

        }
        else
        {
            ToggleDirectionButton(rightButton, currentPOI?.GetPoi(rotation.RotateRight()) != null);
            ToggleDirectionButton(leftButton, currentPOI?.GetPoi(rotation.RotateLeft()) != null);

        }

    }
}
