using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpPower = 10f;
    bool isJump = false;

    CharacterController characterController;

    float gravity = -10f; //중력
    float yVelocity = 0; // 수직 속력 변수

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = direction.normalized; //정규화

        // 메인 카메라를 기준으로 방향 전환
        direction = Camera.main.transform.TransformDirection(direction);

        if (characterController.collisionFlags == CollisionFlags.Below)
        {
            if (isJump){
                isJump = false;
                yVelocity = 0;
            }
        }

        if (Input.GetButtonDown("Jump") && !isJump)
        {
            yVelocity = jumpPower;
            isJump = true;
        }

        //캐릭터 수직 속도에 중력 값을 적용함
        yVelocity += gravity * Time.deltaTime;
        direction.y = yVelocity;

        //이동속도에 맞춰 이동
        characterController.Move(direction * moveSpeed * Time.deltaTime);
        //transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
