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

    //public BaseGridUnitScript selectedUnit { get; private set; }

    private PlayerInput playerInput;

    private PlayerKingdom playerKngdom;

    private bool bIsOnUIElement = false;
    public bool bHasSelectedEntity { get; private set; } = false;


    public Vector2 CameraKeyMovementDirection { get; private set; }
    public Vector2 CameraMouseMovementDirection { get; private set; }

    public void Initialize()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        playerInput = GetComponent<PlayerInput>();
        playerKngdom = PlayerKingdom.Instance;
    }
    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
        playerInput.ActivateInput();
    }
    private void OnDisable()
    {
        playerInput.DeactivateInput();
    }

    public void SetOnUiElement(bool value)
    {
        bIsOnUIElement = value;
    }
    public bool IsCursorOverUIElement()
    {
        return bIsOnUIElement;
    }
    public void SetAttackCursor()
    {
        if (cursorTexture)
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    #region MenuInput
    public void OnCancel(InputValue value)
    {

        Debug.Log("OnCancel");
        //GetComponent<PlayerInput>().SwitchCurrentActionMap("InGame");

        //go back to previous menu in Menu Controller
    }
    public void OnClickMenu(InputValue value)
    {

        //send a position to Menu Controller
    }
    public void OnNavigate(InputValue value)
    {
        Vector2 directionInput = value.Get<Vector2>();
        //Debug.Log("OnNavigate: " + directionInput);

    }





    public void OnPoint(InputValue value)
    {
        mousePos = value.Get<Vector2>();
        //store the mouse position for other menu inputs
    }

    public void OnSubmit(InputValue value)
    {

        Debug.Log("OnSubmit");

        //select the current option in Menu Controller
    }
    #endregion


    #region KeyboardInput
    public void OnCameraZoom(InputValue value)
    {
        //Debug.Log("CameraZoom");
    }
    public void OnMoveCamera(InputValue value)
    {
        CameraKeyMovementDirection = value.Get<Vector2>();

    }


    public void OnEndTurn(InputValue value)
    {

        Debug.Log("OnEndTurn");
        TurnManager.instance.OnTurnEnd();
        //sent to BootManager -> TurnManager (if not have any Unit to move) end turn.

    }

    public void OnNextUnit(InputValue value)
    {

        Debug.Log("OnNextUnit");

        //sent to change selected unit.

    }

    public void OnPauseGame(InputValue value)
    {

        Debug.Log("OnPauseGame");
        //sent to GameManager to pause the game.
        //change to Menu Input Action Map
        //GetComponent<PlayerInput>().SwitchCurrentActionMap("InMenu");

    }

    //Mouse Inputs
    public void OnMoveCameraWithMouse(InputValue value)
    {
        mousePos = value.Get<Vector2>();
        //Debug.Log("OnMoveCameraWithMouse: " + mousePos);
        //send a position to Camera Controller
        //I need a method that tracks the mouse's position on the screen
        //and controls the camera when the mouse is near the edge of the screen.
        //Suggestion: Use an offset for the edges.
        //For example, if the mouse is 100 pixels from the edge, move the camera in the desired direction.
    }

    //method to any sort of tile interactions
    public void OnLeftClick(InputValue value)
    {
        if (bIsOnUIElement || TurnManager.instance.GetCurrentActingKingdom() != playerKngdom) { return; }

        if (CityUI.Instance && CityUI.Instance.cityMenuMode != CityMenuMode.None)
        {
            CityUI.Instance.PlaceEntity();
        }
        else
        {

            UIManager.Instance?.HasUnitSelected(false);
            UIManager.Instance?.HasCitySelected(false);
            {

                TileState state = HexTilemapManager.Instance.GetTileState(HexTilemapManager.Instance.GetCellAtMousePosition());
                //Debug.Log($"clicked tile type: {state}");
                BaseGridUnitScript selectedUnit = UIUtility.selectedUnit;
                if (selectedUnit)
                {
                    if (selectedUnit.aiming == true)
                    {
                        selectedUnit.OnChosingTile();
                        return;
                    }
                    //selectedUnit.OnEntityDeselect();
                    //selectedUnit = null;
                    //bHasSelectedEntity = false;
                    UIUtility.DeselectUnit();
                }
                GridCity selectedCity = UIUtility.selectedCity;
                if (selectedCity)
                {
                    //selectedCity.OnEntityDeselect();
                    //selectedCity = null;
                    //bHasSelectedEntity = false;
                    UIUtility.DeselectCity();
                }
                BaseGridUnitScript unit = HexTilemapManager.Instance.GetUnitOnTile(HexTilemapManager.Instance.GetCellAtMousePosition());
                GridCity city = CityManager.Instance.GetCity(HexTilemapManager.Instance.GetCellAtMousePosition());
                if (unit)
                {
                    //unit.OnEntitySelect(playerKngdom);
                    ////if(unit.GetOwner()==playerKngdom)
                    ////{
                    //selectedUnit = unit;
                    //bHasSelectedEntity = true;
                    ////}
                    //UIManager.Instance.OnUnitSelect(unit);
                    UIUtility.SelectUnit(unit);
                }
                else if (city)
                {
                    ////if(city.GetOwner()!=playerKngdom){
                    ////    return;
                    ////}
                    //selectedCity = city;
                    //selectedCity.OnEntitySelect(playerKngdom);
                    //bHasSelectedEntity = true;
                    //UIManager.Instance?.OnCitySelect(city);
                    UIUtility.SelectCity(city);
                }
            }
        }

        //send a position to Selection Manager
        //I need a mathod that receives a screen position (Vector2)
        //and converts it to a raycast in the world,

    }
    public void OnRightClick(InputValue value)
    {
        if (bIsOnUIElement || TurnManager.instance.GetCurrentActingKingdom() != playerKngdom) { return; }
        //GlobalEventManager.InvokeMouseClickedEvent(mousePos);
        if (UIUtility.selectedUnit)
        {
            UIUtility.selectedUnit.OnTileClicked(HexTilemapManager.Instance.GetCellAtMousePosition());
        }
        //send a position to Selection Manager
        //I need a mathod that receives a screen position (Vector2)
        //and converts it to a raycast in the world,

    }

    public void OnInventory(InputValue value)
    {
        InventoryUI.Instance.OnInventory();
    }
    #endregion

    public Vector3 GetMousePosition()
    {
        return mousePos;
    }
    public Vector2 GetMousePosVector2()
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
