using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 5;
    private Rigidbody2D rb2D;
    private float move;

    public float JumpForce = 4;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
 
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(move * Speed, rb2D.linearVelocity.y);

        if(move!= 0)
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, JumpForce);
        }
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
}
