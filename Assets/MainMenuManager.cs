using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //private const int SHIFT_AMOUNT = 560;
    private const string LEVEL_TO_LOAD = "MainScene";
    public Animator screensAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   /*Transitioning into credit or instruction screen from main, everything shifts to the left*/
    public void ShiftFromMain()
    {
        screensAnim.SetTrigger("shiftLeft");
    }

    /*Transitioning from credit or instruction screen to main, everything shifts to the right*/
    public void ShiftToMain()
    {
        screensAnim.SetTrigger("shiftRight");
    }

    public void Play()
    {
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        //add any animations for fading out and yield for the duration of that animation
        yield return null;
        SceneManager.LoadScene(LEVEL_TO_LOAD);
    }

    public void Quit()
    {
        StartCoroutine(QuitCoroutine());
    }

    private IEnumerator QuitCoroutine()
    {
        //add any animations for fading out and yield for the duration of that animation
        yield return null;
        Application.Quit();
    }
}
