using UnityEngine;

public class FishmanArcherUnit : BaseGridUnitScript
{
    [SerializeField]
    protected GameObject projectilePrefab;
    [SerializeField]
    protected Transform shootingPoint;
    [SerializeField] 
    protected float projectileSpeed = 15f;

    protected override void Attack(BaseGridEntity targetEntity)
    {
        base.Attack(targetEntity);
        var arrow = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);
        Vector3 direction = (targetEntity.transform.position - shootingPoint.position).normalized;
        direction.z = 0;
        arrow.GetComponent<Rigidbody>().linearVelocity = direction * projectileSpeed;
        arrow.GetComponent<ArrowProjectile>().Init(targetEntity.transform.position);
    }
}

