using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform target; //카메라 오브젝트의 트랜스폼

    // Update is called once per frame
    void Update()
    {   // 카메라의 방향과 적 체력바(이 오브젝트)의 방향을 일치하게
        transform.forward = target.forward;
    }
}
