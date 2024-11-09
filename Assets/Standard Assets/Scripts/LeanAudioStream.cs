using System;
using UnityEngine;

public class LeanAudioStream
{
	public LeanAudioStream(float[] audioArr)
	{
		this.audioArr = audioArr;
	}

	public void OnAudioRead(float[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = this.audioArr[this.position];
			this.position++;
		}
	}

	public void OnAudioSetPosition(int newPosition)
	{
		this.position = newPosition;
	}

	public int position;

	public AudioClip audioClip;

	public float[] audioArr;
}
