using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static float survivalTime = 0;
    public static float coinN = 0;

    public GameObject[] blocks;
    public AudioManager audioManager;

    public Text coinText;
    public Text survivalText;

    void Start()
    {
        audioManager.Play("GameBgm");
        
    }

    void Update()
    {
        survivalTime += Time.deltaTime;
        coinText.text = coinN.ToString();
        survivalText.text = ((int)survivalTime).ToString();
    }

    public void SpawnNewBlock(Vector3 pos, Quaternion rot) {
        int index = Random.Range(0, blocks.Length);

        Instantiate(blocks[index], pos, rot);
    }
}
