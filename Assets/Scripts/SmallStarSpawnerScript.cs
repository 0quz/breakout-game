using UnityEngine;

public class SmallStarSpawnerScript : MonoBehaviour
{
    public GameObject smallStar;
    public void SpawnSmallStar(GameObject gameObject)
    {
        // Spawn small stars when the brick is broken.
        Instantiate(smallStar, gameObject.transform.position, Quaternion.identity, transform);
    }
}
