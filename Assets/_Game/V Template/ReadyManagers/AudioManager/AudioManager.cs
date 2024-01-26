using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	private static Dictionary<string, Sound> listSound;

	public AudioMixerGroup mixerGroup;
	public Sound[] sounds;

	public bool soundEnabled = true;

	private void Start()
	{
		Play("Background");
	}
	void Awake()
	{
		if (instance != null)
		{
			LoadSound();
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			listSound = new Dictionary<string, Sound>(sounds.Length);
			DontDestroyOnLoad(gameObject);
			if (enabled)
				LoadSound();
		}

		void LoadSound()
		{
			foreach (Sound s in sounds)
			{
				if (!listSound.ContainsKey(s.name))
				{
					s.source = instance.gameObject.AddComponent<AudioSource>();
					s.source.clip = s.clip;
					s.source.loop = s.loop;
					s.source.volume = s.volume;
					s.source.pitch = s.pitch;

					s.source.outputAudioMixerGroup = mixerGroup;

					listSound.Add(s.name, s);
				}
			}
		}
	}

	public void Play(string sound)
	{
		if (!soundEnabled)
			return;
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public static void Play(Sound s, float pitch = 1)
	{
		if (!instance.soundEnabled)
			return;

		if (s == null)
		{
			Debug.LogWarning("Sound: null!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.pitch = pitch;
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		//s.source.Play();
		s.source.PlayOneShot(s.clip);

	}

	public static void Play(string value, float pitch = 1)
	{
		if (listSound.ContainsKey(value))
		{
			Play(listSound[value], pitch);
		}
		else
		{
			Debug.LogWarning("Sound: " + value + " not found!");
		}
	}

}
