using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{

    [SerializeField] float rotationSpeed = 200f;

    float mx = 0; //회전값 누적 변수 2개
    float my = 0;

    void Start() {
        //커서를 화면 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        //회전값 변수에 마우스 입력 값만큼 미리 누적시킴
        mx += mouse_X * rotationSpeed * Time.deltaTime;
        my += mouse_Y * rotationSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -90f, 90f);
        transform.eulerAngles = new Vector3(-my, mx, 0);

        /*
        // 마우스 입력값으로 회전 방향 결정
        Vector3 direction = new Vector3(-mouse_Y, mouse_X, 0);
        // 회전 방향으로 물체를 회전
        transform.eulerAngles += direction * Time.deltaTime * rotationSpeed;

        //x축 회전값을 -90~90으로 제한
        Vector3 rotation = transform.eulerAngles;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        transform.eulerAngles = rotation;
        */
    }
}
