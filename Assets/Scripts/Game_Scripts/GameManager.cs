using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] blocks;
    public AudioManager audioManager;

    void Start()
    {
        audioManager.Play("GameBgm");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNewBlock(Vector3 pos, Quaternion rot) {
        int index = Random.Range(0, blocks.Length);

        Instantiate(blocks[index], pos, rot);
    }
}
