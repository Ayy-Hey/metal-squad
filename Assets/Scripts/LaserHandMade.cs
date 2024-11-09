using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserHandMade : MonoBehaviour
{
	public Vector2 Tilling
	{
		get
		{
			return this.lineRenderer.material.mainTextureScale;
		}
		set
		{
			this.lineRenderer.material.mainTextureScale = value;
		}
	}

	private void OnValidate()
	{
		if (!this.lineRenderer)
		{
			this.lineRenderer = base.GetComponent<LineRenderer>();
		}
		this.SetColor(this.mainColor);
	}

	private void OnEnable()
	{
		this.OnValidate();
	}

	public void SetMainMaterials(int id)
	{
		try
		{
			this.lineRenderer.material = this.materials[id];
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	public void SetAllMaterials()
	{
		this.lineRenderer.materials = new Material[this.materials.Length];
		for (int i = 0; i < this.materials.Length; i++)
		{
			this.lineRenderer.materials[i] = this.materials[i];
		}
	}

	public void SetTexture(int textureId)
	{
		this.lineRenderer.material.mainTexture = this.textures[textureId];
	}

	public void SetMainTextureTilling(Vector2 tilling)
	{
		this.lineRenderer.material.mainTextureScale = tilling;
	}

	public void SetColor(Color color)
	{
		this.lineRenderer.startColor = color;
		this.lineRenderer.endColor = color;
	}

	public float GetSize()
	{
		return this.lineRenderer.widthMultiplier;
	}

	public void SetSize(float size)
	{
		this.lineRenderer.widthMultiplier = size;
	}

	public void ActiveEffStart(bool enable)
	{
		this.startTransEff.gameObject.SetActive(enable);
	}

	public void ActiveEffEnd(bool enable)
	{
		this.endTransEff.gameObject.SetActive(enable);
	}

	public void ActiveEff()
	{
		this.startTransEff.gameObject.SetActive(true);
		this.endTransEff.gameObject.SetActive(true);
		this.useFx = true;
	}

	public void DisableEff()
	{
		this.startTransEff.gameObject.SetActive(false);
		this.endTransEff.gameObject.SetActive(false);
		this.useFx = false;
	}

	public void OnShow(float deltaTime, Vector3 startPos, Vector3 endPos)
	{
		if (!this._isShow || !this.lineRenderer.enabled)
		{
			this._isShow = true;
			this.lineRenderer.enabled = true;
			this.lineRenderer.positionCount = 2;
		}
		if (this.useFx)
		{
			this.startTransEff.position = startPos;
			this.endTransEff.position = endPos;
		}
		this.lineRenderer.SetPosition(0, startPos);
		this.lineRenderer.SetPosition(1, endPos);
		this.offset = this.lineRenderer.materials[0].mainTextureOffset;
		this.offset.x = this.offset.x - this.speedChangeOffset * deltaTime;
		for (int i = 0; i < this.lineRenderer.materials.Length; i++)
		{
			this.lineRenderer.materials[i].mainTextureOffset = this.offset;
		}
	}

	public void OnShow(float deltaTime, Vector3[] listPos)
	{
		if (!this._isShow || !this.lineRenderer.enabled)
		{
			this._isShow = true;
			this.lineRenderer.enabled = true;
			this.lineRenderer.positionCount = listPos.Length;
		}
		if (this.useFx)
		{
			this.startTransEff.position = listPos[0];
			this.endTransEff.position = listPos[listPos.Length - 1];
		}
		this.lineRenderer.SetPositions(listPos);
		this.offset = this.lineRenderer.materials[0].mainTextureOffset;
		this.offset.x = this.offset.x - this.speedChangeOffset * deltaTime;
		for (int i = 0; i < this.lineRenderer.materials.Length; i++)
		{
			this.lineRenderer.materials[i].mainTextureOffset = this.offset;
		}
	}

	public void Off()
	{
		this._isShow = false;
		this.lineRenderer.enabled = false;
	}

	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private Material[] materials;

	[SerializeField]
	private Texture2D[] textures;

	[SerializeField]
	private Color mainColor = Color.white;

	[SerializeField]
	public float speedChangeOffset = 1f;

	[SerializeField]
	[Range(0f, 100f, order = 2)]
	private int numCapVertices;

	[SerializeField]
	[Range(0f, 100f, order = 2)]
	private int numCornerVertices;

	[SerializeField]
	private Transform startTransEff;

	[SerializeField]
	private Transform endTransEff;

	[SerializeField]
	private bool test;

	public bool useFx;

	private bool _isShow;

	private Vector2 offset;
}
