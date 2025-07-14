using UnityEngine;

public class SettingCogSpinAni : MonoBehaviour
{
    [Header("Spin Settings")]
    [Tooltip("Speed of rotation in degrees per second")]
    public float spinSpeed = 30f;
    
    [Header("Rotation Axes")]
    [Tooltip("Rotate around Y axis")]
    public bool rotateY = true;
    
    [Tooltip("Rotate around Z axis")]
    public bool rotateZ = true;
    
    [Header("Direction")]
    [Tooltip("If true, rotates to the left (negative direction). If false, rotates to the right")]
    public bool rotateLeft = true;
    
    private Transform parentTransform;
    private bool isParentVisible;
    
    private void Start()
    {
        // Get the parent transform
        parentTransform = transform.parent;
        
        if (parentTransform == null)
        {
            Debug.LogWarning("SettingCogSpinAni: No parent object found! Animation will always run.");
        }
    }
    
    private void Update()
    {
        // Check if parent is visible
        CheckParentVisibility();
        
        // Only animate if parent is visible
        if (isParentVisible)
        {
            PerformRotation();
        }
    }
    
    private void CheckParentVisibility()
    {
        if (parentTransform == null)
        {
            // If no parent, always animate
            isParentVisible = true;
            return;
        }
        
        // Check if parent GameObject is active in hierarchy
        isParentVisible = parentTransform.gameObject.activeInHierarchy;
        
        // Additional check for NGUI visibility if parent has UIWidget component
        UIWidget parentWidget = parentTransform.GetComponent<UIWidget>();
        if (parentWidget != null)
        {
            isParentVisible = isParentVisible && parentWidget.isVisible;
        }
        
        // Additional check for NGUI panel visibility
        UIPanel parentPanel = parentTransform.GetComponent<UIPanel>();
        if (parentPanel != null)
        {
            isParentVisible = isParentVisible && parentPanel.enabled;
        }
    }
    
    private void PerformRotation()
    {
        // Calculate rotation amount for this frame
        float rotationAmount = spinSpeed * Time.deltaTime;
        
        // Apply direction (left = negative rotation)
        if (rotateLeft)
        {
            rotationAmount = -rotationAmount;
        }
        
        // Create rotation vector
        Vector3 rotation = Vector3.zero;
        
        if (rotateY)
        {
            rotation.y = rotationAmount;
        }
        
        if (rotateZ)
        {
            rotation.z = rotationAmount;
        }
        
        // Apply the rotation
        transform.Rotate(rotation);
    }
    
    /// <summary>
    /// Public method to change spin speed at runtime
    /// </summary>
    /// <param name="newSpeed">New rotation speed in degrees per second</param>
    public void SetSpinSpeed(float newSpeed)
    {
        spinSpeed = newSpeed;
    }
    
    /// <summary>
    /// Public method to toggle rotation direction
    /// </summary>
    public void ToggleDirection()
    {
        rotateLeft = !rotateLeft;
    }
    
    /// <summary>
    /// Public method to pause/resume animation regardless of parent visibility
    /// </summary>
    public void SetAnimationEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}
