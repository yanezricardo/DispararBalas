using System.Collections;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public GameObject explosion;
    void OnCollisionEnter()
    {
        if (explosion != null)
            Instantiate(explosion, transform.position, transform.rotation);

        Destroy(gameObject, 10f);
    }
}
