using UnityEngine;

public class ShooterTut : MonoBehaviour
{
    public Camera cam;
    public GameObject projectile;
    public Transform firePointR;
    public Transform firePointL;
    public float projectileSpeed = 30;
    public float fireRate = 4;
    public float arcRange = 1;

    private Vector3 destination;
    private bool leftHand;
    private float timeToFire;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        } else
        {
            destination = ray.GetPoint(1000);
        }
        if (leftHand)
        {
            leftHand = false;
            InstantiateProjectile(firePointL);
        } else
        {
            leftHand = true;
            InstantiateProjectile(firePointR);
        }
    }

    private void InstantiateProjectile(Transform firePoint)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(arcRange, arcRange), Random.Range(arcRange, arcRange), 0), Random.Range(0.5f, 2));
    }
}
