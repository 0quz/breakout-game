using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    AudioSource paddleHitAudio;
    GameObject ball;
    Rigidbody2D ballRb2D;
    LogicScript logicScript;
    float mouseX = 0;
    float playerSpeed = 3.0f;
    float ballSpeed = 9f;
    public bool isBallMoving = false;
    public bool startGame = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ball = GameObject.Find("Ball");
        logicScript = GameObject.Find("GameLogic").GetComponent<LogicScript>();
        ballRb2D = ball.GetComponent<Rigidbody2D>();
        paddleHitAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the ball is not moving, move the ball position onto the player's paddle.
        if (!isBallMoving)
        {
            ball.transform.position = new Vector2(transform.position.x, transform.position.y + 0.4f);
        }

        if (startGame)
        {
            // get mouse x positon
            mouseX += Input.GetAxis("Mouse X") / playerSpeed;
            // set player positon as same as mouse x positon.
            transform.position = new Vector2(mouseX, transform.position.y);
            // Left mouse click releases the ball.
            if (Input.GetMouseButtonDown(0) && !isBallMoving)
            {
                isBallMoving = !isBallMoving;
                ballRb2D.linearVelocity = new Vector2(mouseX, ballSpeed);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // make paddle hit sound when ball hits the paddle.
        paddleHitAudio.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // applies the attribute effect when a small Star hits the paddle.
        logicScript.ApplyAttribute(collision.gameObject);
    }
}
