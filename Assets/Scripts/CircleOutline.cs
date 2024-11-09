using System;
using System.Collections.Generic;
using UnityEngine;

public class CircleOutline : ModifiedShadow
{
	public int circleCount
	{
		get
		{
			return this.m_circleCount;
		}
		set
		{
			this.m_circleCount = Mathf.Max(value, 1);
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}
	}

	public int firstSample
	{
		get
		{
			return this.m_firstSample;
		}
		set
		{
			this.m_firstSample = Mathf.Max(value, 2);
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}
	}

	public int sampleIncrement
	{
		get
		{
			return this.m_sampleIncrement;
		}
		set
		{
			this.m_sampleIncrement = Mathf.Max(value, 1);
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}
	}

	public override void ModifyVertices(List<UIVertex> verts)
	{
		if (!this.IsActive())
		{
			return;
		}
		int num = (this.m_firstSample * 2 + this.m_sampleIncrement * (this.m_circleCount - 1)) * this.m_circleCount / 2;
		int num2 = verts.Count * (num + 1);
		if (verts.Capacity < num2)
		{
			verts.Capacity = num2;
		}
		int count = verts.Count;
		int num3 = 0;
		int num4 = this.m_firstSample;
		float num5 = base.effectDistance.x / (float)this.circleCount;
		float num6 = base.effectDistance.y / (float)this.circleCount;
		for (int i = 1; i <= this.m_circleCount; i++)
		{
			float num7 = num5 * (float)i;
			float num8 = num6 * (float)i;
			float num9 = 6.28318548f / (float)num4;
			float num10 = (float)(i % 2) * num9 * 0.5f;
			for (int j = 0; j < num4; j++)
			{
				int num11 = num3 + count;
				base.ApplyShadow(verts, base.effectColor, num3, num11, num7 * Mathf.Cos(num10), num8 * Mathf.Sin(num10));
				num3 = num11;
				num10 += num9;
			}
			num4 += this.m_sampleIncrement;
		}
	}

	[SerializeField]
	private int m_circleCount = 2;

	[SerializeField]
	private int m_firstSample = 4;

	[SerializeField]
	private int m_sampleIncrement = 2;
}
