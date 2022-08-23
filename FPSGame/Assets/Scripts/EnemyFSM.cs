using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState //열거형 변수
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    EnemyState m_State;
    public float findDistance = 8f; //플레이어 발견 범위
    Transform player; //플레이어 트랜스폼 정보 저장할 변수

    public float attackDistance = 2f; //공격 범위
    public float moveSpeed = 5f; // 적 움직임 속도
    public float moveDistance = 20f; //이동 범위
    CharacterController cc; //

    float currentTime = 0; // 공격 딜레이 저장
    float attackDelay = 2f; // 공격 딜레이 시간

    public int attackPower = 3;
    Vector3 originPos; //초기위치

    public int hp = 15; //적의 체력
    int maxHp = 15;
    [SerializeField] Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle; //대기 상태로 초기화
        // 오브젝트의 이름이 Player인 오브젝트의 transform정보를 가져옴
        player = GameObject.Find("Player").transform; // -> Find관련 함수는 남용 금지 (리소스 문제)

        cc = GetComponent<CharacterController>();

        originPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)  //fsm 방식으로 상태 처리
        {
            case EnemyState.Idle: Idle(); break;
            case EnemyState.Move: Move(); break;
            case EnemyState.Attack: Attack(); break;
            case EnemyState.Return: Return(); break;
            case EnemyState.Damaged:; break;
            case EnemyState.Die:; break;
        }

        hpSlider.value = (float)hp / (float)maxHp;
    }
    void Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");
        }
    }

    void Move()
    {
        // 초기위치보다 20만큼 멀어졌다면...
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            print("상태 전환: Move -> Return");

        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            //적과 플레이어의 거리가 2미터 이상이면
            //(플레이어 vector - 적 vector)로 방향을 구한다.
            // 해당 방향으로 적을 이동시킨다.
            Vector3 dir = (player.position - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");
            currentTime = attackDelay; //이걸 해주지 않으면? 
            //currentTime이 0부터 시작하므로 적이 붙고나서 2초뒤에 공격하게됨
        }
    }
    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                // 플레이어의 playerMove 스크립트의 DamageAction함수를 호출한다. (기본적 방법)
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격!");
                currentTime = 0;
            }
        }
        else //거리가 2이상이면 상태를 move로 전환
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack->Move");
            currentTime = 0;
        }
    }
    void Return()
    {
        // 현재 위치와 원래 위치의 거리가 생기면
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            // 초기위치로 방향을 구한 뒤 이동
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else // 리턴 끝나고 도착
        {
            transform.position = originPos;
            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");
        }
    }

    public void HitEnemy(int hitPower) // 데미지를 입을때 체력이 깎임
    {
        if (m_State == EnemyState.Damaged ||
            m_State == EnemyState.Die ||
            m_State == EnemyState.Return)
        {
            return;
        }

        hp -= hitPower;
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any State -> Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any State -> Die");
            Die();
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess() //데미지 처리
    {
        //0.5초 동안 피격 애니메이션 실행 등 데미지를 처리하는 시간만큼
        //시간을 번 뒤 나머지 루틴을 실행
        yield return new WaitForSeconds(0.5f);
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    void Die()
    {
        //진행중인 피격 코루틴 함수를 중지
        StopAllCoroutines();
        StartCoroutine(DieProecss());
    }

    IEnumerator DieProecss()
    {
        cc.enabled = false;
        yield return new WaitForSeconds(2f);
        print(gameObject.name + " 소멸");
        Destroy(gameObject);
    }
}
