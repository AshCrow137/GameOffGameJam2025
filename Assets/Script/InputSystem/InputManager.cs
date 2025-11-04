using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    //Class to handles all inputs from player.

    private Vector2 mousePos;

    //Menu Inputs
    public void OnNavigate(CallbackContext value)
    {
        Vector2 directionInput = value.ReadValue<Vector2>();
        Debug.Log("OnNavigate: " + directionInput);

        //send a direction to Menu Controller
    }

    public void OnSubmit(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnSubmit");
        }
        //select the current option in Menu Controller
    }

    public void OnCancel(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnCancel");
            GetComponent<PlayerInput>().SwitchCurrentActionMap("InGame");
        }
        //go back to previous menu in Menu Controller
    }

    public void OnPoint(CallbackContext value)
    {
        mousePos = value.ReadValue<Vector2>();
        Debug.Log("OnPoint: " + mousePos);
        //store the mouse position for other menu inputs
    }

    public void OnClickMenu(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnClickMenu at position: " + mousePos);
        }
        //send a position to Menu Controller
    }

    //End MenuInputs

    //GameInputs

    //Keyboard Inputs
    public void OnMoveCamera(CallbackContext value)
    {
        Vector2 directionInput = value.ReadValue<Vector2>();
        Debug.Log("OnMove: " + directionInput);

        //send a direction to Camera Controller
        //Eu preciso de um método que receba um vetor 2D e mova a camera nessa direção.
    }

    public void OnEndTurn(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnEndTurn");
            GameManager.instance.turnManager.OnTurnEnd();
            //sent to BootManager -> TurnManager (if not have any Unit to move) end turn.
        }
    }

    public void OnNextUnit(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnNextUnit");

            //sent to change selected unit.
        }
    }

    public void OnPauseGame(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnPauseGame");
            //sent to GameManager to pause the game.
            //change to Menu Input Action Map
            GetComponent<PlayerInput>().SwitchCurrentActionMap("InMenu");
        }
    }

    //Mouse Inputs
    public void OnMoveCameraWithMouse(CallbackContext value)
    {
        mousePos = value.ReadValue<Vector2>();
        Debug.Log("OnMoveCameraWithMouse: " + mousePos);
        //send a position to Camera Controller
        //I need a method that tracks the mouse's position on the screen
        //and controls the camera when the mouse is near the edge of the screen.
        //Suggestion: Use an offset for the edges.
        //For example, if the mouse is 100 pixels from the edge, move the camera in the desired direction.
    }

    public void OnSelectUnitWithMouse(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnClick at position: " + mousePos);
            //send a position to Selection Manager
            //I need a mathod that receives a screen position (Vector2)
            //and converts it to a raycast in the world,
        }
    }

    //End GameInputs
}
