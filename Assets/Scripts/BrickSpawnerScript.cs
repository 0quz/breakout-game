using UnityEngine;

public class BrickSpawnerScript : MonoBehaviour
{
    GameObject brick;
    private readonly string[] bricks = {"Blue", "Green", "Purple", "Grey", "Red", "Yellow"};

    // change all bricks collider isTrigger status
    public void ChangeAllChildBoxColliderIsTriggerStatus(bool isTrigger)
    {
        BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D col in colliders)
        {
            col.isTrigger = isTrigger;
        }
    }


    // Generate Level
    public void DrawCircle(int radius, int position)
    {
        float diameter = radius * 2;

        float cx = radius + position, cy = radius; // Center
        for (float y = 0; y < diameter + 1; y++)
        {
            brick = Resources.Load<GameObject>("Prefabs/" + bricks[Random.Range(0, 5)] + " Brick");
            for (float x = position; x < diameter + position + 1; x++)
            {
                float dx = x - cx;
                float dy = y - cy;
                if (dx * dx + dy * dy <= radius * radius)
                {
                    Instantiate(brick, new Vector2(x, y), Quaternion.identity, transform);
                }
            }
        }
    }

    // Generate Level
    public void DrawRectangle(int amountOfHorizontalBrick)
    {
        for (float x = -amountOfHorizontalBrick; x < amountOfHorizontalBrick; x += 1f)
        {
            brick = Resources.Load<GameObject>("Prefabs/" + bricks[Random.Range(0, 5)] + " Brick");
            for (float y = 0; y < 5f; y += 0.5f)
            {
                Instantiate(brick, new Vector2(x, y), Quaternion.identity, transform);
            }
        }
    }
    // Generate Level
    public void DrawX(int size)
    {
        if (size < 2)
        {
            return;
        }

        for (float i = -size; i <= size; i += 0.5f)
        {
            brick = Resources.Load<GameObject>("Prefabs/" + bricks[Random.Range(0, 5)] + " Brick");
            Instantiate(brick, new Vector2(i, i), Quaternion.identity, transform); // Left-to-right diagonal (\)
            Instantiate(brick, new Vector2(i, -i), Quaternion.identity, transform); // Right-to-left diagonal (/)
        }

    }
}
