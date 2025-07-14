using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that can be attached to the event system to toggle tutorial skipping for development purposes.
/// When enabled, it tricks the game into thinking all tutorials have been completed.
/// When disabled, it restores the original tutorial state.
/// </summary>
public class DevTutorialSkipper : MonoBehaviour
{
    [Header("Tutorial Skip Settings")]
    [SerializeField]
    [Tooltip("Enable this to skip all tutorials")]
    private bool skipTutorials = false;
    
    [SerializeField]
    [Tooltip("When enabled, also sets the DebugFlags stopTutorial flag")]
    private bool useDebugFlags = true;
    
    [Header("Debug Info")]
    [SerializeField]
    [Tooltip("Shows current tutorial skip state (read-only)")]
    private bool currentlySkipping = false;
    
    // Backup of original tutorial state
    private List<string> originalTutorialsCompleted = new List<string>();
    private bool originalStopTutorialFlag = false;
    private bool hasBackedUp = false;
    
    // All tutorial IDs that can be completed (based on the tutorial database)
    private readonly string[] allTutorialIds = {
        "B1", "B2", "B2.2", "B2.5", "B2.6", "A28", "A29", "A30", "A31", "A32", "A33", "A34", "A35",
        "A36", "A37", "A38", "A39", "A40", "A41", "A42", "A43", "A44", "A45", "A46", "A47", "A48",
        "A49", "A50", "A51", "A52", "A53", "A54", "A55", "A56", "A57", "A58", "A59", "A60", "A61",
        "A62", "A63", "A64", "A65", "A66", "A67", "A68", "A69", "A70", "A71", "A72", "A73", "A74",
        "A75", "A76", "A77", "A78", "A79", "A80", "A81", "A82", "A83", "A84", "A85", "A86", "A87",
        "A88", "A89", "A90", "A91", "A92", "A93", "A94", "A95", "A96", "A97", "A98", "A99", "A100",
        "FIRSTQUEST", "DECKMANAGER", "CARDCRAFTING", "MAINMENU"
    };
    
    private void Start()
    {
        // Apply the current setting on start
        if (skipTutorials)
        {
            EnableTutorialSkip();
        }
    }
    
    private void Update()
    {
        // Check if the setting has changed and apply it
        if (skipTutorials && !currentlySkipping)
        {
            EnableTutorialSkip();
        }
        else if (!skipTutorials && currentlySkipping)
        {
            DisableTutorialSkip();
        }
    }
    
    /// <summary>
    /// Enables tutorial skipping by marking all tutorials as completed
    /// </summary>
    public void EnableTutorialSkip()
    {
        PlayerInfoScript playerInfo = PlayerInfoScript.GetInstance();
        if (playerInfo == null)
        {
            Debug.LogWarning("DevTutorialSkipper: PlayerInfoScript not found!");
            return;
        }
        
        // Backup original state if we haven't already
        if (!hasBackedUp)
        {
            BackupOriginalState();
        }
        
        // Mark all tutorials as completed
        foreach (string tutorialId in allTutorialIds)
        {
            if (!playerInfo.tutorialsCompleted.Contains(tutorialId))
            {
                playerInfo.tutorialsCompleted.Add(tutorialId);
            }
        }
        
        // Set debug flags if enabled
        if (useDebugFlags)
        {
            DebugFlagsScript debugFlags = DebugFlagsScript.GetInstance();
            if (debugFlags != null)
            {
                debugFlags.stopTutorial = true;
            }
        }
        
        // Save the changes
        playerInfo.Save();
        
        currentlySkipping = true;
        Debug.Log("DevTutorialSkipper: Tutorial skipping ENABLED - All tutorials marked as completed");
    }
    
    /// <summary>
    /// Disables tutorial skipping by restoring the original tutorial state
    /// </summary>
    public void DisableTutorialSkip()
    {
        PlayerInfoScript playerInfo = PlayerInfoScript.GetInstance();
        if (playerInfo == null)
        {
            Debug.LogWarning("DevTutorialSkipper: PlayerInfoScript not found!");
            return;
        }
        
        // Restore original tutorial state if we have a backup
        if (hasBackedUp)
        {
            playerInfo.tutorialsCompleted.Clear();
            playerInfo.tutorialsCompleted.AddRange(originalTutorialsCompleted);
            
            // Restore debug flags if enabled
            if (useDebugFlags)
            {
                DebugFlagsScript debugFlags = DebugFlagsScript.GetInstance();
                if (debugFlags != null)
                {
                    debugFlags.stopTutorial = originalStopTutorialFlag;
                }
            }
            
            // Save the changes
            playerInfo.Save();
            
            currentlySkipping = false;
            Debug.Log("DevTutorialSkipper: Tutorial skipping DISABLED - Original tutorial state restored");
        }
        else
        {
            Debug.LogWarning("DevTutorialSkipper: No backup available! Cannot restore original tutorial state.");
        }
    }
    
    /// <summary>
    /// Backs up the original tutorial completion state
    /// </summary>
    private void BackupOriginalState()
    {
        PlayerInfoScript playerInfo = PlayerInfoScript.GetInstance();
        if (playerInfo != null)
        {
            // Backup tutorial completion list
            originalTutorialsCompleted.Clear();
            originalTutorialsCompleted.AddRange(playerInfo.tutorialsCompleted);
            
            // Backup debug flags
            if (useDebugFlags)
            {
                DebugFlagsScript debugFlags = DebugFlagsScript.GetInstance();
                if (debugFlags != null)
                {
                    originalStopTutorialFlag = debugFlags.stopTutorial;
                }
            }
            
            hasBackedUp = true;
            Debug.Log($"DevTutorialSkipper: Backed up original tutorial state ({originalTutorialsCompleted.Count} tutorials completed)");
        }
    }
    
    /// <summary>
    /// Toggles tutorial skipping on/off
    /// </summary>
    [ContextMenu("Toggle Tutorial Skip")]
    public void ToggleTutorialSkip()
    {
        skipTutorials = !skipTutorials;
        Debug.Log($"DevTutorialSkipper: Tutorial skip toggled to {(skipTutorials ? "ENABLED" : "DISABLED")}");
    }
    
    /// <summary>
    /// Force enable tutorial skipping (for button/event binding)
    /// </summary>
    [ContextMenu("Enable Tutorial Skip")]
    public void ForceEnableTutorialSkip()
    {
        skipTutorials = true;
    }
    
    /// <summary>
    /// Force disable tutorial skipping (for button/event binding)
    /// </summary>
    [ContextMenu("Disable Tutorial Skip")]
    public void ForceDisableTutorialSkip()
    {
        skipTutorials = false;
    }
    
    /// <summary>
    /// Prints current tutorial completion status to console
    /// </summary>
    [ContextMenu("Debug: Print Tutorial Status")]
    public void PrintTutorialStatus()
    {
        PlayerInfoScript playerInfo = PlayerInfoScript.GetInstance();
        if (playerInfo != null)
        {
            Debug.Log($"DevTutorialSkipper: Currently skipping tutorials: {currentlySkipping}");
            Debug.Log($"DevTutorialSkipper: Tutorials completed: {playerInfo.tutorialsCompleted.Count}");
            Debug.Log($"DevTutorialSkipper: Has backup: {hasBackedUp}");
            Debug.Log($"DevTutorialSkipper: Backup count: {originalTutorialsCompleted.Count}");
            
            if (useDebugFlags)
            {
                DebugFlagsScript debugFlags = DebugFlagsScript.GetInstance();
                if (debugFlags != null)
                {
                    Debug.Log($"DevTutorialSkipper: Debug stopTutorial flag: {debugFlags.stopTutorial}");
                }
            }
        }
    }
}
