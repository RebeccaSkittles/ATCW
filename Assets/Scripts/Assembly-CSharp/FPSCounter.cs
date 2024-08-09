using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Add this for the Text component

public class FPSCounter : MonoBehaviour
{
    public float frequency = 0.5f;
    public UISprite bar;

    private Text uiText; // Replace GUIText with Text
    private UILabel uiLabel;
    private TextMesh textMesh;

    public float FramesPerSec { get; protected set; }

    private void OnEnable()
    {
        uiText = GetComponent<Text>();
        textMesh = GetComponent<TextMesh>();
        uiLabel = GetComponent<UILabel>();
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        while (true)
        {
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;
            FramesPerSec = (float)frameCount / timeSpan;

            UpdateFPSText();
            UpdateFPSColor();

            if (bar != null)
            {
                bar.fillAmount = FramesPerSec / 60f;
            }
        }
    }

    private void UpdateFPSText()
    {
        string fpsText = string.Format("{0:F1} FPS", FramesPerSec);
        if (uiText != null) uiText.text = fpsText;
        if (textMesh != null) textMesh.text = fpsText;
        if (uiLabel != null) uiLabel.text = fpsText;
    }

    private void UpdateFPSColor()
    {
        Color fpsColor = FramesPerSec >= 30f ? Color.green : (FramesPerSec >= 10f ? Color.yellow : Color.red);

        if (uiText != null) uiText.color = fpsColor;
        if (uiLabel != null) uiLabel.color = fpsColor;
        if (bar != null) bar.color = fpsColor;
    }
}
