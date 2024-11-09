using System;
using System.Collections;
using UnityEngine;

public class HurtFlashEffect : MonoBehaviour
{
	public void Flash()
	{
		if (this.mpb == null)
		{
			this.mpb = new MaterialPropertyBlock();
		}
		if (this.meshRenderer == null)
		{
			this.meshRenderer = base.GetComponent<MeshRenderer>();
		}
		this.meshRenderer.GetPropertyBlock(this.mpb);
		base.StartCoroutine(this.FlashRoutine());
	}

	private IEnumerator FlashRoutine()
	{
		if (this.flashCount < 0)
		{
			this.flashCount = 3;
		}
		int fillPhase = Shader.PropertyToID(this.fillPhaseProperty);
		int fillColor = Shader.PropertyToID(this.fillColorProperty);
		WaitForSeconds wait = new WaitForSeconds(this.interval);
		for (int i = 0; i < this.flashCount; i++)
		{
			this.mpb.SetColor(fillColor, this.flashColor);
			this.mpb.SetFloat(fillPhase, 1f);
			this.meshRenderer.SetPropertyBlock(this.mpb);
			yield return wait;
			this.mpb.SetFloat(fillPhase, 0f);
			this.meshRenderer.SetPropertyBlock(this.mpb);
			yield return wait;
		}
		yield return null;
		yield break;
	}

	private const int DefaultFlashCount = 3;

	public int flashCount = 3;

	public Color flashColor = Color.white;

	[Range(0.008333334f, 0.06666667f)]
	public float interval = 0.0166666675f;

	public string fillPhaseProperty = "_FillPhase";

	public string fillColorProperty = "_FillColor";

	private MaterialPropertyBlock mpb;

	private MeshRenderer meshRenderer;
}
