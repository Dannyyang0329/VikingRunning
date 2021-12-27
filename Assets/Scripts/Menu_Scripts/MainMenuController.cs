using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour 
{
    public AudioManager audioManager;
    public GameObject tutorial;

    private void Start() 
    {
        audioManager.Play("Bgm");
        HideTutorialImage();
    }

    public void Quit() 
    {
        Application.Quit();
    }

    public void LoadGame() 
    {
        audioManager.Stop("Bgm");
        SceneManager.LoadScene("GameScene");
    }


    public void HoverSound() 
    {
        audioManager.Play("HoverSound");
    }

    public void ShowTutorialImage() {
        tutorial.SetActive(true);
        Invoke("HideTutorialImage", 5);
    }

    public void HideTutorialImage() {
        tutorial.SetActive(false);
    }
}
