/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPageLogic : MonoBehaviour
{
    public AudioSource clickSFX;
    public GameObject loadingWheel;
  
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        clickSFX.Play();
        Debug.Log("Starting loading animation...");
        loadingWheel.SetActive(true);

        float minimumLoadingTime = 2.0f; // Set a minimum loading time of 2 seconds
        float startTime = Time.time; // Record the time when the loading starts

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Prevent the scene from activating immediately

        // Wait for either the minimum time to pass or the scene to finish loading
        while (!asyncLoad.isDone)
        {
            // Calculate how long we've been waiting
            float elapsedTime = Time.time - startTime;

            // Check if the scene is ready to be activated and minimum time has passed
            if (asyncLoad.progress >= 0.9f && elapsedTime >= minimumLoadingTime)
            {
                // Activate the scene
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        Debug.Log("Stopping loading animation...");
        loadingWheel.SetActive(false);
    }



    void Start() 
    {
        
    }

    void Update() { }

    public void GoToPlayScene()
    {
        StartCoroutine(LoadSceneAsync("SampleScene"));
    }

    public void EndGame()
    {
        //clickSFX.Play();
        Debug.Log("Closing the game now...");
        Application.Quit();
    }

    public void Help()
    {
        clickSFX.Play();
        //SceneManager.LoadScene("HelpScene");
        StartCoroutine(LoadSceneAsync("HelpScene"));
    }
}


*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPageLogic : MonoBehaviour
{
    public AudioSource clickSFX;
    public GameObject loadingWheel;
    public Animator transitionAnimator;
    public Button playButton;// Reference to the Animator controlling the scene transition animations
    public Button helpButton;
    public Button quitButton;
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        clickSFX.Play();
        Debug.Log("Starting loading animation...");
        loadingWheel.SetActive(true);

        float minimumLoadingTime = 2.0f; // Set a minimum loading time of 2 seconds
        float startTime = Time.time; // Record the time when the loading starts

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Prevent the scene from activating immediately

        // Wait for either the minimum time to pass or the scene to finish loading
        while (!asyncLoad.isDone)
        {
            // Calculate how long we've been waiting
            float elapsedTime = Time.time - startTime;

            // Check if the scene is ready to be activated and minimum time has passed
            if (asyncLoad.progress >= 0.9f && elapsedTime >= minimumLoadingTime)
            {
                // Trigger the "start" animation
                transitionAnimator.SetTrigger("start");

                // Wait for the transition animation to finish before loading the scene
                yield return new WaitForSeconds(1f); // Adjust this to match your animation length

                // Activate the scene
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        Debug.Log("Stopping loading animation...");
        loadingWheel.SetActive(false);
    }

    void Start() { }

    void Update() { }

    public void GoToPlayScene()
    {
        StartCoroutine(LoadSceneAsync("SampleScene"));
        disableButtons();
    }

    public void EndGame()
    {
        Debug.Log("Closing the game now...");
        disableButtons();
        Application.Quit();
    }

    
    public void disableButtons ()
    {
        playButton.interactable = false;
        helpButton.interactable = false;
        quitButton.interactable = false;
    }
    
    public void Help()
    {
        
        clickSFX.Play();
        StartCoroutine(LoadSceneAsync("HelpScene"));
        disableButtons();
    }
}
