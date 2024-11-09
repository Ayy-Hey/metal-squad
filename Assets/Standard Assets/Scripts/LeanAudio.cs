using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LeanAudio : MonoBehaviour
{
	public static LeanAudioOptions options()
	{
		if (LeanAudio.generatedWaveDistances == null)
		{
			LeanAudio.generatedWaveDistances = new float[LeanAudio.PROCESSING_ITERATIONS_MAX];
			LeanAudio.longList = new float[LeanAudio.PROCESSING_ITERATIONS_MAX];
		}
		return new LeanAudioOptions();
	}

	public static LeanAudioStream createAudioStream(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options = null)
	{
		if (options == null)
		{
			options = new LeanAudioOptions();
		}
		options.useSetData = false;
		int waveLength = LeanAudio.createAudioWave(volume, frequency, options);
		LeanAudio.createAudioFromWave(waveLength, options);
		return options.stream;
	}

	public static AudioClip createAudio(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options = null)
	{
		if (options == null)
		{
			options = new LeanAudioOptions();
		}
		int waveLength = LeanAudio.createAudioWave(volume, frequency, options);
		return LeanAudio.createAudioFromWave(waveLength, options);
	}

	private static int createAudioWave(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options)
	{
		float time = volume[volume.length - 1].time;
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < LeanAudio.PROCESSING_ITERATIONS_MAX; i++)
		{
			float num3 = frequency.Evaluate(num2);
			if (num3 < LeanAudio.MIN_FREQEUNCY_PERIOD)
			{
				num3 = LeanAudio.MIN_FREQEUNCY_PERIOD;
			}
			float num4 = volume.Evaluate(num2 + 0.5f * num3);
			if (options.vibrato != null)
			{
				for (int j = 0; j < options.vibrato.Length; j++)
				{
					float num5 = Mathf.Abs(Mathf.Sin(1.5708f + num2 * (1f / options.vibrato[j][0]) * 3.14159274f));
					float num6 = 1f - options.vibrato[j][1];
					num5 = options.vibrato[j][1] + num6 * num5;
					num4 *= num5;
				}
			}
			if (num2 + 0.5f * num3 >= time)
			{
				break;
			}
			if (num >= LeanAudio.PROCESSING_ITERATIONS_MAX - 1)
			{
				UnityEngine.Debug.LogError("LeanAudio has reached it's processing cap. To avoid this error increase the number of iterations ex: LeanAudio.PROCESSING_ITERATIONS_MAX = " + LeanAudio.PROCESSING_ITERATIONS_MAX * 2);
				break;
			}
			LeanAudio.generatedWaveDistances[num / 2] = num3;
			num2 += num3;
			LeanAudio.longList[num] = num2;
			LeanAudio.longList[num + 1] = ((i % 2 != 0) ? num4 : (-num4));
			num += 2;
		}
		num += -2;
		LeanAudio.generatedWaveDistancesCount = num / 2;
		return num;
	}

	private static AudioClip createAudioFromWave(int waveLength, LeanAudioOptions options)
	{
		float num = LeanAudio.longList[waveLength - 2];
		float[] array = new float[(int)((float)options.frequencyRate * num)];
		int num2 = 0;
		float num3 = LeanAudio.longList[num2];
		float num4 = 0f;
		float num5 = LeanAudio.longList[num2];
		float num6 = LeanAudio.longList[num2 + 1];
		for (int i = 0; i < array.Length; i++)
		{
			float num7 = (float)i / (float)options.frequencyRate;
			if (num7 > LeanAudio.longList[num2])
			{
				num4 = LeanAudio.longList[num2];
				num2 += 2;
				num3 = LeanAudio.longList[num2] - LeanAudio.longList[num2 - 2];
				num6 = LeanAudio.longList[num2 + 1];
			}
			num5 = num7 - num4;
			float num8 = num5 / num3;
			float num9 = Mathf.Sin(num8 * 3.14159274f);
			num9 *= num6;
			array[i] = num9;
		}
		int num10 = array.Length;
		AudioClip audioClip;
		if (options.useSetData)
		{
			string name = "Generated Audio";
			int lengthSamples = num10;
			int channels = 1;
			int frequencyRate = options.frequencyRate;
			bool stream = false;
			AudioClip.PCMReaderCallback pcmreadercallback = null;
			if (LeanAudio._003C_003Ef__mg_0024cache0 == null)
			{
				LeanAudio._003C_003Ef__mg_0024cache0 = new AudioClip.PCMSetPositionCallback(LeanAudio.OnAudioSetPosition);
			}
			audioClip = AudioClip.Create(name, lengthSamples, channels, frequencyRate, stream, pcmreadercallback, LeanAudio._003C_003Ef__mg_0024cache0);
			audioClip.SetData(array, 0);
		}
		else
		{
			options.stream = new LeanAudioStream(array);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"len:",
				array.Length,
				" lengthSamples:",
				num10,
				" freqRate:",
				options.frequencyRate
			}));
			audioClip = AudioClip.Create("Generated Audio", num10, 1, options.frequencyRate, false, new AudioClip.PCMReaderCallback(options.stream.OnAudioRead), new AudioClip.PCMSetPositionCallback(options.stream.OnAudioSetPosition));
			options.stream.audioClip = audioClip;
		}
		return audioClip;
	}

	private static void OnAudioSetPosition(int newPosition)
	{
	}

	public static AudioClip generateAudioFromCurve(AnimationCurve curve, int frequencyRate = 44100)
	{
		float time = curve[curve.length - 1].time;
		float num = time;
		float[] array = new float[(int)((float)frequencyRate * num)];
		for (int i = 0; i < array.Length; i++)
		{
			float time2 = (float)i / (float)frequencyRate;
			array[i] = curve.Evaluate(time2);
		}
		int lengthSamples = array.Length;
		AudioClip audioClip = AudioClip.Create("Generated Audio", lengthSamples, 1, frequencyRate, false);
		audioClip.SetData(array, 0);
		return audioClip;
	}

	public static AudioSource play(AudioClip audio, Vector3 pos = default(Vector3), float volume = 1f, float pitch = 1f)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.position = pos;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		audioSource.clip = audio;
		audioSource.Play();
		UnityEngine.Object.Destroy(gameObject, audio.length);
		return audioSource;
	}

	public static void printOutAudioClip(AudioClip audioClip, ref AnimationCurve curve, float scaleX = 1f)
	{
		float[] array = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData(array, 0);
		int i = 0;
		Keyframe[] array2 = new Keyframe[array.Length];
		while (i < array.Length)
		{
			array2[i] = new Keyframe((float)i * scaleX, array[i]);
			i++;
		}
		curve = new AnimationCurve(array2);
	}

	public static float MIN_FREQEUNCY_PERIOD = 0.000115f;

	public static int PROCESSING_ITERATIONS_MAX = 50000;

	public static float[] generatedWaveDistances;

	public static int generatedWaveDistancesCount;

	private static float[] longList;

	[CompilerGenerated]
	private static AudioClip.PCMSetPositionCallback _003C_003Ef__mg_0024cache0;
}
