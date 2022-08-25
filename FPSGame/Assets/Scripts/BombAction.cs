using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    [SerializeField] GameObject bombEffect;
    public float explosionRadius = 5f;
    public int attackPower = 10;

    private void OnCollisionEnter(Collision other)
    {

        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }

        GameObject eft = Instantiate(bombEffect);
        eft.transform.position = transform.position;
        Destroy(gameObject);
    }
}
