using UnityEngine;

public class BallScript : MonoBehaviour
{
    BrickScript brickScript;
    AudioSource brickHitSource;
    LogicScript logicScript;
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        logicScript = GameObject.Find("GameLogic").GetComponent<LogicScript>();
        brickHitSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the ball position below the player postion then display game over menu
        if (transform.position.y < player.transform.position.y && !logicScript.gameOver) {
            logicScript.DisplayGameOverMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ignore brick durability if isTrigger status is active on bricks.
        if (collision.gameObject.name.Contains("Brick"))
        {
            brickScript.DestroyBrick(collision.gameObject, collision.gameObject.GetComponent<BrickScript>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // when the ball hits the brick play the hit song, decrease the brick durability and change the brick color.
        if (collision.gameObject.name.Contains("Brick"))
        {
            brickHitSource.Play();
            brickScript = collision.gameObject.GetComponent<BrickScript>();
            brickScript.DecreaseBrickDurability(1);
            brickScript.LoadBrickSpriteFromResources(brickScript.brickDurability);
            // if brick durability is 0 destroy the brick.
            if (brickScript.brickDurability <= 0)
            {
                brickScript.DestroyBrick(collision.gameObject, brickScript);
            }
        }
    }
}
