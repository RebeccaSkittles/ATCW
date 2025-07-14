using UnityEngine;

/// <summary>
/// Dynamically sizes a logo background to fill the screen with a 10% buffer
/// Attach this to the Logo Background GameObject and call ResizeToScreen() when needed
/// </summary>
public class UILogoStartScreenSizer : MonoBehaviour
{
    [Header("Target GameObject")]
    [Tooltip("The GameObject to resize (if null, uses this GameObject)")]
    public GameObject targetObject;
    
    [Header("Settings")]
    [Tooltip("Extra scale factor - 1.1 means 10% bigger than screen")]
    public float scaleMultiplier = 1.1f;
    
    [Tooltip("Automatically resize on Start")]
    public bool resizeOnStart = true;
    
    [Tooltip("Automatically resize when screen resolution changes")]
    public bool resizeOnResolutionChange = true;
    
    private int lastScreenWidth;
    private int lastScreenHeight;
    
    void Start()
    {
        // Set target to this GameObject if not specified
        if (targetObject == null)
        {
            targetObject = gameObject;
        }
        
        // Store initial screen size
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        
        // Resize if enabled
        if (resizeOnStart)
        {
            ResizeToScreen();
        }
    }
    
    void Update()
    {
        // Check for resolution changes
        if (resizeOnResolutionChange && 
            (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight))
        {
            ResizeToScreen();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
        }
    }
    
    /// <summary>
    /// Resize the target object to fill the screen with the specified scale multiplier
    /// </summary>
    public void ResizeToScreen()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("UILogoStartScreenSizer: No target object specified");
            return;
        }
        
        // Get screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        // Apply the scale multiplier (e.g., 1.1 for 10% bigger)
        float targetWidth = screenWidth * scaleMultiplier;
        float targetHeight = screenHeight * scaleMultiplier;
        
        // Set the transform scale
        targetObject.transform.localScale = new Vector3(targetWidth, targetHeight, 1f);
        
        Debug.Log($"UILogoStartScreenSizer: Resized {targetObject.name} to {targetWidth}x{targetHeight} " +
                 $"(screen: {screenWidth}x{screenHeight}, multiplier: {scaleMultiplier})");
    }
    
    /// <summary>
    /// Set a custom scale multiplier and resize
    /// </summary>
    /// <param name="multiplier">Scale factor (1.0 = exact screen size, 1.1 = 10% bigger)</param>
    public void SetScaleAndResize(float multiplier)
    {
        scaleMultiplier = multiplier;
        ResizeToScreen();
    }
    
    /// <summary>
    /// Reset to original size (before any scaling)
    /// </summary>
    public void ResetToOriginalSize()
    {
        if (targetObject != null)
        {
            targetObject.transform.localScale = Vector3.one;
        }
    }
}
