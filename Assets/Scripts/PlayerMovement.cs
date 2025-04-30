using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻�̵� ����")]
    public float moveSpeed = 5f;                                //�̵� �ӵ� ���� ���� 
    public float jumpForce = 7f;
    public float turnSpeed = 10f;

    [Header("���� ���� ����")]
    public float failMultiplier = 2.5f;
    public float lowJumpMultipler = 2.0f;

    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrounded = true;

    [Header("�۶��̴� ����")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float glidermaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;

    public bool isGrounded = true;                              //���� �ִ��� üũ �ϴ� ���� (true/false)

    public int coinCount = 0;                                   //���� ȹ�� ���� ����
    public int totalCoins = 5;                                  //�� ���� ȹ�� �ʿ� ���� ����

    public Rigidbody rb;                                        //�÷��̾� ��ü�� ���� 

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
        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        Vector3 movement = new Vector3(moveHorizontal, 0, moveSpeed);

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        //�ӵ��� ���� �̵�
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);



        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (failMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultipler - 1) * Time.deltaTime;
        }
        //���� �Է�
        if (Input.GetButtonDown("Jump") && isGrounded)          //&& �� ���� �����Ҷ� -> (�����̽� ��ư�� �������� �� isGrounded �� True �϶�)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);         //�������� ������ ����ŭ ��ü�� �ش�. 
            isGrounded = false;
            realGrounded = false;
            coyoteTimeCounter = 0;//������ �ϴ� ���� ������ �������� ������ false��� ���ش�. 
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

    void OnCollisionEnter(Collision collision)              //�浹 ó�� �Լ� 
    {
        if (collision.gameObject.tag == "Ground")            //�浹�� �Ͼ ��ü�� Tag�� Ground ���
        {
            isGrounded = true;                              //���� �浹�ϸ� true�� �����Ѵ�.
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


    private void OnTriggerEnter(Collider other)             //Ʈ���� ���� �ȿ� ���Գ��� �˻��ϴ� �Լ� 
    {
        //���� ����
        if (other.CompareTag("Coin"))                        //���� Ʈ���ſ� �浹 �ϸ� 
        {
            coinCount++;                                    //coinCount = coinCount + 1  ���� ���� 1�� �÷��ش�.
            Destroy(other.gameObject);                      //���� ������Ʈ�� ������. 
            Debug.Log($"���� ���� : {coinCount}/{totalCoins}");
        }

        //������ ���� �� ���� �α� �ⷰ
        if (other.gameObject.tag == "Door" && coinCount == totalCoins)           //��� ������ ȹ���Ŀ� ������ ���� ���� ����
        {
            Debug.Log("���� Ŭ����");
            //���� �Ϸ� ���� �߰� ����
        }
    }
    void UpdateGroundedState()
    {

        if (realGrounded) //���� �ڹ��Ť��������� �ڿ���Ÿ�Ӹ���
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
    







