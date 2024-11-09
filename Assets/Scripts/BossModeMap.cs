using System;
using UnityEngine;

public class BossModeMap : MonoBehaviour
{
	public EBoss boss;

	public GameObject objBoss;

	public GameObject objMap;

	public Transform tfCreatePointPlayer;

	public ParallaxLayer parallaxLayer;

	public float cameraStartSize = 3.6f;

	public Vector4 startBoundary;

	public AudioSource soundBG;

	public PlayerManagerStory.TypeBegin playerTypeBegin;
}
