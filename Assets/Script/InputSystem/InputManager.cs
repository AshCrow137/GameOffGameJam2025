using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    //Class to handles all inputs from player.
    [SerializeField]
    private Texture2D cursorTexture;
    public static InputManager instance { get; private set; }
    private Vector2 mousePos;

    public BaseGridUnitScript selectedUnit { get; private set; }
    private GridCity selectedCity;


    [SerializeField]
    private PlayerKingdom playerKngdom;

    private bool bIsOnUIElement = false;
    public bool bHasSelectedEntity { get; private set; } = false;

    public void Initialize()
    {
        if(instance != null)
        {
            Destroy(instance); 
        }
        instance = this;
    }
    public void SetOnUiElement(bool value)
    {
        bIsOnUIElement = value;
    }
    //Menu Inputs
    public void OnNavigate(CallbackContext value)
    {
        Vector2 directionInput = value.ReadValue<Vector2>();
        //Debug.Log("OnNavigate: " + directionInput);

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
        //store the mouse position for other menu inputs
    }

    public void OnClickMenu(CallbackContext value)
    {
        if (value.performed)
        {
        }
        //send a position to Menu Controller
    }

    public void SetAttackCursor()
    {
        if(cursorTexture)
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    //End MenuInputs

    //GameInputs

    //Keyboard Inputs
    public void OnMoveCamera(CallbackContext value)
    {
        Vector2 directionInput = value.ReadValue<Vector2>();

    }

    public void OnEndTurn(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("OnEndTurn");
            TurnManager.instance.OnTurnEnd();
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
        //Debug.Log("OnMoveCameraWithMouse: " + mousePos);
        //send a position to Camera Controller
        //I need a method that tracks the mouse's position on the screen
        //and controls the camera when the mouse is near the edge of the screen.
        //Suggestion: Use an offset for the edges.
        //For example, if the mouse is 100 pixels from the edge, move the camera in the desired direction.
    }

    //method to any sort of tile interactions
    public void OnTileInteraction(CallbackContext value)
    {
        if (bIsOnUIElement) { return; }
        if (CityUI.Instance && CityUI.Instance.cityMenuMode != CityMenuMode.None) return; // if we are trying to place a building on the tile then don't do tile interaction from here.
        if (!value.performed || TurnManager.instance.GetCurrentActingKingdom() != playerKngdom) { return; }
        UIManager.Instance?.HasUnitSelected(false);
        UIManager.Instance?.HasCitySelected(false);
        if (ToggleManager.Instance.GetToggleState(ToggleUseCase.CityPlacement))
        {
            CityManager.Instance.TestPlaceCity();
        }
        else
        {
            
            TileState state = HexTilemapManager.Instance.GetTileState(HexTilemapManager.Instance.GetCellAtMousePosition());
            Debug.Log($"clicked tile type: {state}");
            if (selectedUnit)
            {
                selectedUnit.OnEntityDeselect();
                selectedUnit = null;
                bHasSelectedEntity = false;
            }
            if(selectedCity)
            {
                selectedCity.OnEntityDeselect();
                selectedCity = null;
                bHasSelectedEntity = false;
            }
            BaseGridUnitScript unit = HexTilemapManager.Instance.GetUnitOnTile(HexTilemapManager.Instance.GetCellAtMousePosition());
            GridCity city = CityManager.Instance.GetCity(HexTilemapManager.Instance.GetCellAtMousePosition());
            if (unit)
            {
                unit.OnEntitySelect(playerKngdom);
                //if(unit.GetOwner()==playerKngdom)
                //{
                    selectedUnit = unit;
                    bHasSelectedEntity = true;
                //}
                UIManager.Instance.SelectedUnit(unit);
            }
            else if(city)
            {
                //if(city.GetOwner()!=playerKngdom){
                //    return;
                //}
                selectedCity = city;
                selectedCity.OnEntitySelect(playerKngdom);
                bHasSelectedEntity= true;
                UIManager.Instance?.SelectedCity(city);
            }
        }
       
            //send a position to Selection Manager
            //I need a mathod that receives a screen position (Vector2)
            //and converts it to a raycast in the world,

    }
    public void OnAlternativeTileInteraction(CallbackContext value)
    {
        if (value.performed)
        {
            GlobalEventManager.InvokeMouseClickedEvent(mousePos);
            //send a position to Selection Manager
            //I need a mathod that receives a screen position (Vector2)
            //and converts it to a raycast in the world,
        }
    }
    //End GameInputs

    //Tests
    public void OnTestMadness(CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log(TurnManager.instance.GetCurrentActingKingdom().GetMadnessEffects());
        }
    }
    public Vector3 GetMousePosition()
    {
        return mousePos;
    }
    public Vector3 GetWorldPositionOnMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // cast a 3d ray onto a 2d plane. 
        // Note: composite collider2d with outlines as geometry type does not work because that creates an edge collider with no area.
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        // Raycast to detect what was clicked (adjust layer mask if needed)

        if (!hit.collider)
        {
            // return infinite vector
            return new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        }
        return hit.point;
    }
}
