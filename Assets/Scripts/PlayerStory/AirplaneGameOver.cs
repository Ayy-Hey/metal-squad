using System;
using UnityEngine;

namespace PlayerStory
{
	public class AirplaneGameOver : MonoBehaviour
	{
		public void OnBegin(Action OnEndedStep1, Action OnEnded)
		{
			this.Step = 0;
			this.isInit = true;
			this.isReadyToDestroy = false;
			base.gameObject.SetActive(true);
			base.transform.position = this.tfNode[0].position;
			this.OnEndedStep1 = OnEndedStep1;
			this.OnEnded = OnEnded;
		}

		private void Update()
		{
			if (!this.isInit)
			{
				return;
			}
			Vector2 a = Vector2.right;
			int step = this.Step;
			if (step != 0)
			{
				if (step != 1)
				{
					if (step == 2)
					{
						a = this.tfNode[2].position - base.transform.position;
						a.Normalize();
						base.transform.Translate(a * Time.deltaTime * 3f);
						float num = Mathf.Abs(this.tfNode[2].position.x - base.transform.position.x);
						if (num <= 0.5f)
						{
							this.Step = 1;
						}
					}
				}
			}
			else
			{
				a = this.tfNode[1].position - base.transform.position;
				a.Normalize();
				base.transform.Translate(a * Time.deltaTime * 2f);
				float num = Mathf.Abs(this.tfNode[1].position.x - base.transform.position.x);
				if (num <= 0.5f)
				{
					this.objump.SetActive(true);
					this.isReadyToDestroy = true;
					this.Step = 1;
					if (this.OnEndedStep1 != null)
					{
						this.OnEndedStep1();
					}
				}
			}
		}

		private void OnBecameInvisible()
		{
			if (this.isReadyToDestroy)
			{
				if (this.OnEnded != null)
				{
					this.OnEnded();
				}
				base.gameObject.SetActive(false);
			}
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.CompareTag("Rambo"))
			{
				this.isReadyToDestroy = true;
				GameManager.Instance.player.gameObject.SetActive(false);
				this.Step = 2;
			}
		}

		public Transform[] tfNode;

		private int Step;

		private bool isInit;

		private bool isReadyToDestroy;

		private Action OnEndedStep1;

		private Action OnEnded;

		public GameObject objump;
	}
}
