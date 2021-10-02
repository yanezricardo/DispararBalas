using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastShooter : MonoBehaviour
{
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;


    void Start()
    {
        laserLine = GetComponent<LineRenderer>();

        gunAudio = GetComponent<AudioSource>();

        fpsCam = GetComponentInParent<Camera>();
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));


            laserLine.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out RaycastHit hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);
                var target = hit.transform.GetComponent<Target>();
                if (target != null)
                    target.TakeDamage(gunDamage, hit.point);
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }


    private IEnumerator ShotEffect()
    {
        if (gunAudio != null)
            gunAudio.Play();

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }
}
