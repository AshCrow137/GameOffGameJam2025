using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    //Class to handles all inputs from player.
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

    //End MenuInputs

    //GameInputs
    public void OnMoveCamera(CallbackContext value)
    {
        Vector2 directionInput = value.ReadValue<Vector2>();
        Debug.Log("OnMove: " + directionInput);

        //send to Camera Controller
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
            GetComponent<PlayerInput>().SwitchCurrentActionMap("InMenu");
        }
    }

    //End GameInputs
}
