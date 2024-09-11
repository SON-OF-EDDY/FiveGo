using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator transitionAnim;
    private static SceneTransition instance;
    private void Awake()
    {
        // Ensure there's only one instance of this object
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy the duplicate instance
        }
    }


    void Start()
    {
        //StartCoroutine(FadeAndLoadScene());
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
