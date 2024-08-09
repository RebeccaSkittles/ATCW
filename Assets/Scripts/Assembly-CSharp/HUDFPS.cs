using UnityEngine;
using UnityEngine.UI; // Add this for the Text component

public class HUDFPS : MonoBehaviour
{
    public float updateInterval = 0.5f;

    private float accum;
    private int frames;
    private float timeleft;

    private Text uiText; // Replace GUIText with Text
    private TextMesh textMesh;
    private UILabel uiLabel;

    private void Start()
    {
        if (GetComponent<Text>() != null)
        {
            uiText = GetComponent<Text>();
        }
        if (GetComponent<TextMesh>() != null)
        {
            textMesh = GetComponent<TextMesh>();
        }
        if (GetComponent<UILabel>() != null)
        {
            uiLabel = GetComponent<UILabel>();
        }
        timeleft = updateInterval;
    }

    private void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            string text = string.Format("{0:F2} FPS", fps);

            if (uiText != null)
            {
                uiText.text = text;
            }
            if (textMesh != null)
            {
                textMesh.text = text;
            }
            if (uiLabel != null)
            {
                uiLabel.text = text;
            }

            if (fps < 30f)
            {
                SetTextColor(Color.yellow);
            }
            else if (fps < 10f)
            {
                SetTextColor(Color.red);
            }
            else
            {
                SetTextColor(Color.green);
            }

            timeleft = updateInterval;
            accum = 0f;
            frames = 0;
        }
    }

    private void SetTextColor(Color color)
    {
        if (uiText != null)
        {
            uiText.color = color;
        }
        if (uiLabel != null)
        {
            uiLabel.color = color;
        }
        // Note: TextMesh color is not changed as it might be using a shared material
    }
}
