using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    private float reachThreshold = 1f;

    public void Init(Vector3 targetPos)
    {
        targetPosition = targetPos;
        Vector3 direction = targetPos - transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) <= reachThreshold)
        {
            Destroy(gameObject);
        }
    }
}