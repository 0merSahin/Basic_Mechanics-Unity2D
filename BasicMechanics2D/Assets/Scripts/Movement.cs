using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject SurfaceLeft;
    private GameObject SurfaceRight;
    private GameObject HitBoxLeft;
    private GameObject HitBoxRight;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] private float attackDistance;
    public bool canRightMove;
    public bool canLeftMove;
    public bool canJump;
    public bool canDash;
    public bool canAttack;
    public float dashDistance;
    public bool collisionEnemyRight;
    public bool collisionEnemyLeft;
    public float horizontalInputKey;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SurfaceLeft = GameObject.Find("SurfaceLeft");
        SurfaceRight = GameObject.Find("SurfaceRight");
        HitBoxLeft = GameObject.Find("HitBoxLeft");
        HitBoxRight = GameObject.Find("HitBoxRight");
        canRightMove = true;
        canLeftMove = true;
        canDash = true;
        canAttack = true;
        horizontalInputKey = 1;
    }


    void Update()
    {
        HorizontalMove();
        VerticalMove();
        DashMove();
        Attack();
    }


    private void HorizontalMove()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if ((horizontalInput == 1 && canRightMove) || (horizontalInput == -1 && canLeftMove))
        {
            horizontalInputKey = horizontalInput;
            transform.Translate(new Vector2(horizontalInput, 0) * speed * Time.deltaTime);
            ReColorSurface();
        }
    }


    private void VerticalMove()
    {
        bool verticalInput = Input.GetButtonDown("Jump");
        if (canJump && verticalInput)
        {
            canJump = false;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }


    private void DashMove()
    {
        if (canDash && Input.GetButton("Dash"))
        {
            Vector2 hitStart = transform.position;
            hitStart.y -= transform.localScale.y / 2 + 0.01f; // hit başlangıcı objenin en altından başlayacak.
            Vector2 hitStartRight = hitStart;
            hitStartRight.x += transform.localScale.x / 2 + 0.001f;
            Vector2 hitStartLeft = hitStart;
            hitStartLeft.x -= transform.localScale.x / 2 + 0.001f;

            RaycastHit2D[] hits;
            bool hitDetectKey = false;

            if (horizontalInputKey == -1)
                hits = Physics2D.RaycastAll(hitStartLeft, -transform.right, dashDistance);
            else
                hits = Physics2D.RaycastAll(hitStartRight, transform.right, dashDistance);

            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Ground"))
                {
                    hitDetectKey = true;
                    StartCoroutine(Dash(hit));
                    break;
                }
            }
            if (!hitDetectKey)
                StartCoroutine(Dash(new RaycastHit2D())); // Boş öğe gödneriliyor (null gönderilemediği için)
        }
    }


    IEnumerator Dash(RaycastHit2D hit)
    {
        canDash = false;
        float startTime = Time.time;
        float elapsedTime = 0f;
        float dashDuration = 0.15f;

        if (hit.collider != null)
        {
            dashDistance = hit.distance;
            dashDuration = hit.distance * 3 / 100;
        }

        Vector2 startPosition = transform.position;
        Vector2 dashPosition = startPosition + new Vector2(dashDistance * horizontalInputKey, 0f);
        while (elapsedTime <= dashDuration) // Dash işlemleri:
        {
            transform.position = Vector2.Lerp(startPosition, dashPosition, elapsedTime / dashDuration);
            elapsedTime = Time.time - startTime;
            yield return null;
        }
        while (elapsedTime <= 0.6f) // Dash'ler arası gecikme:
        {
            elapsedTime = Time.time - startTime;
            yield return null;
        }
        canDash = true;
        dashDistance = 5f;
    }



    private void Attack()
    {
        if (canAttack && Input.GetButtonDown("Fire1"))
        {
            Vector2 hitStart = transform.position;
            Vector2 hitStartRight = hitStart;
            hitStartRight.x += transform.localScale.x / 2 + 0.001f;
            Vector2 hitStartLeft = hitStart;
            hitStartLeft.x -= transform.localScale.x / 2 + 0.001f;

            RaycastHit2D hit;
            if (horizontalInputKey == -1)
                hit = Physics2D.Raycast(hitStartLeft, -transform.right, attackDistance);
            else
                hit = Physics2D.Raycast(hitStartRight, transform.right, attackDistance);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                canAttack = false;
                KillObject(hit.collider);
                StartCoroutine(ReColorHitBox(true));
            }
            else
            {
                canAttack = false;
                StartCoroutine(ReColorHitBox(false));
            }
        }
    }


    private void KillObject(Collider2D collider)
    {
        Destroy(collider.gameObject);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Wall"))
        {
            canJump = true;
        }
    }



    private void ReColorSurface()
    {
        if (horizontalInputKey == -1)
        {
            SurfaceLeft.GetComponent<SpriteRenderer>().color = Color.red;
            SurfaceRight.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            SurfaceLeft.GetComponent<SpriteRenderer>().color = Color.white;
            SurfaceRight.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }



    IEnumerator ReColorHitBox(bool key)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        if (key) // kırmızı işaret:
        {
            if (horizontalInputKey == -1) // sola vuruldu:
            {
                while (elapsedTime < 0.2f)
                {
                    HitBoxLeft.GetComponent<SpriteRenderer>().color = Color.red;
                    HitBoxLeft.GetComponent<SpriteRenderer>().enabled = true;
                    elapsedTime = Time.time - startTime;
                    yield return null;
                }
                HitBoxLeft.GetComponent<SpriteRenderer>().enabled = false;
                canAttack = true;
            }
            else // sağa vuruldu:
            {
                while (elapsedTime < 0.2f)
                {
                    HitBoxRight.GetComponent<SpriteRenderer>().color = Color.red;
                    HitBoxRight.GetComponent<SpriteRenderer>().enabled = true;
                    elapsedTime = Time.time - startTime;
                    yield return null;
                }
                HitBoxRight.GetComponent<SpriteRenderer>().enabled = false;
                canAttack = true;
            }
        }

        else // beyaz işaret:
        {
            if (horizontalInputKey == -1) // sola vuruldu:
            {
                while (elapsedTime < 0.2f)
                {
                    HitBoxLeft.GetComponent<SpriteRenderer>().color = Color.white;
                    HitBoxLeft.GetComponent<SpriteRenderer>().enabled = true;
                    elapsedTime = Time.time - startTime;
                    yield return null;
                }
                HitBoxLeft.GetComponent<SpriteRenderer>().enabled = false;
                canAttack = true;
            }
            else // sağa vuruldu:
            {
                while (elapsedTime < 0.2f)
                {
                    HitBoxRight.GetComponent<SpriteRenderer>().color = Color.white;
                    HitBoxRight.GetComponent<SpriteRenderer>().enabled = true;
                    elapsedTime = Time.time - startTime;
                    yield return null;
                }
                HitBoxRight.GetComponent<SpriteRenderer>().enabled = false;
                canAttack = true;
            }
        }
    }
    
}

