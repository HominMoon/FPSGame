using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] GameObject firePosition;
    [SerializeField] GameObject bombFactory;
    [SerializeField] float throwPower = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            //발사위치에 수류탄 오브젝트 생성
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;
            //수류탄을 던지기 위해 리지드바디 사용
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            //카메라의 정면 방향으로 물리적 힘을 가한다
            rigidbody.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
    }
}
