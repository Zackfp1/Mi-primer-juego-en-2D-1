using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private Animator animator;

    private int FruitsApple;
    public TMP_Text texApple;
    private int Cup;
    public TMP_Text texCup;

    public AudioSource audioSource;
    public AudioClip appleClip;
    public AudioClip barrelClip;
    public AudioClip jumpClip;
    public AudioClip CupClip;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
            audioSource.PlayOneShot(jumpClip);
        }

        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Fruits"))
        {
            Destroy(collision.gameObject);
            FruitsApple++;
            texApple.text = FruitsApple.ToString();
            audioSource.PlayOneShot(appleClip);
        }

        if (collision.transform.CompareTag("Saws"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(collision.transform.CompareTag("Barrel"))
        {
            audioSource.PlayOneShot(barrelClip);
            Vector2 knockbackDir = (rb2D.position - (Vector2)collision.transform.position).normalized;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.AddForce(knockbackDir * 3, ForceMode2D.Impulse);

            BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();

            foreach (BoxCollider2D col in colliders)
            {
                col.enabled = false;
            }

            collision.GetComponent<Animator>().enabled = true;
            
            Destroy(collision.gameObject, 0.5f);
        }
        if (collision.transform.CompareTag("Cup") && Cup == 0)
        {
            Cup++;
            texCup.text = Cup.ToString();
            audioSource.PlayOneShot(CupClip);
        }
        
    }
}
