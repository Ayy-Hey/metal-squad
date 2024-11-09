using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifiedShadow : Shadow
{
	public override void ModifyMesh(VertexHelper vh)
	{
		if (!this.IsActive())
		{
			return;
		}
		List<UIVertex> list = ListPool<UIVertex>.Get();
		vh.GetUIVertexStream(list);
		this.ModifyVertices(list);
		vh.Clear();
		vh.AddUIVertexTriangleStream(list);
		ListPool<UIVertex>.Release(list);
	}

	public virtual void ModifyVertices(List<UIVertex> verts)
	{
	}
}
