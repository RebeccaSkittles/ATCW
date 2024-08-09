using System;
using UnityEngine;

public class NcParticleSpiral : NcEffectBehaviour
{
	public struct SpiralSettings
	{
		public int numArms;

		public int numPPA;

		public float partSep;

		public float turnDist;

		public float vertDist;

		public float originOffset;

		public float turnSpeed;

		public float fade;

		public float size;
	}

	protected const int Min_numArms = 1;

	protected const int Max_numArms = 10;

	protected const int Min_numPPA = 20;

	protected const int Max_numPPA = 60;

	protected const float Min_partSep = -0.3f;

	protected const float Max_partSep = 0.3f;

	protected const float Min_turnDist = -1.5f;

	protected const float Max_turnDist = 1.5f;

	protected const float Min_vertDist = 0f;

	protected const float Max_vertDist = 0.5f;

	protected const float Min_originOffset = -3f;

	protected const float Max_originOffset = 3f;

	protected const float Min_turnSpeed = -180f;

	protected const float Max_turnSpeed = 180f;

	protected const float Min_fade = -1f;

	protected const float Max_fade = 1f;

	protected const float Min_size = -2f;

	protected const float Max_size = 2f;

	public float m_fDelayTime;

	protected float m_fStartTime;

	public GameObject m_ParticlePrefab;

	public int m_nNumberOfArms = 2;

	public int m_nParticlesPerArm = 100;

	public float m_fParticleSeparation = 0.05f;

	public float m_fTurnDistance = 0.5f;

	public float m_fVerticalTurnDistance;

	public float m_fOriginOffset;

	public float m_fTurnSpeed;

	public float m_fFadeValue;

	public float m_fSizeValue;

	public int m_nNumberOfSpawns = 9999999;

	public float m_fSpawnRate = 5f;

	private float timeOfLastSpawn = -1000f;

	private int spawnCount;

	private int totParticles;

	private SpiralSettings defaultSettings;

	public override int GetAnimationState()
	{
		if (base.enabled && NcEffectBehaviour.IsActive(base.gameObject))
		{
			if (NcEffectBehaviour.GetEngineTime() < m_fStartTime + m_fDelayTime + 0.1f)
			{
				return 1;
			}
			return -1;
		}
		return -1;
	}

	public void RandomizeEditor()
	{
		m_nNumberOfArms = UnityEngine.Random.Range(1, 10);
		m_nParticlesPerArm = UnityEngine.Random.Range(20, 60);
		m_fParticleSeparation = UnityEngine.Random.Range(-0.3f, 0.3f);
		m_fTurnDistance = UnityEngine.Random.Range(-1.5f, 1.5f);
		m_fVerticalTurnDistance = UnityEngine.Random.Range(0f, 0.5f);
		m_fOriginOffset = UnityEngine.Random.Range(-3f, 3f);
		m_fTurnSpeed = UnityEngine.Random.Range(-180f, 180f);
		m_fFadeValue = UnityEngine.Random.Range(-1f, 1f);
		m_fSizeValue = UnityEngine.Random.Range(-2f, 2f);
	}

    private void Start()
    {
        m_fStartTime = NcEffectBehaviour.GetEngineTime();
        if (m_ParticlePrefab == null)
        {
            ParticleSystem component = GetComponent<ParticleSystem>();
            if (component == null)
            {
                return;
            }
            component.Stop();
        }
        defaultSettings = getSettings();
    }

private void SpawnEffect()
    {
        GameObject gameObject;
        if (m_ParticlePrefab != null)
        {
            gameObject = CreateGameObject(m_ParticlePrefab);
            if (gameObject == null)
            {
                return;
            }
            ChangeParent(base.transform, gameObject.transform, true, null);
        }
        else
        {
            gameObject = base.gameObject;
        }
        ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
        if (particleSystem == null)
        {
            return;
        }
        particleSystem.Stop();
        ParticleSystem.MainModule main = particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;

        int totalParticles = m_nNumberOfArms * m_nParticlesPerArm;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[totalParticles];

        float angleStep = (float)Math.PI * 2f / (float)m_nNumberOfArms;
        for (int i = 0; i < m_nNumberOfArms; i++)
        {
            float radius = 0f;
            float angle = 0f;
            float armAngle = (float)i * angleStep;
            for (int j = 0; j < m_nParticlesPerArm; j++)
            {
                int index = i * m_nParticlesPerArm + j;
                radius = m_fOriginOffset + m_fTurnDistance * angle;
                Vector3 position = gameObject.transform.localPosition;
                position.x += radius * Mathf.Cos(angle);
                position.z += radius * Mathf.Sin(angle);
                float x = position.x * Mathf.Cos(armAngle) + position.z * Mathf.Sin(armAngle);
                float z = (0f - position.x) * Mathf.Sin(armAngle) + position.z * Mathf.Cos(armAngle);
                position.x = x;
                position.z = z;
                position.y += (float)j * m_fVerticalTurnDistance;

                particles[index].position = position;
                particles[index].startLifetime = particleSystem.main.startLifetime.constant;
                particles[index].startSize = particleSystem.main.startSize.constant;
                particles[index].startColor = particleSystem.main.startColor.color;

                angle += m_fParticleSeparation;
                if (m_fFadeValue != 0f)
                {
                    float lifetimeScale = 1f - Mathf.Abs(m_fFadeValue) + Mathf.Abs(m_fFadeValue) * (float)((m_fFadeValue < 0f) ? (m_nParticlesPerArm - j) : (j + 1)) / (float)m_nParticlesPerArm;
                    particles[index].remainingLifetime = particles[index].startLifetime * lifetimeScale;
                }
                if (m_fSizeValue != 0f)
                {
                    float sizeScale = 1f + Mathf.Abs(m_fSizeValue) * (float)((m_fSizeValue < 0f) ? (m_nParticlesPerArm - j) : (j + 1)) / (float)m_nParticlesPerArm;
                    particles[index].startSize *= sizeScale;
                }
            }
        }
        particleSystem.SetParticles(particles, totalParticles);
    }

	private void Update()
	{
		if (!(NcEffectBehaviour.GetEngineTime() < m_fStartTime + m_fDelayTime) && m_fTurnSpeed != 0f)
		{
			base.transform.Rotate(base.transform.up * NcEffectBehaviour.GetEngineDeltaTime() * m_fTurnSpeed, Space.World);
		}
	}

	private void LateUpdate()
	{
		if (!(NcEffectBehaviour.GetEngineTime() < m_fStartTime + m_fDelayTime))
		{
			float num = NcEffectBehaviour.GetEngineTime() - timeOfLastSpawn;
			if (m_fSpawnRate <= num && spawnCount < m_nNumberOfSpawns)
			{
				SpawnEffect();
				timeOfLastSpawn = NcEffectBehaviour.GetEngineTime();
				spawnCount++;
			}
		}
	}

	public SpiralSettings getSettings()
	{
		SpiralSettings result = default(SpiralSettings);
		result.numArms = m_nNumberOfArms;
		result.numPPA = m_nParticlesPerArm;
		result.partSep = m_fParticleSeparation;
		result.turnDist = m_fTurnDistance;
		result.vertDist = m_fVerticalTurnDistance;
		result.originOffset = m_fOriginOffset;
		result.turnSpeed = m_fTurnSpeed;
		result.fade = m_fFadeValue;
		result.size = m_fSizeValue;
		return result;
	}

	public SpiralSettings resetEffect(bool killCurrent, SpiralSettings settings)
	{
		if (killCurrent)
		{
			killCurrentEffects();
		}
		m_nNumberOfArms = settings.numArms;
		m_nParticlesPerArm = settings.numPPA;
		m_fParticleSeparation = settings.partSep;
		m_fTurnDistance = settings.turnDist;
		m_fVerticalTurnDistance = settings.vertDist;
		m_fOriginOffset = settings.originOffset;
		m_fTurnSpeed = settings.turnSpeed;
		m_fFadeValue = settings.fade;
		m_fSizeValue = settings.size;
		SpawnEffect();
		timeOfLastSpawn = NcEffectBehaviour.GetEngineTime();
		spawnCount++;
		return getSettings();
	}

	public SpiralSettings resetEffectToDefaults(bool killCurrent)
	{
		return resetEffect(killCurrent, defaultSettings);
	}

	public SpiralSettings randomizeEffect(bool killCurrent)
	{
		if (killCurrent)
		{
			killCurrentEffects();
		}
		RandomizeEditor();
		SpawnEffect();
		timeOfLastSpawn = NcEffectBehaviour.GetEngineTime();
		spawnCount++;
		return getSettings();
	}

    private void killCurrentEffects()
    {
        ParticleSystem[] particleSystems = base.transform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            ParticleSystem.MainModule main = particleSystem.main;
            main.loop = false;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            particleSystem.Stop();
            particleSystem.Clear();
        }
    }

	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		m_fDelayTime /= fSpeedRate;
		m_fTurnSpeed *= fSpeedRate;
	}
}
