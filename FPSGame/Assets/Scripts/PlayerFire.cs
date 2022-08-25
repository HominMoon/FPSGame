using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] GameObject firePosition;
    [SerializeField] GameObject bombFactory;
    [SerializeField] float throwPower = 1f;
    [SerializeField] int weaponPower = 5;

    [SerializeField] GameObject bulletEffect; // 피격 이펙트 오브젝트
    ParticleSystem ps; // 피격 파티클 시스템

    // Start is called before the first frame update
    void Start()// 이런것들을 callback함수라고 한다, 일반 함수와는 다르게 시스템의 필요에 따라 실행됨
    {
        // 피격 이펙트 오브젝트의 파티클 시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gm.gState != GameManager.GameState.Run) return;

        if (Input.GetMouseButtonDown(1))
        {
            //발사위치에 수류탄 오브젝트 생성
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            //수류탄을 던지기 위해 리지드바디 사용
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();

            //카메라의 정면 방향으로 물리적 힘을 가한다
            rigidbody.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(0))
        {   // 레이를 생성한 후 발사될 위치와 진행 방향을 설정
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
        }
    }
}
