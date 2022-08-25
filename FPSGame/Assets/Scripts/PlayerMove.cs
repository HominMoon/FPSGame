using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpPower = 10f;
    [SerializeField] Slider hpSlider;
    [SerializeField] GameObject hitEffect; //hit효과 이미지 변수
    bool isJump = false;

    public int hp = 20;
    int maxHp = 20;
    
    // static: 공유변수, 클래스변수

    CharacterController characterController;

    float gravity = -10f; //중력
    float yVelocity = 0; // 수직 속력 변수

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //
        if(GameManager.gm.gState != GameManager.GameState.Run) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = direction.normalized; //정규화

        // 메인 카메라를 기준으로 방향 전환
        direction = Camera.main.transform.TransformDirection(direction);

        if (characterController.collisionFlags == CollisionFlags.Below)
        {
            if (isJump)
            {
                isJump = false;

            }
            yVelocity = 0;
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

        // 현재체력 / 최대체력 값을 슬라이더의 value에 적용한다.
        hpSlider.value = (float)hp / (float)maxHp;
    }

    public void DamageAction(int damage)
    {
        hp -= damage;
        
        // 죽지 않았다면 피격효과 발생
        if(hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
    }

    IEnumerator PlayHitEffect()
    {
        //0.3초간 hit 이미지 활성화 후 비활성화
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitEffect.SetActive(false);
    }
}
