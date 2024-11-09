using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace CrossAdPlugin
{
	public class CrossAdsManager : Singleton<CrossAdsManager>
	{
		protected override void Awake()
		{
			base.Awake();
			if (!Directory.Exists(this.GetCacheFolderPath()))
			{
				Directory.CreateDirectory(this.GetCacheFolderPath());
			}
		}

		private string GetCacheFolderPath()
		{
			return Application.persistentDataPath + "/cache";
		}

		public void Preload(string androidConfigFileUrl, string iosConfigFileUrl, Action<CrossAdsConfig> callback = null)
		{
			if (this.isPreloadExcuted)
			{
				return;
			}
			this.isPreloadExcuted = true;
			this.PreloadConfigAndTexture(androidConfigFileUrl, callback);
		}

		public void PreloadConfigAndTexture(string configFileUrl, Action<CrossAdsConfig> callback)
		{
			UnityEngine.Debug.Log("--------------------------------------");
			this.LoadConfig(configFileUrl, delegate(CrossAdsConfig result)
			{
				if (result == null)
				{
					result = BPJsonUtil.ReadJsonFromResources<CrossAdsConfig>("DefaultConfig_Android");
				}
				this.config = result;
				if (this.config != null && this.config.adsList != null)
				{
					PlayerPrefs.SetString("countryIpUrl", this.config.countryIpUrl);
					ArrayList arrayList = this.ParseNotInstallAdList();
					this.cachedAdList = new AdsItem[arrayList.Count];
					this.cachedShowedAdTimers = new float[arrayList.Count];
					this.cachedTextures = new Texture[arrayList.Count];
					for (int i = 0; i < arrayList.Count; i++)
					{
						this.cachedAdList[i] = (AdsItem)arrayList[i];
						this.cachedShowedAdTimers[i] = 1f;
						this.cachedTextures[i] = null;
					}
					this.PreloadCachedTextures(ref this.cachedAdList);
				}
				else
				{
					this.isPreloadExcuted = false;
				}
				if (callback != null)
				{
					callback(this.config);
				}
			});
		}

		private string ParsePackageName(string adsUrl)
		{
			string[] array = adsUrl.Split(new char[]
			{
				'='
			});
			if (array != null && array.Length > 0)
			{
				return array[array.Length - 1];
			}
			return string.Empty;
		}

		private ArrayList ParseNotInstallAdList()
		{
			ArrayList arrayList = new ArrayList();
			string text = string.Empty;
			for (int i = 0; i < this.config.adsList.Length; i++)
			{
				text = this.ParsePackageName(this.config.adsList[i].adsUrl);
				if (!PackageUtils.CheckInstalled(text))
				{
					arrayList.Add(this.Config.adsList[i]);
					BPDebug.LogMessage("Not Install: " + text, false);
				}
				else
				{
					BPDebug.LogMessage("Installed: " + text, false);
				}
			}
			return arrayList;
		}

		public void LoadConfig(string configFileUrl, Action<CrossAdsConfig> onAdsLoaded)
		{
			if (this.config == null)
			{
				base.StartCoroutine(BPNetworkUtils.DownloadTxt<CrossAdsConfig>(configFileUrl, delegate(CrossAdsConfig result)
				{
					if (onAdsLoaded != null)
					{
						onAdsLoaded(result);
					}
				}, delegate(string error)
				{
					BPDebug.LogMessage(error, false);
					if (onAdsLoaded != null)
					{
						onAdsLoaded(null);
					}
				}));
			}
			else if (onAdsLoaded != null)
			{
				onAdsLoaded(this.config);
			}
		}

		private void PreloadCachedTextures(ref AdsItem[] adsList)
		{
			for (int i = 0; i < adsList.Length; i++)
			{
				CrossAdTextureInfo info = new CrossAdTextureInfo(adsList[i].floatAdsImageUrl, i);
				this.LoadLocalOrDownload(info, delegate(CrossAdTextureInfo texInfo)
				{
					this.cachedTextures[texInfo.index] = texInfo.DonwloadedTexture;
				});
			}
		}

		private void LoadLocalOrDownload(CrossAdTextureInfo info, Action<CrossAdTextureInfo> callback)
		{
			if (File.Exists(info.filePath))
			{
				BPDebug.LogMessage("Load from device: " + info.filePath, false);
				base.StartCoroutine(this.ReadImage(info, delegate(CrossAdTextureInfo rInfo)
				{
					if (callback != null)
					{
						callback(rInfo);
					}
				}));
			}
			else
			{
				base.StartCoroutine(BPNetworkUtils.DownloadImage(info, delegate(CrossAdTextureInfo rInfo)
				{
					if (callback != null)
					{
						callback(rInfo);
					}
					try
					{
						BPDebug.LogMessage("WriteAllBytes .... " + rInfo.filePath, false);
						if (rInfo.DonwloadedTexture != null)
						{
							File.WriteAllBytes(rInfo.filePath, rInfo.DonwloadedTexture.EncodeToPNG());
						}
					}
					catch (Exception ex)
					{
						BPDebug.LogMessage("WriteAllBytes error: " + ex.StackTrace, false);
					}
				}));
			}
		}

		private IEnumerator ReadImage(CrossAdTextureInfo info, Action<CrossAdTextureInfo> callback)
		{
			while (!File.Exists(info.filePath))
			{
				yield return new WaitForEndOfFrame();
			}
			byte[] texData = File.ReadAllBytes(info.filePath);
			Texture2D texture = new Texture2D(300, 300);
			texture.LoadImage(texData);
			info.SetTexture(texture);
			if (callback != null)
			{
				callback(info);
			}
			yield break;
		}

		private void CheckShowOnLoad(bool showAdsOnLoad)
		{
			if (showAdsOnLoad)
			{
				this.ShowFloatAds();
			}
			else
			{
				this.HideFloatAds();
			}
		}

		public void ShowAdList()
		{
			this.FindOrLoadAdListManager();
			if (this.adListManager != null)
			{
				this.adListManager.ShowAd();
				if (this.floatAdsManager != null)
				{
					this.floatAdsManager.transform.SetSiblingIndex(this.adListManager.transform.GetSiblingIndex());
				}
			}
		}

		public void HideAdList()
		{
			this.adListManager = UnityEngine.Object.FindObjectOfType<AdListManager>();
			if (this.adListManager != null)
			{
				this.adListManager.HideAd();
			}
		}

		public void HideFloatAds()
		{
			this.floatAdsManager = UnityEngine.Object.FindObjectOfType<FloatAdManager>();
			if (this.floatAdsManager != null)
			{
				this.floatAdsManager.HideAd();
			}
		}

		public void ShowFloatAds(Vector2 atPos, Vector2 size)
		{
			this.FindOrLoadFloatAd();
			if (this.floatAdsManager != null)
			{
				this.floatAdsManager.ShowAd(atPos, size);
			}
		}

		public void ShowFloatAds()
		{
			this.FindOrLoadFloatAd();
			if (this.floatAdsManager != null)
			{
				if (ThisPlatform.IsIphoneX)
				{
					this.floatAdsManager.ShowAd(new Vector2(50f, (float)(Screen.height - 50 - 130)), new Vector2(130f, 130f));
				}
				else
				{
					this.floatAdsManager.ShowAd(new Vector2(10f, (float)(Screen.height - 10 - 130)), new Vector2(130f, 130f));
				}
			}
		}

		private void FindOrLoadAdListManager()
		{
			if (this.adListManager == null)
			{
				this.adListManager = UnityEngine.Object.FindObjectOfType<AdListManager>();
				if (this.adListManager == null)
				{
					this.adListManager = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("AdListManager") as GameObject).GetComponent<AdListManager>();
					this.adListManager.Init(this.config);
					this.FindAndAssignParentForAd(this.adListManager.transform);
				}
			}
		}

		private void FindOrLoadFloatAd()
		{
			if (this.floatAdsManager == null)
			{
				this.floatAdsManager = UnityEngine.Object.FindObjectOfType<FloatAdManager>();
				if (this.floatAdsManager == null)
				{
					this.floatAdsManager = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("FloadAdsManager") as GameObject).GetComponent<FloatAdManager>();
					this.FindAndAssignParentForAd(this.floatAdsManager.transform);
				}
			}
		}

		private void FindAndAssignParentForAd(Transform child)
		{
			if (this.parentCanvas == null)
			{
				Canvas component = (UnityEngine.Object.Instantiate(Resources.Load("Canvas")) as GameObject).GetComponent<Canvas>();
				UnityEngine.Object.DontDestroyOnLoad(component.gameObject);
				this.parentCanvas = component.gameObject.transform;
			}
			child.parent = this.parentCanvas;
		}

		public Texture[] CachedTexture
		{
			get
			{
				return this.cachedTextures;
			}
		}

		public AdsItem[] CachedAdsItem
		{
			get
			{
				return this.cachedAdList;
			}
		}

		public float[] CachedShowedAdTimer
		{
			get
			{
				return this.cachedShowedAdTimers;
			}
			set
			{
				this.cachedShowedAdTimers = value;
			}
		}

		public CrossAdsConfig Config
		{
			get
			{
				return this.config;
			}
		}

		private Transform parentCanvas;

		private FloatAdManager floatAdsManager;

		private AdListManager adListManager;

		private CrossAdsConfig config;

		private bool isPreloadExcuted;

		private AdsItem[] cachedAdList;

		private Texture[] cachedTextures;

		private float[] cachedShowedAdTimers;
	}
}
