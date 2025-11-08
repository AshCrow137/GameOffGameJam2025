using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridCity : MonoBehaviour
{
    public Sprite sprite;
    public Vector3Int position;

    // we can add building's relation to city later
    // public List<Building> buildings;

    public float maxHP = 100f;
    public float currentHP;

    public int visionRadius;

    public BaseKingdom Owner;

    // Resources this city generates per turn
    public Dictionary<ResourceType, int> resourceGainPerTurn = new Dictionary<ResourceType, int>();
    [SerializeField]
    private Transform CameraArm;
    [SerializeField]
    private GameObject bodySprite;
    [SerializeField]
    private Canvas rotatebleCanvas;
    private SpriteRenderer spriteRenderer;
    private bool bCanSpawnUnits = true;
    public void Initialize()
    {
        sprite = GetComponent<Sprite>();
        if (Camera.main != null && Camera.main.transform.parent != null)
        {
            CameraArm = Camera.main.transform.parent;
        }
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.color = Owner.GetKingdomColor();
        position = HexTilemapManager.Instance.GetMainTilemap().WorldToCell(transform.position);
        CityManager.Instance.AddCity(HexTilemapManager.Instance.GetMainTilemap().WorldToCell(transform.position), this);
    }
    public void InstantiateCity(CityData cityData, Vector3Int position,BaseKingdom owner)
    {
        this.sprite = cityData.sprite;
        this.position = position;
        this.maxHP = cityData.maxHP;
        this.currentHP = cityData.maxHP;
        this.visionRadius = cityData.visionRadius;
        this.Owner = owner;

        // Initialize empty resource dictionary - will be populated by other means
        this.resourceGainPerTurn = new Dictionary<ResourceType, int>();
        Owner.AddCityToKingdom(this);
        
        Initialize();
    }
    public void OnCitySelect(BaseKingdom selector)
    {
        if (selector != Owner)
        {
            return;
        }
        Debug.Log($"Select {this.name} city");
        spriteRenderer.color = Color.gray;
        GameplayCanvasManager.instance.ActivateUnitProductionPanel(this);
    }
    public void OnCityDeselect()
    {
        Debug.Log($"Deselect {this.name} city");
        spriteRenderer.color = Owner.GetKingdomColor();
        GameplayCanvasManager.instance.DeactivateUnitProductionPanel();
    }
    void LateUpdate()
    {
        //rotating unit body sprite
        bodySprite.transform.localRotation = Quaternion.Euler(new Vector3(CameraArm.transform.rotation.eulerAngles.z + 90, -90, -90));
        rotatebleCanvas.transform.rotation = Quaternion.Euler(new Vector3(0, 0, CameraArm.transform.rotation.eulerAngles.z));
    }
    public void TryToSpawnUnitInCity(GameObject unitPrefab)
    {
        List<Vector3Int> possiblePositions = HexTilemapManager.Instance.GetCellsInRange(position, 1);
    }
}
