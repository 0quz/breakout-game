using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BrickScript : MonoBehaviour
{
    SpriteRenderer brickSR;
    LogicScript logicScript;
    SmallStarSpawnerScript smallStarScript;
    GameObject bricks;
    GameObject player;
    PlayerScript playerScript;
    // the bricks durabilities.
    public Dictionary<string, int> bricksDurability = new ()
    {
        {"Grey Brick(Clone)", 1},
        {"Blue Brick(Clone)", 2},
        {"Purple Brick(Clone)", 3},
        {"Green Brick(Clone)", 4},
        {"Yellow Brick(Clone)", 5},
        {"Red Brick(Clone)", 6}
    };
    public int brickDurability;
    public bool hasBonus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bricks = GameObject.Find("Bricks");
        logicScript = GameObject.Find("GameLogic").GetComponent<LogicScript>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        smallStarScript = GameObject.Find("SmallStars").GetComponent<SmallStarSpawnerScript>();
        brickSR = GetComponent<SpriteRenderer>();
        // Bonus chance %10 in brick.
        hasBonus = Random.Range(0, 100) % 10 == 0;
        // this if is not necessary.
        if (bricksDurability.ContainsKey(gameObject.name))
        {
            // set brick durability according to their color.
            brickDurability = bricksDurability[gameObject.name];
        }
    }

    // Load sprites from the resources for the bricks according to their durabilities.
    public void LoadBrickSpriteFromResources(int colorCode)
    {
        if (colorCode > 0)
        {
            var name = bricksDurability.FirstOrDefault(brickD => brickD.Value == colorCode).Key;
            Sprite newSprite = Resources.Load<Sprite>("Sprites/" + name); // Load sprite from "Resources/Sprites/MySprite.png"
            if (newSprite != null)
            {
                brickSR.sprite = newSprite;
            }
        }
    }

    // Decrease brick durability.
    public void DecreaseBrickDurability(int value)
    {
        brickDurability -= value;
    }

    // Destroy bricks and check if it has bonus.
    public void DestroyBrick(GameObject gameObject, BrickScript brickScript)
    {
        logicScript.AddScore(gameObject.name, brickScript);
        if (brickScript.hasBonus)
        {
            smallStarScript.SpawnSmallStar(gameObject);
        }
        Destroy(gameObject);
        if (bricks.transform.childCount <= 1)
        {
            playerScript.isBallMoving = !playerScript.isBallMoving;
            logicScript.NextLevel(false);
        }
    }
}
