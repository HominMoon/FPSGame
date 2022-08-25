using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] GameObject firePosition;
    [SerializeField] GameObject bombFactory;
    [SerializeField] float throwPower = 1f;
    [SerializeField] int weaponPower = 5;

    [SerializeField] GameObject bulletEffect; // 피격 이펙트 오브젝트
    ParticleSystem ps; // 피격 파티클 시스템

    Animator anim;

    [SerializeField] Text wModeText;
    [SerializeField] GameObject[] eff_Flash; //총 발사 효과 오브젝트 배열


    enum WeaponMode
    {
        Normal, //소총
        Sniper  //스나이퍼
    }
    WeaponMode wMode;
    bool zoomMode = false;

    // Start is called before the first frame update
    void Start()// 이런것들을 callback함수라고 한다, 일반 함수와는 다르게 시스템의 필요에 따라 실행됨
    {
        wMode = WeaponMode.Normal;

        // 피격 이펙트 오브젝트의 파티클 시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run) return;

        //노멀 모드: 수류탄 투척

        //스나이퍼 모드: 화면 확대        

        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                case WeaponMode.Normal:
                    //발사위치에 수류탄 오브젝트 생성
                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    //수류탄을 던지기 위해 리지드바디 사용
                    Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();

                    //카메라의 정면 방향으로 물리적 힘을 가한다
                    rigidbody.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;
                case WeaponMode.Sniper:
                    if(!zoomMode) //줌모드: 확대
                    {
                        Camera.main.fieldOfView = 15f;
                    }
                    else //줌모드 아님 -> 원래 상태로
                    {
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;
                    }
                    break;
            }
        }

        // 숫자키1: 일반 , 숫자키2: 스나이퍼모드
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            wModeText.text = "Normal Mode";
            wMode = WeaponMode.Normal;
            Camera.main.fieldOfView = 60f;

        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            wModeText.text = "Sniper Mode";
            wMode = WeaponMode.Sniper;

        }

        if (Input.GetMouseButtonDown(0))
        {
            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            // 레이를 생성한 후 발사될 위치와 진행 방향을 설정
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            //레이가 부딪힌 대상의 정보를 저장할 변수 생성
            RaycastHit hitInfo = new RaycastHit();

            //부딪힌 물체가 있으면 피격이펙트 생성
            if (Physics.Raycast(ray, out hitInfo))
            {
                //만약 레이에 부딪힌 대상이 Enemy라면
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM enemyFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    enemyFSM.HitEnemy(weaponPower);
                }

                else
                {
                    //피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동
                    bulletEffect.transform.position = hitInfo.point;
                    bulletEffect.transform.forward = hitInfo.normal; //법선벡터 값을 넣어준다
                    ps.Play();
                }

            }

            StartCoroutine(ShootEffectOn(0.5f));
        }
    }

    IEnumerator ShootEffectOn(float duration)
    {
        int num = UnityEngine.Random.Range(0, eff_Flash.Length);
        eff_Flash[num].SetActive(true);
        yield return new WaitForSeconds(duration);
        eff_Flash[num].SetActive(false);
    }
}
