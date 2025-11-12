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
    }
    protected virtual void OnEndTurn(BaseKingdom entity)
    {
        if (entity != Owner) { return; }
    }
    protected virtual void OnStartTurn(BaseKingdom entity)
    {
        if (entity != Owner) { return; }
    }
    protected virtual void LateUpdate()
    {
        //rotating entity body sprite and canvas facing camera 
        if(CameraArm == null)
        {
            Debug.Log("help");
        }
        bodySprite.transform.localRotation = Quaternion.Euler(new Vector3(CameraArm.transform.rotation.eulerAngles.z + 90, -90, -90));
        rotatebleCanvas.transform.rotation = Quaternion.Euler(new Vector3(0, 0, CameraArm.transform.rotation.eulerAngles.z));
    }
    public virtual void OnEntitySelect(BaseKingdom selector)
    {
        if (selector != Owner)
        {
            return;
        }
    }
    public virtual void OnEntityDeselect()
    {

    }
    public Vector3Int GetCellPosition()
    {
        return hTM.GetMainTilemap().WorldToCell(transform.position);
    }
    public BaseKingdom GetOwner() { return Owner; }
}
