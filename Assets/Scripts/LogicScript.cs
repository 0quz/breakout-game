using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class LogicScript : MonoBehaviour
{
    Text score;
    Text highScore;
    Button playAgain;
    Button startGame;
    Text displayAttribute;
    GameObject ball;
    GameObject bricks;
    PlayerScript playerScript;
    BrickSpawnerScript brickSpawnerScript;
    Rigidbody2D ballRb2D;
    GameObject player;
    AudioSource gameOverSound;
    int playerScore = 0;
    int currentLevel = 1;
    public bool gameOver = false;
    string bonusAttribute;
    string[] bonusAttributes = { "biggerPeddle", "smallerPeddle", "slowerBall", "fasterBall", "ignoreBricks" };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ball = GameObject.Find("Ball");
        ballRb2D = ball.GetComponent<Rigidbody2D>();
        bricks = GameObject.Find("Bricks");
        brickSpawnerScript = bricks.GetComponent<BrickSpawnerScript>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        score = GameObject.Find("Canvas/Score").GetComponent<Text>();
        displayAttribute = GameObject.Find("Canvas/DisplayAttribute").GetComponent<Text>();
        highScore = GameObject.Find("Canvas/HighScore").GetComponent<Text>();
        highScore.text = "High Score: " + PlayerPrefs.GetInt("highScore", 0).ToString();
        startGame = GameObject.Find("Canvas/StartGameButton").GetComponent<Button>();
        playAgain = GameObject.Find("Canvas/PlayAgainButton").GetComponent<Button>();
        playAgain.gameObject.SetActive(!playAgain.gameObject.activeSelf);
        gameOverSound = GetComponent<AudioSource>();
    }

    // add score when the ball break the brick.
    public void AddScore(string brickType, BrickScript brickScript)
    {
        playerScore += brickScript.bricksDurability[brickType] * 10;
        score.text = "Score: " + playerScore.ToString();
        if (PlayerPrefs.GetInt("highScore", 0) < playerScore) {
            PlayerPrefs.SetInt("highScore", playerScore);
            highScore.text = "High Score: " + playerScore.ToString();
        }
    }

    // Display attribute name and effective time.
    public void DisplayBonus(float timer, string attributeName, bool display)
    {
        if (display) {
            displayAttribute.text = "Attribute: " + attributeName + "\nTime: " + timer.ToString();
        } else
        {
            displayAttribute.text = "";
        }
    }


    // This method can be written better. But now it stays like this.
    // It loads next level.
    public void NextLevel(bool startFirst) {
        if (startFirst || currentLevel > 3) {
            currentLevel = 1;
        }
        if (currentLevel == 1)
        {
            brickSpawnerScript.DrawRectangle(3);
        }
        else if (currentLevel == 2)
        {
            brickSpawnerScript.DrawCircle(2, -6);
            brickSpawnerScript.DrawCircle(2, 2);
        }
        else if (currentLevel == 3)
        {
            brickSpawnerScript.DrawX(3);
        }
        currentLevel += 1;
    }

    public void PlayAgain()
    {
        ShowMouseCursor(false);
        NextLevel(true);
        playerScore = 0;
        score.text = "Score: " + playerScore.ToString();
        RePositionText(score, -974, 644);
        RePositionText(highScore, 952, 644);
        ball.transform.position = new Vector2(transform.position.x, transform.position.y + 0.3f);
        playerScript.isBallMoving = !playerScript.isBallMoving;
        playerScript.startGame = !playerScript.startGame;
        gameOver = !gameOver;
        playAgain.gameObject.SetActive(!playAgain.gameObject.activeSelf);
    }

    public void DisplayGameOverMenu()
    {
        ShowMouseCursor(true);
        gameOverSound.Play();
        gameOver = !gameOver;
        RemoveAllChildrenFromParent();
        playAgain.gameObject.SetActive(!playAgain.gameObject.activeSelf);
        RePositionText(score, 0, -100);
        RePositionText(highScore, 0, -200);
        playerScript.startGame = !playerScript.startGame;

    }

    public void StartGame()
    {
        playerScript.startGame = !playerScript.startGame;
        startGame.gameObject.SetActive(!startGame.gameObject.activeSelf);
        NextLevel(true);
        ShowMouseCursor(false);
    }

    // Removes whole bricks.
    public void RemoveAllChildrenFromParent()
    {
        for (int i = bricks.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(bricks.transform.GetChild(i).gameObject);
        }
    }

    private void RePositionText(Text text, int x, int y)
    {
        text.rectTransform.anchoredPosition = new Vector2(x, y);
    }

    // This method can be written better. But now it stays like this.
    public void ApplyAttribute(GameObject gameObject)
    {
        float timer = 10f;
        bonusAttribute = bonusAttributes[Random.Range(0, bonusAttributes.Length)];
        if (bonusAttribute == "biggerPeddle")
        {
            player.transform.localScale = new Vector2(3, 1);
        }
        else if (bonusAttribute == "smallerPeddle")
        {
            player.transform.localScale = new Vector2(1, 1);
        }
        else if (bonusAttribute == "slowerBall")
        {
            ballRb2D.linearVelocity = ballRb2D.linearVelocity / 1.5f;
        }
        else if (bonusAttribute == "fasterBall")
        {
            ballRb2D.linearVelocity = ballRb2D.linearVelocity * 1.5f;
        }
        else if (bonusAttribute == "ignoreBricks")
        {
            timer = 3f;
            brickSpawnerScript.ChangeAllChildBoxColliderIsTriggerStatus(true);
        };
        // Destroys attribute effect after the timer ends.
        DisplayBonus(timer, bonusAttribute, true);
        StartCoroutine(ExecuteAfterTime(timer, bonusAttribute));
        Destroy(gameObject);
    }

    private IEnumerator ExecuteAfterTime(float delay, string bonusAttribute)
    {
        yield return new WaitForSeconds(delay);
        if (bonusAttribute == "ignoreBricks")
        {
            brickSpawnerScript.ChangeAllChildBoxColliderIsTriggerStatus(false);
        }
        else if (bonusAttribute == "fasterBall")
        {
            ballRb2D.linearVelocity = ballRb2D.linearVelocity / 1.5f;
        }
        else if (bonusAttribute == "slowerBall")
        {
            ballRb2D.linearVelocity = ballRb2D.linearVelocity * 1.5f;
        }
        else if (bonusAttribute == "smallerPeddle" || bonusAttribute == "biggerPeddle")
        {
            player.transform.localScale = new Vector2(2, 1);
        }
        DisplayBonus(delay, bonusAttribute, false);
    }

    public void ShowMouseCursor(bool show) {
        Cursor.visible = show;
    }
}
