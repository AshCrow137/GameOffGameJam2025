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
    [SerializeField]
    public EntityType entityType;
    protected Transform CameraArm;

    protected EntityVision entityVision;

    protected HexTilemapManager hTM = HexTilemapManager.Instance;
    protected Vector3Int gridPosition;

    protected BaseKingdom Owner;
    protected SpriteRenderer baseSprite;

    protected static readonly float[,] AttackModifiers = {
                /*C |  I  | A | S | B */
    /*Cavalry*/ {1.0f,1.0f,1.5f,1f,1f },
    /*Infantry*/{1.5f,1.0f,1.0f,1f,1f },
    /*Archers*/ {1.0f,1.5f,1.0f,1f,1f },
    /*Special*/ {1.0f,1.0f,1.0f,1f,1f },
    /*Building*/{1.0f,1.0f,1.0f,1f,1f },
    };
    protected float GetDamageModifier(EntityType attacker, EntityType defender)
    {
        return AttackModifiers[(int)attacker, (int)defender];
    }
    public EntityType getVulnerableEntityType(EntityType attackerType)
    {
        switch(attackerType)
        {
            case EntityType.Infantry: return EntityType.Cavalry;
            case EntityType.Archer: return EntityType.Infantry;
            case EntityType.Cavalry: return EntityType.Archer;
        }
        return EntityType.None;
    }
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
        if(!baseSprite) baseSprite = GetComponent<SpriteRenderer>();
        Color ownerColor = Owner.GetKingdomColor();
        baseSprite.color = new Color(ownerColor.r, ownerColor.g, ownerColor.b, baseSprite.color.a);
        gridPosition = HexTilemapManager.Instance.WorldToCellPos(transform.position);
        transform.position = hTM.CellToWorldPos(gridPosition);
        // Initialize EntityVision component
        entityVision = GetComponent<EntityVision>();
        if (entityVision == null)
        {
            entityVision = gameObject.AddComponent<EntityVision>();
        }
        entityVision.Initialize(this);
    }
    protected virtual void OnDisable()
    {
        GlobalEventManager.EndTurnEvent.RemoveListener(OnEndTurn);
        GlobalEventManager.StartTurnEvent.RemoveListener(OnStartTurn);
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
        if (CameraArm == null) return;

        bodySprite.transform.localRotation = Quaternion.Euler(new Vector3(CameraArm.transform.rotation.eulerAngles.z + 90, -90, -90));
        foreach(GameObject obj in rotatebleObjects)
        {
            var objeu = obj.transform.localRotation.eulerAngles;
            obj.transform.localRotation = Quaternion.Euler(new Vector3(CameraArm.transform.rotation.eulerAngles.z + 90, -90, -90));
        }
        rotatebleCanvas.transform.localRotation = Quaternion.Euler(new Vector3(CameraArm.transform.rotation.eulerAngles.z+90, -90, -90));
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
        baseSprite.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, baseSprite.color.a);
        AudioManager.Instance.ui_menumain_volume.Post(gameObject);
    }
    /// <summary>
    /// invokes when kingdom deselect unit
    /// </summary>
    public virtual void OnEntityDeselect()
    {
        Color ownerColor = Owner.GetKingdomColor();
        baseSprite.color = new Color(ownerColor.r, ownerColor.g, ownerColor.b, baseSprite.color.a);
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

    public Sprite GetSprite() { return bodySprite.GetComponent<SpriteRenderer>().sprite; }
    public int GetVision() { return Vision; }
    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }
}
