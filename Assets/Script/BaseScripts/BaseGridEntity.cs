using UnityEngine;
using UnityEngine.UI;

public class BaseGridEntity : MonoBehaviour
{
    [Header("Grid entity variables")]
    [SerializeField]
    protected int Health = 2;
    [SerializeField]
    protected int CurrentHealth = 2;
    [SerializeField]
    protected int Vision = 5;
    [SerializeField]
    protected Image HPImage;
    [SerializeField]
    protected GameObject bodySprite;
    [SerializeField]
    protected GameObject[] rotatebleObjects;
    [SerializeField]
    protected Canvas rotatebleCanvas;
    protected Transform CameraArm;


    protected HexTilemapManager hTM = HexTilemapManager.Instance;
    protected Vector3Int gridPosition;

    protected BaseKingdom Owner;
    protected SpriteRenderer baseSprite;

    public virtual void Initialize(BaseKingdom owner)
    {
        hTM = HexTilemapManager.Instance;
       
        Owner = owner;
        if (!Owner)
        {
            Debug.LogError($"{this.gameObject} has no owner!");
        }
        if (Camera.main != null && Camera.main.transform.parent != null)
        {
            CameraArm = Camera.main.transform.parent;
        }
        GlobalEventManager.EndTurnEvent.AddListener(OnEndTurn);
        GlobalEventManager.StartTurnEvent.AddListener(OnStartTurn);
        HPImage.color = Owner.GetKingdomColor();
        gridPosition = HexTilemapManager.Instance.WorldToCellPos(transform.position);

        // Initialize EntityVision component
        EntityVision entityVision = GetComponent<EntityVision>();
        if (entityVision == null)
        {
            entityVision = gameObject.AddComponent<EntityVision>();
        }
        entityVision.Initialize(this);
    }
    /// <summary>
    /// Invokes whis EndTurnEvent in GlobalEventManager
    /// </summary>
    /// <param name="entity">Kingdom that end his turn</param>
    protected virtual void OnEndTurn(BaseKingdom entity)
    {
        
    }
    /// <summary>
    /// Invokes whis StartTurnEvent in GlobalEventManager
    /// </summary>
    /// <param name="entity">current kingdom's turn</param>
    protected virtual void OnStartTurn(BaseKingdom entity)
    {
        
    }

    protected virtual void LateUpdate()
    {
        //rotating entity body sprite and canvas facing camera 

        bodySprite.transform.localRotation = Quaternion.Euler(new Vector3(CameraArm.transform.rotation.eulerAngles.z + 90, -90, -90));
        foreach(GameObject obj in rotatebleObjects)
        {
            var objeu = obj.transform.localRotation.eulerAngles;
            obj.transform.localRotation = Quaternion.Euler(new Vector3(CameraArm.transform.rotation.eulerAngles.z + 90, -90, -90));
        }
        rotatebleCanvas.transform.rotation = Quaternion.Euler(new Vector3(0, 0, CameraArm.transform.rotation.eulerAngles.z));
    }
    /// <summary>
    /// invokes when kingdom select grid entity
    /// </summary>
    /// <param name="selector">kingdom that select this grid entity</param>
    public virtual void OnEntitySelect(BaseKingdom selector)
    {
        if (selector != Owner)
        {
            return;
        }
    }
    /// <summary>
    /// invokes when kingdom deselect unit
    /// </summary>
    public virtual void OnEntityDeselect()
    {

    }
    protected virtual void Death()
    {

    }
    public virtual void TakeDamage(int amount, BaseGridUnitScript attacker, bool retallitionAttack)
    {

    }
    /// <summary>
    /// Get cell position of entity on main tilemap
    /// </summary>
    /// <returns>Vector3Int position of grid entity on main tilemap </returns>
    public Vector3Int GetCellPosition()
    {
        return hTM.GetMainTilemap().WorldToCell(transform.position);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Kingdom that own this entity</returns>
    public BaseKingdom GetOwner() { return Owner; }

    /// <summary>
    /// Gets the canvas component
    /// </summary>
    public Canvas GetRotatableCanvas() { return rotatebleCanvas; }
}
