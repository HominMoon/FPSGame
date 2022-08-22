using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    [SerializeField] GameObject bombEffect;



    private void OnCollisionEnter(Collision other) {
        GameObject eft = Instantiate(bombEffect);
        eft.transform.position = transform.position;
        Destroy(gameObject);
    }
}
