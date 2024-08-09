using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidProjectileLauncher : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity;

    public void launch()
    {
        GameObject launchedProjectile = Instantiate(projectile, transform.position, transform.rotation);
        launchedProjectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3 (0, launchVelocity, 0));
    }
}
