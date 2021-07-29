using UnityEngine;

public class DartShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float triggerRange;
    public float fireRate;
    [SerializeField]
    Vector3 rayDirection = Vector3.left;
    [SerializeField]
    LayerMask shootLayerMask;

    private float nextFire = 0f;

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, triggerRange, shootLayerMask, QueryTriggerInteraction.Ignore) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            shoot();
        }
    }

    void shoot()
    {
        Quaternion rotation = Quaternion.Euler(0, -90, 0);
        if (rayDirection.x > 0)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation) as GameObject;
        Dart dartObj = bullet.GetComponent<Dart>();
        dartObj.SetVelocity(rayDirection);
    }
}
