using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject bulletPrefab;
    public bool isShooting = false;
    private bool isCountingDown = false;
    // this is to prevent the shooting variable to switch TOO fast 
    // that the animation is not shown
    public float shootingAnimationGap = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();

        }
        // if Fire1 is NOT PRESSED, not get button up, set isShooting to false
        // NOT GET BUTTON UP!
        if (!Input.GetButton("Fire1") && !isCountingDown)
        {
            isShooting = false;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        isShooting = true;
        StartCoroutine(ShootingAnimation());
        // destroy bullet after 2 seconds
        Destroy(bullet, 2f);
    }

    IEnumerator ShootingAnimation()
    {
        isCountingDown = true;
        yield return new WaitForSeconds(shootingAnimationGap);
        isCountingDown = false;
    }
}
