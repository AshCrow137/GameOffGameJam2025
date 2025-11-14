using UnityEngine;
using TMPro;

/// <summary>
/// Displays the cell position of the tile under the mouse cursor
/// </summary>
public class TilePositionDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text positionText;


    private void Start()
    {
        
        if (positionText == null)
        {
            Debug.LogError("TilePositionDisplay: Position text field is not assigned!");
        }
    }

    private void Update()
    {

        // Get the cell position at mouse cursor
        Vector3Int cellPosition = HexTilemapManager.Instance.GetCellAtMousePosition();

        // Display the position (if valid)
        if (cellPosition.x != int.MaxValue)
        {
            positionText.text = $"Tile Position: ({cellPosition.x}, {cellPosition.y}, {cellPosition.z})";
        }
        else
        {
            positionText.text = "Tile Position: (None)";
        }
    }
}

