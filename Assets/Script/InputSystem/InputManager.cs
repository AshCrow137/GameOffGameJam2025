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
    }

    public void OnSubmit(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnSubmit");
        }
    }

    public void OnCancel(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnCancel");
            GetComponent<PlayerInput>().SwitchCurrentActionMap("InGame");
        }
    }

    public void OnPoint(CallbackContext value)
    {
        mousePos = value.ReadValue<Vector2>();
        Debug.Log("OnPoint: " + mousePos);
    }

    public void OnClickMenu(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnClickMenu at position: " + mousePos);
        }
    }

    //End MenuInputs

    //GameInputs

    //Keyboard Inputs
    public void OnMoveCamera(CallbackContext value)
    {
        Vector2 directionInput = value.ReadValue<Vector2>();
        Debug.Log("OnMove: " + directionInput);

        //send a direction to Camera Controller
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
    }

    public void OnSelectUnitWithMouse(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnClick at position: " + mousePos);
            //send a position to Selection Manager
        }
    }

    //End GameInputs
}
