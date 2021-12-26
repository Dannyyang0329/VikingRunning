using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject coin;
    [Range(0, 1)]
    public float spawnPossibility = 0.1f;

    void Start()
    {
        Vector3 offset = Vector3.up * 2;

        if(Random.Range(0f,1f) < spawnPossibility)
            Instantiate(coin, transform.position + offset, coin.transform.rotation);
    }
}
