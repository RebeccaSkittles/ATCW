using UnityEngine;

[RequireComponent(typeof(FingerDownDetector))]
[RequireComponent(typeof(ScreenRaycaster))]
[RequireComponent(typeof(FingerUpDetector))]
[RequireComponent(typeof(FingerHoverDetector))]
[RequireComponent(typeof(FingerMotionDetector))]
public class FingerEventsSamplePart1 : SampleBase
{
    public GameObject fingerDownObject;
    public GameObject fingerStationaryObject;
    public GameObject fingerHoverObject;
    public GameObject fingerUpObject;

    public float chargeDelay = 0.5f;
    public float chargeTime = 5f;
    public float minSationaryParticleEmissionCount = 5f;
    public float maxSationaryParticleEmissionCount = 50f;
    public Material highlightMaterial;

    private int stationaryFingerIndex = -1;
    private Material originalStationaryMaterial;
    private Material originalHoverMaterial;
    private ParticleSystem stationaryParticleSystem;

	private void OnFingerDown(FingerDownEvent e)
	{
		if (e.Selection == fingerDownObject)
		{
			SpawnParticles(fingerDownObject);
		}
	}

	private void OnFingerUp(FingerUpEvent e)
	{
		if (e.Selection == fingerUpObject)
		{
			SpawnParticles(fingerUpObject);
		}
		FingerGestures.Finger finger = e.Finger;
	}

	private void OnFingerHover(FingerHoverEvent e)
	{
		if (e.Selection == fingerHoverObject)
		{
			if (e.Phase == FingerHoverPhase.Enter)
			{
				base.UI.StatusText = "Finger entered " + fingerHoverObject.name;
				originalHoverMaterial = fingerHoverObject.GetComponent<Renderer>().sharedMaterial;
				fingerHoverObject.GetComponent<Renderer>().sharedMaterial = highlightMaterial;
			}
			else if (e.Phase == FingerHoverPhase.Exit)
			{
				base.UI.StatusText = "Finger left " + fingerHoverObject.name;
				fingerHoverObject.GetComponent<Renderer>().sharedMaterial = originalHoverMaterial;
			}
		}
	}

	private void OnFingerStationary(FingerMotionEvent e)
    {
        if (e.Phase == FingerMotionPhase.Started)
        {
            if (stationaryFingerIndex == -1)
            {
                GameObject selection = e.Selection;
                if (selection == fingerStationaryObject)
                {
                    UI.StatusText = "Begin stationary on finger " + e.Finger.Index;
                    stationaryFingerIndex = e.Finger.Index;
                    originalStationaryMaterial = selection.GetComponent<Renderer>().sharedMaterial;
                    selection.GetComponent<Renderer>().sharedMaterial = highlightMaterial;
                }
            }
        }
        else if (e.Phase == FingerMotionPhase.Updated)
        {
            if (!(e.ElapsedTime < chargeDelay) && e.Selection == fingerStationaryObject)
            {
                float chargeRatio = Mathf.Clamp01((e.ElapsedTime - chargeDelay) / chargeTime);
                float emissionRate = Mathf.Lerp(minSationaryParticleEmissionCount, maxSationaryParticleEmissionCount, chargeRatio);
                
                var emission = stationaryParticleSystem.emission;
                emission.rateOverTime = emissionRate;
                
                if (!stationaryParticleSystem.isPlaying)
                    stationaryParticleSystem.Play();
                
                UI.StatusText = "Charge: " + (100f * chargeRatio).ToString("N1") + "%";
            }
        }
        else if (e.Phase == FingerMotionPhase.Ended && e.Finger.Index == stationaryFingerIndex)
        {
            float elapsedTime = e.ElapsedTime;
            UI.StatusText = string.Concat("Stationary ended on finger ", e.Finger, " - ", elapsedTime.ToString("N1"), " seconds elapsed");
            StopStationaryParticleSystem();
            fingerStationaryObject.GetComponent<Renderer>().sharedMaterial = originalStationaryMaterial;
            stationaryFingerIndex = -1;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (fingerStationaryObject)
        {
            stationaryParticleSystem = fingerStationaryObject.GetComponentInChildren<ParticleSystem>();
        }
    }

    private void StopStationaryParticleSystem()
    {
        if (stationaryParticleSystem)
            stationaryParticleSystem.Stop();
        UI.StatusText = string.Empty;
    }

    private void SpawnParticles(GameObject obj)
    {
        ParticleSystem particleSystem = obj.GetComponentInChildren<ParticleSystem>();
        if (particleSystem)
        {
            particleSystem.Emit(1);
        }
    }
}
