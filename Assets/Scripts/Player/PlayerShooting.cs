using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    [SerializeField] private Rigidbody2D firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Camera cam;
    [SerializeField] private float reloadTime = 0.25f;
    [SerializeField] private float bulletSpeed = 4f;

    private Vector2 mousePos;
    private Vector2 lookDirection;

    private Transform cachedFirePointTransform;

    private bool canShoot = true;

    private void Start()
    {
        cachedFirePointTransform = firePoint.transform;
    }

    private void Update()
    {
        //Camera Follow
        //TODO: Bound at edges of scene
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
        MouseLook();

        if(canShoot && Input.GetButton("Fire1"))
        {
            Shoot();
            canShoot = false;
            Invoke("Reload", reloadTime);
        }

    }

    private void MouseLook()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = mousePos - firePoint.position;
        float angle = (Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg) - 90f;
        cachedFirePointTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void Shoot()
    {
        // Get reusable bullet object from Bullet Pool
        GameObject bullet = BulletPool.bulletPoolInstance.GetBullet();

        //Set Bullet attributes
        bullet.transform.position = cachedFirePointTransform.position;
        bullet.transform.rotation = cachedFirePointTransform.rotation;
        Bullet b = bullet.GetComponent<Bullet>();
            b.SetDirection(cachedFirePointTransform.up);
            b.SetSpeed(bulletSpeed);
            b.SetDamage(1);
            b.SetHostility(false);

        //Activate bullet object (must happen last)
        bullet.SetActive(true);

        //Instantiate(bulletPrefab, cachedFirePointTransform.position, cachedFirePointTransform.rotation);
        //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        //rb.AddForce(cachedFirePointTransform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private void Reload()
    {
        canShoot = true;
    }
}