using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MyDataLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace CreateLevel
{
	[RequireComponent(typeof(CreateLevelLevel))]
	[ExecuteInEditMode]
	public class CreateLevelJson : MonoBehaviour
	{
		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				return;
			}
			base.name = this.levelFileName;
			if (this.writeLevel)
			{
				this.writeLevel = false;
				this.level.GetPointPos();
				string contents = JsonConvert.SerializeObject(this.level.level);
				File.WriteAllText(Application.dataPath + "/Resources/Map/Decrypt/" + this.levelFileName + ".txt", contents);
			}
			if (this.readLevel)
			{
				this.readLevel = false;
				base.StartCoroutine(this.ReadLevelData());
			}
		}

		private void Reset()
		{
			this.level = base.GetComponent<CreateLevelLevel>();
			this.levelFileName = base.gameObject.scene.name.Replace(" ", string.Empty);
			if (this.level.points == null || this.level.points.Count == 0)
			{
				base.StartCoroutine(this.CreateNewPoint());
			}
		}

		private IEnumerator CreateNewPoint()
		{
			CreateLevelPoint point = base.GetComponentInChildren<CreateLevelPoint>();
			if (!point)
			{
				point = new GameObject("Point_0").AddComponent<CreateLevelPoint>();
				point.transform.parent = base.transform;
				CreateLevelEnemyDataInfo createLevelEnemyDataInfo = new GameObject().AddComponent<CreateLevelEnemyDataInfo>();
				createLevelEnemyDataInfo.transform.parent = point.transform;
				point.enemydata.enemyDataInfo = new List<CreateLevelEnemyDataInfo>();
				point.enemydata.enemyDataInfo.Add(createLevelEnemyDataInfo);
			}
			this.level.points = new List<CreateLevelPoint>();
			this.level.points.Add(point);
			yield return 0;
			yield break;
		}

		private IEnumerator ReadLevelData()
		{
			TextAsset data = Resources.Load<TextAsset>("Map/Decrypt/" + this.levelFileName);
			LevelData level = JsonConvert.DeserializeObject<LevelData>(data.text);
			yield return new WaitUntil(() => level != null);
			base.transform.DestroyChildren();
			this.level.points = new List<CreateLevelPoint>();
			for (int i = 0; i < level.points.Count; i++)
			{
				CreateLevelPoint createLevelPoint = new GameObject("Point_" + i).AddComponent<CreateLevelPoint>();
				createLevelPoint.SetPoint(level.points[i].checkpoint_pos_x, level.points[i].checkpoint_pos_y, level.points[i].isUnlocked);
				createLevelPoint.transform.parent = base.transform;
				createLevelPoint.enemydata.enemyDataInfo = new List<CreateLevelEnemyDataInfo>();
				for (int j = 0; j < level.points[i].enemyData.enemyDataInfo.Count; j++)
				{
					CreateLevelEnemyDataInfo createLevelEnemyDataInfo = new GameObject().AddComponent<CreateLevelEnemyDataInfo>();
					createLevelEnemyDataInfo.SetEnemyDataInfo(level.points[i].enemyData.enemyDataInfo[j].type, level.points[i].enemyData.enemyDataInfo[j].ismove, level.points[i].enemyData.enemyDataInfo[j].level, level.points[i].enemyData.enemyDataInfo[j].pos_x, level.points[i].enemyData.enemyDataInfo[j].pos_y);
					createLevelEnemyDataInfo.transform.parent = createLevelPoint.transform;
					createLevelPoint.enemydata.enemyDataInfo.Add(createLevelEnemyDataInfo);
				}
				this.level.points.Add(createLevelPoint);
			}
			yield break;
		}

		public CreateLevelLevel level;

		public string levelFileName;

		public bool writeLevel;

		public bool readLevel;

		private Vector2 pos;
	}
}
