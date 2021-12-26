using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float survivalTime = 0;
    public static float coinN = 0;

    public GameObject[] blocks;
    public AudioManager audioManager;

    public Text coinText;
    public Text survivalText;

    public GameObject menu;
    public GameObject gameover;

    void Start()
    {
        Time.timeScale = 1;

        audioManager.Play("GameBgm");
        menu.SetActive(false);
        gameover.SetActive(false);
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

    public void showMenu() {
        menu.SetActive(true);
        Time.timeScale = 0;
    }
    public void continueGame() {
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void restartGame() {
        SceneManager.LoadScene("GameScene");
        coinN = 0;
        survivalTime = 0;
        Time.timeScale = 1;
    }
    public void backToHome() {
        SceneManager.LoadScene("Menu");
        coinN = 0;
        survivalTime = 0;
        Time.timeScale = 1;
    }
}
