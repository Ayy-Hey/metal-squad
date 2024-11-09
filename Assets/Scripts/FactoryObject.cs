using System;
using System.Collections.Generic;
using UnityEngine;

public class FactoryObject : MonoBehaviour
{
	public void CreateObj(int amount)
	{
		for (int i = this.listObjs.Count; i < amount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.objBase, new Vector3(0f, 0f, 0f), Quaternion.identity);
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			this.listObjs.Add(gameObject);
		}
		for (int j = 0; j < this.listObjs.Count; j++)
		{
			this.listObjs[j].SetActive(j < amount);
		}
	}

	public void DestroyAll()
	{
		for (int i = 0; i < this.listObjs.Count; i++)
		{
			UnityEngine.Object.Destroy(this.listObjs[i]);
		}
		this.listObjs = new List<GameObject>();
	}

	public GameObject objBase;

	public List<GameObject> listObjs;
}
