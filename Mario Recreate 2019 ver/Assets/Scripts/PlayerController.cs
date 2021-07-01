using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float maxSpeed = 10;
    public float upSpeed;

    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private bool pressLeft;
    private bool pressRight;
    private bool releaseLeft;
    private bool releaseRight;

    //public Transform enemyLocation;
    //public Text scoreText;
    //private int score = 0;
    //private bool jumpedOverEnemy = false;

    private bool timeToRestart = false;
    private float wait;

    private Animator marioAnimator;
    private AudioSource marioAudio;

    private bool isLanding = false;
    public ParticleSystem dustCloud;


    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();


    }

    private void Update()
    {

        pressLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a");
        pressRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d");
        releaseLeft = Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp("a");
        releaseRight = Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp("d");

        // toggle state
        if (pressLeft && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;

            // check velocity
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        if (pressRight && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;

            // check velocity
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        if (isLanding && onGroundState)
        {
            dustCloud.Play();
            isLanding = false;
        }

        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        //if (!onGroundState
        //    && marioBody.position.y > enemyLocation.position.y
        //    && Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
        //{
        //    jumpedOverEnemy = true;
        //}

        if (timeToRestart)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            marioBody.velocity = Vector2.zero;

            wait += Time.deltaTime;

            if (wait > 1.0)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }

    }

    // FixedUpdate may be called once per frame. See documentation for details.

    void FixedUpdate()
    {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        if (releaseLeft || releaseRight)
        {
            // stop
            marioBody.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            isLanding = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }

    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles"))
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);

            //if (jumpedOverEnemy)
            //{
            //    score++;
            //    jumpedOverEnemy = false;
            //}
            //scoreText.text = "Score: " + score.ToString();
        };
    }

    // called when mario collides with enemy - Gomba
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");

            timeToRestart = true;
        }
    }

    void PlayJumpSound()
    {
        marioAudio.PlayOneShot(marioAudio.clip);
    }

}
