using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본이동 설정")]
    public float moveSpeed = 5f;                                //이동 속도 변수 설정 
    public float jumpForce = 7f;
    public float turnSpeed = 10f;

    [Header("점프 개선 설정")]
    public float failMultiplier = 2.5f;
    public float lowJumpMultipler = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrounded = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float glidermaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;

    public bool isGrounded = true;                              //땅에 있는지 체크 하는 변수 (true/false)

    public int coinCount = 0;                                   //코인 획득 변수 선언
    public int totalCoins = 5;                                  //총 코인 획들 필요 변수 선언

    public Rigidbody rb;                                        //플레이어 강체를 선언 

    // Start is called before the first frame update
    void Start()
    {
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        gliderTimeLeft = glidermaxTime;


        coyoteTimeCounter = 0;
    }
    void EnableGlider()
    {
        isGliding = true;
        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }
    }

    void DisableGlider()
    {
        isGliding = false;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
    }

    void ApplyGliderMovement(float horizontal, float vertical)
    {
        Vector3 gliderVelocity = new Vector3(
            horizontal * gliderMoveSpeed,
            -gliderFallSpeed,
            vertical * gliderMoveSpeed
    );

    rb.velocity = gliderVelocity;
}

    // Update is called once per frame
    void Update()
    {
        UpdateGroundedState();
        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        Vector3 movement = new Vector3(moveHorizontal, 0, moveSpeed);

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        //속도로 직접 이동
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);



        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (failMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultipler - 1) * Time.deltaTime;
        }
        //점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded)          //&& 두 값을 만족할때 -> (스페이스 버튼일 눌렸을때 와 isGrounded 가 True 일때)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);         //위쪽으로 설정한 힘만큼 강체에 준다. 
            isGrounded = false;
            realGrounded = false;
            coyoteTimeCounter = 0;//점프를 하는 순간 땅에서 떨어졌기 때문에 false라고 해준다. 
        }

        if(Input .GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if (isGliding)
            {
                EnableGlider();
            }

            gliderTimeLeft -= Time.deltaTime;

            if(isGliding )

                    DisableGlider();
        }

        if(isGliding)
        {
            //ApplyGliderMovement(moveHorizontal * moveVertical);
        }
        else
        {
            rb.velocity = new Vector3(moveHorizontal * moveSpeed , rb.velocity .y , moveVertical * moveSpeed);

            if (rb.velocity.y < 0)
            {
               // rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    void OnCollisionEnter(Collision collision)              //충돌 처리 함수 
    {
        if (collision.gameObject.tag == "Ground")            //충돌이 일어난 물체의 Tag가 Ground 경우
        {
            isGrounded = true;                              //땅과 충돌하면 true로 변경한다.
        }
    }

   void OnCollisionStay(Collision collision)
    {
        if (collision . gameObject .CompareTag("grounded"))
        {
            realGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrounded = false;
        }
    }


    private void OnTriggerEnter(Collider other)             //트리거 영역 안에 들어왔나를 검사하는 함수 
    {
        //코인 수집
        if (other.CompareTag("Coin"))                        //코인 트리거와 충돌 하면 
        {
            coinCount++;                                    //coinCount = coinCount + 1  코인 변수 1을 올려준다.
            Destroy(other.gameObject);                      //코인 오브젝트를 없엔다. 
            Debug.Log($"코인 수집 : {coinCount}/{totalCoins}");
        }

        //목적지 도착 시 종료 로그 출럭
        if (other.gameObject.tag == "Door" && coinCount == totalCoins)           //모든 코인을 획득후에 문으로 가면 게임 종료
        {
            Debug.Log("게임 클리어");
            //게임 완료 로직 추가 가능
        }
    }
    void UpdateGroundedState()
    {

        if (realGrounded) //실제 자묘ㅕㄴ애있음ㄴ 코요테타임리셋
        {

            coyoteTimeCounter = coyoteTime;
            isGrounded = true;

        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;

            }

            else
            {
                isGrounded = false;
            }
        }
    }
}
    







