using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poly : MonoBehaviour
{
    //Unit[] unit = new Unit[3];

    Unit unit = new Unit();
    Marine marine;
    Tank tank;

    void Start()
    {
        unit.AttackTest(new Marine());
        unit.AttackTest(new Tank());

        // unit[0] = new Unit();
        // unit[1] = new Marine();
        // unit[2] = new Tank();


        // for (int i = 0; i < unit.Length; i++)
        // {
        //     unit[i].Attack();
        //     unit[i].Move();
        // }
        
    }

    void Update()
    {

    }
}


class Unit
{
    public virtual void Attack() //virtual 가상함수
    {
        Debug.Log("Attack...");
    }
    public virtual void AttackTest(Unit unit) //virtual 가상함수
    {
        unit.Attack();
    }
    public virtual void Move()
    {
        Debug.Log("Move...");
    }
}

class Marine : Unit
{
    public override void Attack()
    {
        Debug.Log("마린의 기관총 공격");
    }
    public override void Move()
    {
        base.Move(); //부모의 Move() 호출
        Debug.Log("마린이 아장아장 걷는다");
    }
}

class Tank : Unit
{
    public override void Attack()
    {
        Debug.Log("탱크의 퉁퉁포 공격");
    }
    public override void Move()
    {
        base.Move(); //부모의 Move() 호출
        Debug.Log("탱크가 쿠구구궁 기동한다");
    }
}