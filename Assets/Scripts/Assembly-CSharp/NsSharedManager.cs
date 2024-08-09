using System.Collections.Generic;
using UnityEngine;

public class NsSharedManager : MonoBehaviour
{
	protected static NsSharedManager _inst;

	protected List<GameObject> m_SharedPrefabs = new List<GameObject>();

	protected List<GameObject> m_SharedGameObjects = new List<GameObject>();

	protected List<AudioClip> m_SharedAudioClip = new List<AudioClip>();

	protected List<List<AudioSource>> m_SharedAudioSources = new List<List<AudioSource>>();

	public static NsSharedManager inst
	{
		get
		{
			if (_inst == null)
			{
				_inst = NcEffectBehaviour.GetRootInstanceEffect().AddComponent<NsSharedManager>();
			}
			return _inst;
		}
	}

	public GameObject GetSharedParticleGameObject(GameObject originalParticlePrefab)
	{
    	int num = m_SharedPrefabs.IndexOf(originalParticlePrefab);
    	if (num < 0 || m_SharedGameObjects[num] == null)
    	{
        	if (!NcEffectBehaviour.IsSafe())
        	{
            	return null;
        	}
        	GameObject gameObject = Object.Instantiate(originalParticlePrefab);
        	gameObject.transform.parent = NcEffectBehaviour.GetRootInstanceEffect().transform;
        	if (0 <= num)
        	{
            	m_SharedGameObjects[num] = gameObject;
        	}
        	else
        	{
            	m_SharedPrefabs.Add(originalParticlePrefab);
            	m_SharedGameObjects.Add(gameObject);
        	}
        	NcParticleSystem component = gameObject.GetComponent<NcParticleSystem>();
        	if (component != null)
        	{
            	component.enabled = false;
        	}
        	ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
        	if (particleSystem != null)
        	{
            	var emission = particleSystem.emission;
            	emission.enabled = false;
            	particleSystem.useAutoRandomSeed = false;
            	particleSystem.simulationSpace = ParticleSystemSimulationSpace.World;
        	}
        	NcParticleSystem component3 = gameObject.GetComponent<NcParticleSystem>();
        	if (component3 != null)
        	{
            component3.m_bBurst = false;
        	}
        	return gameObject;
    	}
    	return m_SharedGameObjects[num];
	}


public void EmitSharedParticleSystem(GameObject originalParticlePrefab, int nEmitCount, Vector3 worldPos)
{
    GameObject sharedParticleGameObject = GetSharedParticleGameObject(originalParticlePrefab);
    if (sharedParticleGameObject == null)
    {
        return;
    }
    sharedParticleGameObject.transform.position = worldPos;
    ParticleSystem particleSystem = sharedParticleGameObject.GetComponent<ParticleSystem>();
    if (particleSystem != null)
    {
        particleSystem.Emit(nEmitCount);
    }
}


	public AudioSource GetSharedAudioSource(AudioClip audioClip, int nPriority, bool bLoop, float fVolume, float fPitch)
	{
		int num = m_SharedAudioClip.IndexOf(audioClip);
		if (num < 0)
		{
			if (!NcEffectBehaviour.IsSafe())
			{
				return null;
			}
			List<AudioSource> list = new List<AudioSource>();
			m_SharedAudioClip.Add(audioClip);
			m_SharedAudioSources.Add(list);
			return AddAudioSource(list, audioClip, nPriority, bLoop, fVolume, fPitch);
		}
		foreach (AudioSource item in m_SharedAudioSources[num])
		{
			if (item.volume == fVolume && item.pitch == fPitch && item.loop == bLoop && item.priority == nPriority)
			{
				return item;
			}
		}
		return AddAudioSource(m_SharedAudioSources[num], audioClip, nPriority, bLoop, fVolume, fPitch);
	}

	private AudioSource AddAudioSource(List<AudioSource> sourceList, AudioClip audioClip, int nPriority, bool bLoop, float fVolume, float fPitch)
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		sourceList.Add(audioSource);
		audioSource.clip = audioClip;
		audioSource.priority = nPriority;
		audioSource.loop = bLoop;
		audioSource.volume = fVolume;
		audioSource.pitch = fPitch;
		audioSource.playOnAwake = false;
		return audioSource;
	}

	public void PlaySharedAudioSource(bool bUniquePlay, AudioClip audioClip, int nPriority, bool bLoop, float fVolume, float fPitch)
	{
		AudioSource sharedAudioSource = GetSharedAudioSource(audioClip, nPriority, bLoop, fVolume, fPitch);
		if (!(sharedAudioSource == null) && (!bUniquePlay || !sharedAudioSource.isPlaying))
		{
			sharedAudioSource.Play();
		}
	}
}
