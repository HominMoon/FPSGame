using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    [SerializeField] float destroyTime = 1.5f;
    float currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        if(currentTime > destroyTime){
            Destroy(gameObject);
        }
        currentTime += Time.deltaTime;
    }
}
