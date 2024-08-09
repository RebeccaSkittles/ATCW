using UnityEngine;
using UnityEngine.UI;

public class NcDrawFpsText : MonoBehaviour
{
    public float updateInterval = 0.5f;

    private float accum;
    private int frames;
    private float timeleft;
    private Text fpsText;

    private void Start()
    {
        fpsText = GetComponent<Text>();
        if (!fpsText)
        {
            base.enabled = false;
        }
        else
        {
            timeleft = updateInterval;
        }
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
            fpsText.text = text;

            if (fps < 30f)
            {
                fpsText.color = Color.yellow;
            }
            else if (fps < 10f)
            {
                fpsText.color = Color.red;
            }
            else
            {
                fpsText.color = Color.green;
            }

            timeleft = updateInterval;
            accum = 0f;
            frames = 0;
        }
    }
}
