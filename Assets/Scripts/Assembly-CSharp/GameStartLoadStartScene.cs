using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles loading the StartScene when canvas is clicked/tapped
/// Fades out current scene, loads new scene additively, then cleans up
/// </summary>
public class GameStartLoadStartScene : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load")]
    public string startSceneName = "StartScene";
    
    [Header("Fade Settings")]
    [Tooltip("Duration of the fade out effect")]
    public float fadeDuration = 1f;
    
    [Tooltip("Canvas for the scene (if null, will search for one)")]
    public Canvas targetCanvas;
    
    [Tooltip("Color to fade to (usually black)")]
    public Color fadeColor = Color.black;
    
    [Header("Input Settings")]
    [Tooltip("Require click/tap input to start transition")]
    public bool waitForInput = true;
    
    [Tooltip("Auto-start after this delay (0 = disabled)")]
    public float autoStartDelay = 0f;
    
    private bool transitionStarted = false;
    private bool inputReceived = false;
    private GameObject fadeOverlay;
    
    void Start()
    {
        // Find Canvas if not assigned
        if (targetCanvas == null)
        {
            targetCanvas = GetComponent<Canvas>();
            if (targetCanvas == null)
            {
                targetCanvas = GetComponentInChildren<Canvas>();
            }
        }
        
        if (targetCanvas == null)
        {
            Debug.LogWarning("GameStartLoadStartScene: No Canvas found!");
        }
        
        // Start auto-timer if enabled
        if (autoStartDelay > 0f)
        {
            StartCoroutine(AutoStartTimer());
        }
        
        Debug.Log("GameStartLoadStartScene: Ready. " + 
                 (waitForInput ? "Waiting for input..." : "Auto-starting..."));
    }
    
    void Update()
    {
        // Check for input if waiting and transition hasn't started
        if (waitForInput && !transitionStarted && !inputReceived)
        {
            if (Input.GetMouseButtonDown(0) || 
                (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                inputReceived = true;
                StartTransition();
            }
        }
    }
    
    private IEnumerator AutoStartTimer()
    {
        yield return new WaitForSeconds(autoStartDelay);
        
        if (!transitionStarted)
        {
            StartTransition();
        }
    }
    
    /// <summary>
    /// Start the scene transition process
    /// </summary>
    public void StartTransition()
    {
        if (transitionStarted) return;
        
        transitionStarted = true;
        Debug.Log("GameStartLoadStartScene: Starting transition to " + startSceneName);
        
        StartCoroutine(TransitionToStartScene());
    }
    
    private IEnumerator TransitionToStartScene()
    {
        // Phase 1: Fade out current scene
        yield return StartCoroutine(FadeOut());
        
        // Phase 2: Load the start scene additively
        Debug.Log("GameStartLoadStartScene: Loading " + startSceneName);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(startSceneName, LoadSceneMode.Additive);
        
        // Wait for scene to load
        while (!loadOperation.isDone)
        {
            yield return null;
        }
        
        // Phase 3: Set the new scene as active
        Scene loadedScene = SceneManager.GetSceneByName(startSceneName);
        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);
            Debug.Log("GameStartLoadStartScene: Set " + startSceneName + " as active scene");
        }
        
        // Phase 4: Unload current scene
        Scene currentScene = gameObject.scene;
        if (currentScene.IsValid() && currentScene.name != startSceneName)
        {
            Debug.Log("GameStartLoadStartScene: Unloading current scene: " + currentScene.name);
            yield return SceneManager.UnloadSceneAsync(currentScene);
        }
        
        // Phase 5: Clean up this object (it should be destroyed with the scene)
        Debug.Log("GameStartLoadStartScene: Transition complete. Destroying component.");
        Destroy(this);
    }
    
    private IEnumerator FadeOut()
    {
        if (targetCanvas == null)
        {
            Debug.LogWarning("GameStartLoadStartScene: No Canvas for fade effect");
            yield return new WaitForSeconds(fadeDuration);
            yield break;
        }
        
        // Create a fade overlay
        CreateFadeOverlay();
        
        if (fadeOverlay != null)
        {
            UnityEngine.UI.Image fadeImage = fadeOverlay.GetComponent<UnityEngine.UI.Image>();
            float elapsedTime = 0f;
            
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / fadeDuration;
                
                // Fade from transparent to opaque
                Color newColor = fadeColor;
                newColor.a = Mathf.Lerp(0f, 1f, progress);
                fadeImage.color = newColor;
                
                yield return null;
            }
            
            // Ensure fully opaque
            Color finalColor = fadeColor;
            finalColor.a = 1f;
            fadeImage.color = finalColor;
        }
        
        Debug.Log("GameStartLoadStartScene: Fade out complete");
    }
    
    private void CreateFadeOverlay()
    {
        if (targetCanvas == null) return;
        
        // Create fade overlay GameObject
        fadeOverlay = new GameObject("FadeOverlay");
        fadeOverlay.transform.SetParent(targetCanvas.transform, false);
        
        // Add Image component for the fade effect
        UnityEngine.UI.Image fadeImage = fadeOverlay.AddComponent<UnityEngine.UI.Image>();
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f); // Start transparent
        
        // Set up RectTransform to fill the screen
        RectTransform rectTransform = fadeOverlay.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
        
        // Make sure it's on top
        fadeOverlay.transform.SetAsLastSibling();
    }
    
    /// <summary>
    /// Public method to trigger transition (for UI events)
    /// </summary>
    public void OnCanvasClicked()
    {
        StartTransition();
    }
    
    /// <summary>
    /// Alternative method name for UI events
    /// </summary>
    public void LoadStartScene()
    {
        StartTransition();
    }
    
    void OnDestroy()
    {
        // Stop all coroutines when destroyed
        StopAllCoroutines();
    }
}
