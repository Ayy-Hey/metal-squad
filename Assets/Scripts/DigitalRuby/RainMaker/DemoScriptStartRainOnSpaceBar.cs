using System;
using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class DemoScriptStartRainOnSpaceBar : MonoBehaviour
	{
		private void Start()
		{
			if (this.RainScript == null)
			{
				return;
			}
			this.RainScript.EnableWind = false;
		}

		private void Update()
		{
			if (this.RainScript == null)
			{
				return;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				this.RainScript.RainIntensity = ((this.RainScript.RainIntensity != 0f) ? 0f : 1f);
				this.RainScript.EnableWind = !this.RainScript.EnableWind;
			}
		}

		public BaseRainScript RainScript;
	}
}
