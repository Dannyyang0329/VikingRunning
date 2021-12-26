using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawning : MonoBehaviour
{
    public GameObject[] obstacles;

    [Range(0,1)]
    public float possibility = 0.7f;

    void Start()
    {
        if(Random.Range(0f, 1f) < possibility) {
            int index = Random.Range(0, obstacles.Length);
            Instantiate(obstacles[index], transform.position+obstacles[index].transform.position, obstacles[index].transform.rotation);
        }
    }
}
