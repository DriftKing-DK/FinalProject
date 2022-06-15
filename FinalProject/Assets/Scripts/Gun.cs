using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage, magazineSize, bulletsPerTap;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public bool allowButtonHold;

    public float duration, magnitude, recoil;
    
    private float elapsed;
    private int bulletsLeft, bulletsShot;
    private bool shooting, readyToShoot, reloading, shaking;

    //private List<Vector3> debugDirection;

    private Camera fpsCam;


    public void Setup(Camera c)
    {
        fpsCam = c;
        elapsed = 0f;
        bulletsLeft = magazineSize;
        shooting = false;
        readyToShoot = true;
        reloading = false;
        shaking = false;
        //debugDirection = new List<Vector3>();
    }

    public int GetBulletLeft()
    {
        return bulletsLeft;
    }

    public bool CanChangeGun()
    {
        return !reloading && bulletsLeft > 0 && readyToShoot;
    }

    public int ManageInput()
    {
        int res = 0;
        if (allowButtonHold)
            shooting = Input.GetMouseButton(1);
        else
            shooting = Input.GetMouseButtonDown(1);
        
        if (Input.GetKeyDown(KeyCode.R) && !reloading && bulletsLeft < magazineSize && readyToShoot)
        {
            Reload();
            res = 1;
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
            res = 2;
        }
        Shake();

        //debug
        /*foreach (Vector3 v in debugDirection)
            Debug.DrawRay(fpsCam.transform.position, v * range);*/
        
        return res;
    }

    private void ResetReload()
    {
        reloading = false;
        bulletsLeft = magazineSize;
    }

    private void Reload()
    {
        reloading = true; // forbid the player to reload again
        Invoke("ResetReload", reloadTime);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Shoot()
    {
        readyToShoot = false; // forbid the player to continu fireing
        bulletsLeft -= 1;
        ShootBis(); // first shot
        while (bulletsShot > 1) // shot all bullets per tap
        {
            Invoke("ShootBis", timeBetweenShots);
            bulletsShot -= 1;
        }
        Invoke("ResetShot", timeBetweenShooting); // allow the player to shoot again
    }

    private void ShootBis()
    {
        LauchFlash();
        ResetShake();
        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //direction with spread
        Quaternion spreadAngle_x = Quaternion.AngleAxis(x, new Vector3(0, 1, 0));
        Quaternion spreadAngle_y = Quaternion.AngleAxis(y, new Vector3(1, 0, 0));

        Vector3 direction = spreadAngle_y * spreadAngle_x * fpsCam.transform.forward;

        //raycast
        RaycastHit rayHit;
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        {
            Target target = rayHit.transform.GetComponent<Target>();
            //Debug.Log("touch");
            if (target != null)
                target.TakeDamage(damage);
        }

        //debugDirection.Add(direction);
    }

    private void LauchFlash()
    {
        ParticleSystem[] muzzleFlashs = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem muzzle in muzzleFlashs)
        {
            Light l = muzzle.GetComponentInChildren<Light>();
            l.enabled = true;
            muzzle.Play();
            l.enabled = false;
        }
    }

    private void ResetShake()
    {
        elapsed = 0f;
        shaking = true;
    }

    private void Shake()
    {
        if (elapsed >= duration)
        {
            shaking = false;
            elapsed = 0f;
        }
        
        if (shaking)
        {
            float x = Random.Range(-0.1f, 0.1f) * magnitude;
            float y = Random.Range(-0.1f, 0.1f) * magnitude;
            transform.localPosition = new Vector3(x, y, transform.localPosition.z - recoil * Time.deltaTime);
            elapsed += Time.deltaTime;
        }
        else
            transform.localPosition = Vector3.zero;
    }
}
