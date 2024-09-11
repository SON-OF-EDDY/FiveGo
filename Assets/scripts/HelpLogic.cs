using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HelpLogic : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource clickSFX;
    public GameObject loadingWheel;
    public Animator transitionAnimator;
    public Button backButton; // Reference to the Animator controlling the scene transition animations

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



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToMainMenu ()
    {
        clickSFX.Play();
        backButton.interactable = false;
        //SceneManager.LoadScene("Title Page");
        StartCoroutine(LoadSceneAsync("Title Page"));
    }
}
