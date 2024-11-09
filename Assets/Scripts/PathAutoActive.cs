using System;
using SWS;
using UnityEngine;

public class PathAutoActive : MonoBehaviour
{
	private void Start()
	{
		this.activatorTransform = GameManager.Instance.player.transform;
		this.targetSplineMove = UnityEngine.Object.FindObjectOfType<splineMove>();
		this.pathManager = base.GetComponent<PathManager>();
		this.waypoints = this.pathManager.GetPathPoints(false);
		this.pathStartX = this.waypoints[0].x;
		this.pathEndX = this.waypoints[this.waypoints.Length - 1].x;
		this.activeCnt = 0;
		this.IsInit = true;
	}

	private void Update()
	{
		if (!this.IsInit)
		{
			return;
		}
		if (this.CheckSetNewPath())
		{
			this.SetNewPathForNPC();
			this.activeCnt++;
		}
	}

	private bool CheckSetNewPath()
	{
		bool flag = this.CheckActivatorIsInPath();
		return flag && this.targetSplineMove.pathContainer != this.pathManager && (!(this.targetSplineMove.pathContainer != null) || this.targetSplineMove.loopType != splineMove.LoopType.none || this.targetSplineMove.IsFinishMove) && (this.pathManager.loopType != splineMove.LoopType.none || this.activeCnt == 0);
	}

	private bool CheckActivatorIsInPath()
	{
		return this.activatorTransform.position.x >= this.pathStartX && this.activatorTransform.position.x <= this.pathEndX;
	}

	private void SetNewPathForNPC()
	{
		this.targetSplineMove.SetPath(this.pathManager);
	}

	private Transform activatorTransform;

	private splineMove targetSplineMove;

	private PathManager pathManager;

	private Vector3[] waypoints;

	private float pathStartX;

	private float pathEndX;

	private int activeCnt;

	private bool IsInit;
}
