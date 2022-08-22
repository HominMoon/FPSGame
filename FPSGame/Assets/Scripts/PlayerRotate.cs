using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 200f;

    float mx = 0; //회전값 누적 변수 2개

    void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");

        //회전값 변수에 마우스 입력 값만큼 미리 누적시킴
        mx += mouse_X * rotationSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
