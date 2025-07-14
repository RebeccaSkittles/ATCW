using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Makes legacy UI Text flash by animating the alpha (opacity) value
/// Attach this to a GameObject with a Text component to make it flash
/// </summary>
public class UIFlashText : MonoBehaviour
{
    [Header("Target Text")]
    [Tooltip("The Text component to flash (if null, uses this GameObject's Text component)")]
    public Text targetText;
    
    [Header("Flash Settings")]
    [Tooltip("Speed of the flash animation")]
    public float flashSpeed = 2f;
    
    [Tooltip("Minimum alpha value (0 = fully transparent)")]
    [Range(0f, 1f)]
    public float minAlpha = 0f;
    
    [Tooltip("Maximum alpha value (1 = fully opaque)")]
    [Range(0f, 1f)]
    public float maxAlpha = 1f;
    
    [Tooltip("Start flashing automatically when enabled")]
    public bool autoStart = true;
    
    [Tooltip("Flash pattern type")]
    public FlashType flashType = FlashType.Smooth;
    
    public enum FlashType
    {
        Smooth,     // Smooth sine wave fade
        Pulse,      // Sharp pulse effect
        Blink       // On/off blinking
    }
    
    private bool isFlashing = false;
    private float flashTimer = 0f;
    private Color originalColor;
    
    void Start()
    {
        // Get the Text component if not specified
        if (targetText == null)
        {
            targetText = GetComponent<Text>();
        }
        
        if (targetText == null)
        {
            Debug.LogWarning("UIFlashText: No Text component found!");
            enabled = false;
            return;
        }
        
        // Store the original color
        originalColor = targetText.color;
        
        // Start flashing if auto-start is enabled
        if (autoStart)
        {
            StartFlashing();
        }
    }
    
    void Update()
    {
        if (isFlashing && targetText != null)
        {
            flashTimer += Time.deltaTime * flashSpeed;
            
            float alpha = CalculateAlpha();
            
            // Apply the alpha to the text color
            Color newColor = originalColor;
            newColor.a = alpha;
            targetText.color = newColor;
        }
    }
    
    private float CalculateAlpha()
    {
        switch (flashType)
        {
            case FlashType.Smooth:
                // Smooth sine wave between min and max alpha
                return Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(flashTimer) + 1f) * 0.5f);
                
            case FlashType.Pulse:
                // Sharp pulse using a triangle wave
                float triangleWave = Mathf.PingPong(flashTimer, 1f);
                return Mathf.Lerp(minAlpha, maxAlpha, triangleWave);
                
            case FlashType.Blink:
                // On/off blinking
                return (Mathf.Sin(flashTimer) > 0f) ? maxAlpha : minAlpha;
                
            default:
                return maxAlpha;
        }
    }
    
    /// <summary>
    /// Start the flashing animation
    /// </summary>
    public void StartFlashing()
    {
        isFlashing = true;
        flashTimer = 0f;
    }
    
    /// <summary>
    /// Stop the flashing animation
    /// </summary>
    public void StopFlashing()
    {
        isFlashing = false;
        
        // Reset to original color
        if (targetText != null)
        {
            targetText.color = originalColor;
        }
    }
    
    /// <summary>
    /// Toggle flashing on/off
    /// </summary>
    public void ToggleFlashing()
    {
        if (isFlashing)
        {
            StopFlashing();
        }
        else
        {
            StartFlashing();
        }
    }
    
    /// <summary>
    /// Set the flash speed at runtime
    /// </summary>
    /// <param name="speed">New flash speed</param>
    public void SetFlashSpeed(float speed)
    {
        flashSpeed = speed;
    }
    
    /// <summary>
    /// Set the alpha range at runtime
    /// </summary>
    /// <param name="min">Minimum alpha value</param>
    /// <param name="max">Maximum alpha value</param>
    public void SetAlphaRange(float min, float max)
    {
        minAlpha = Mathf.Clamp01(min);
        maxAlpha = Mathf.Clamp01(max);
    }
    
    /// <summary>
    /// Change the flash type at runtime
    /// </summary>
    /// <param name="type">New flash type</param>
    public void SetFlashType(FlashType type)
    {
        flashType = type;
        flashTimer = 0f; // Reset timer for new pattern
    }
    
    void OnEnable()
    {
        if (autoStart && targetText != null)
        {
            StartFlashing();
        }
    }
    
    void OnDisable()
    {
        StopFlashing();
    }
}
