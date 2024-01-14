using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls animations and transitions betwen pseudoscreens and levels for main menu
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    private const string LEVEL_TO_LOAD = "MainScene";
    public Animator screensAnim;
    public Animator fadeAnim;
    public GameObject infoScreen;
    public GameObject creditsScreen;

   /*Transitioning into credit or instruction screen from main, everything shifts to the left*/
    public void ShiftFromMain()
    {
        StartCoroutine(ShiftFromMainCoroutine());
    }

    /*Plays shifting animation */
    private IEnumerator ShiftFromMainCoroutine()
    {
        FindObjectOfType<AudioSource>().Play();
        screensAnim.SetTrigger("shiftLeft");
        yield return null;
        yield return new WaitForSeconds(screensAnim.GetCurrentAnimatorStateInfo(0).length);

    }

    /*Transitioning from credit or instruction screen to main, everything shifts to the right*/
    public void ShiftToMain()
    {
        StartCoroutine(ShiftToMainCoroutine());
    }


    /*Plays shifting animation */
    private IEnumerator ShiftToMainCoroutine()
    {
        FindObjectOfType<AudioSource>().Play();
        screensAnim.SetTrigger("shiftRight");
        yield return null;
        yield return new WaitForSeconds(screensAnim.GetCurrentAnimatorStateInfo(0).length);
        creditsScreen.gameObject.SetActive(false);
        infoScreen.gameObject.SetActive(false);
    }
    public void Play()
    {
        FindObjectOfType<AudioSource>().Play();
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        fadeAnim.SetTrigger("fadeOut");
        yield return null;
        yield return new WaitForSeconds(fadeAnim.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(LEVEL_TO_LOAD);
    }

    public void Quit()
    {
        FindObjectOfType<AudioSource>().Play();
        StartCoroutine(QuitCoroutine());
    }

    private IEnumerator QuitCoroutine()
    {
        fadeAnim.SetTrigger("fadeOut");
        yield return null;
        yield return new WaitForSeconds(fadeAnim.GetCurrentAnimatorStateInfo(0).length);
        Application.Quit();
    }
}
