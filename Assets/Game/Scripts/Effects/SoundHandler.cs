using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour 
{
	
	private static SoundHandler instance;
	public static SoundHandler GetInstance ()
	{
		return instance;
	}

	[SerializeField] private AudioSource bgmSource;
	[SerializeField] private AudioSource enemySfx;
	[SerializeField] private AudioSource characterSfx;
	[SerializeField] private AudioSource uiSfx;

	[SerializeField] private AudioClip[] enemyClips;
	[SerializeField] private AudioClip[] characterClips;
	[SerializeField] private AudioClip[] uiClips;

	[SerializeField] private SoundData data;


	void Awake ()
	{
		instance = this;
	}

	void OnDestroy ()
	{
		instance = null;
	}

	#region Play Sounds

	public static void PlaySlash ()
	{
		if(instance != null)
			instance.uiSfx.PlayOneShot(instance.data.slash);
	}

	public static void PlayButton ()
	{
		if(instance != null)
			instance.uiSfx.PlayOneShot(instance.data.button);
	}

	public static void PlayGirlRelease()
	{
		if(instance != null)
			instance.uiSfx.PlayOneShot(instance.data.girlSaved);
	}

	#endregion

	private AudioClip GetSFXClip (string soundName)
	{
		for(int i=0; i < uiClips.Length; i++)
		{
			if(uiClips[i] == null)
				continue;
			
			if(soundName == uiClips[i].name)
				return uiClips[i];
		}
		return null;
	}


}


[System.Serializable]
public class SoundData
{
	public AudioClip slash;
	public AudioClip girlSaved;
	public AudioClip button;
}