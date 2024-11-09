// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Purchasing;
// using UnityEngine.Purchasing.Extension;
// using UnityEngine.Purchasing.Security;
//
// public class InAppManager : MonoBehaviour, IStoreListener
// {
// 	public static InAppManager Instance
// 	{
// 		get
// 		{
// 			if (InAppManager.instance == null)
// 			{
// 				InAppManager.instance = UnityEngine.Object.FindObjectOfType<InAppManager>();
// 			}
// 			return InAppManager.instance;
// 		}
// 	}
//
// 	private void OnEnable()
// 	{
// 		if (this.IsInitialized())
// 		{
// 			return;
// 		}
// 	}
//
// 	private bool IsInitialized()
// 	{
// 		return InAppManager.m_StoreController != null && InAppManager.m_StoreExtensionProvider != null;
// 	}
//
// 	private void GetListSku()
// 	{
// 		if (this.isLoadSku)
// 		{
// 			return;
// 		}
// 		this.isLoadSku = true;
// 		this.ListSkuCurrent = new List<string>();
// 		foreach (string item in this.skuListAndroid)
// 		{
// 			this.ListSkuCurrent.Add(item);
// 		}
// 	}
//
// 	private void Start()
// 	{
// 		this.GetListSku();
// 		if (InAppManager.m_StoreController == null)
// 		{
// 			this.InitializePurchasing();
// 		}
// 		try
// 		{
// 			AppsFlyer.setAppsFlyerKey(this.adFlyerKey);
// 			AppsFlyer.setAppID(this.AppId);
// 			AppsFlyer.setCollectIMEI(false);
// 			AppsFlyer.init(this.adFlyerKey);
// 			AppsFlyer.trackAppLaunch();
// 		}
// 		catch
// 		{
// 		}
// 	}
//
// 	public void InitializePurchasing()
// 	{
// 		if (this.IsInitialized())
// 		{
// 			return;
// 		}
// 		ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(), new IPurchasingModule[0]);
// 		configurationBuilder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = true;
// 		configurationBuilder.Configure<IGooglePlayConfiguration>().SetPublicKey(this.lincenseKey);
// 		for (int i = 0; i < this.ListSkuCurrent.Count; i++)
// 		{
// 			configurationBuilder.AddProduct(this.ListSkuCurrent[i], ProductType.Consumable);
// 		}
// 		UnityPurchasing.Initialize(this, configurationBuilder);
// 	}
//
// 	private void TrackAppflyerPurchase(string purchaseId, decimal cost, string currency)
// 	{
// 		try
// 		{
// 			AppsFlyer.trackRichEvent("af_purchase", new Dictionary<string, string>
// 			{
// 				{
// 					"af_revenue",
// 					cost.ToString()
// 				},
// 				{
// 					"af_content_id",
// 					purchaseId
// 				},
// 				{
// 					"af_currency",
// 					currency
// 				},
// 				{
// 					"af_quantity",
// 					"1"
// 				}
// 			});
// 		}
// 		catch (Exception ex)
// 		{
// 		}
// 	}
//
// 	public void FlyerTrackingEvent(string _Event, string _EventValue)
// 	{
// 		try
// 		{
// 			AppsFlyer.trackRichEvent(_Event, new Dictionary<string, string>
// 			{
// 				{
// 					_Event,
// 					_EventValue
// 				}
// 			});
// 		}
// 		catch
// 		{
// 		}
// 	}
//
// 	public void RestorePurchases()
// 	{
// 		this.isClickRestore = true;
// 		PopupManager.Instance.ShowMiniLoading();
// 		if (!this.IsInitialized())
// 		{
// 			BPDebug.LogMessage("RestorePurchases FAIL. Not initialized.", false);
// 			PopupManager.Instance.CloseMiniLoading();
// 			PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Restore_purchase_failed, null), PopupManager.Instance.GetText(Localization0.Restore_Purchase, null));
// 			return;
// 		}
// 		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
// 		{
// 			BPDebug.LogMessage("RestorePurchases started ...", false);
// 			IAppleExtensions extension = InAppManager.m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
// 			extension.RestoreTransactions(delegate(bool result)
// 			{
// 				PopupManager.Instance.CloseMiniLoading();
// 				if (result)
// 				{
// 					for (int i = 0; i < InAppManager.m_StoreController.products.all.Length; i++)
// 					{
// 						if (InAppManager.m_StoreController.products.all[i].hasReceipt)
// 						{
// 							if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[33], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Machine_Gun, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[14], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Yoo_na, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[25], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Yoo_na, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[15], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Ice_Gun, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[26], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Ice_Gun, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[16], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.MGL_140, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 								PopupManager.Instance.SaveReward(Item.Rocket, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[27], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.MGL_140, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 								PopupManager.Instance.SaveReward(Item.Rocket, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[17], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Sniper, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 								PopupManager.Instance.SaveReward(Item.Laser_Gun, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[28], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Sniper, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 								PopupManager.Instance.SaveReward(Item.Laser_Gun, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[18], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Sword, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 								PopupManager.Instance.SaveReward(Item.Thunder_Shot, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[29], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Sword, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 								PopupManager.Instance.SaveReward(Item.Thunder_Shot, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[19], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Dvornikov, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							else if (string.Equals(InAppManager.m_StoreController.products.all[i].definition.id, this.ListSkuCurrent[30], StringComparison.Ordinal))
// 							{
// 								PopupManager.Instance.SaveReward(Item.Dvornikov, 1, base.name + "_" + InAppManager.m_StoreController.products.all[i].definition.id, null);
// 							}
// 							PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Restore_purchase_success, null), PopupManager.Instance.GetText(Localization0.Restore_Purchase, null));
// 						}
// 					}
// 				}
// 				BPDebug.LogMessage("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.", false);
// 			});
// 		}
// 		else
// 		{
// 			PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Restore_purchase_failed, null), PopupManager.Instance.GetText(Localization0.Restore_Purchase, null));
// 		}
// 	}
//
// 	public void PurchaseProduct(string sku, Action<InAppManager.InforCallback> OnPurchaseEnded)
// 	{
// 		if (this.IsInitialized())
// 		{
// 			this.OnPurchaseEnded = OnPurchaseEnded;
// 			Product product = InAppManager.m_StoreController.products.WithID(sku);
// 			if (product != null && product.availableToPurchase)
// 			{
// 				UnityEngine.Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
// 				InAppManager.m_StoreController.InitiatePurchase(product);
// 			}
// 			else
// 			{
// 				UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
// 				PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Not_purchasing_product, null), PopupManager.Instance.GetText(Localization0.Purchase_Failed, null));
// 			}
// 		}
// 		else
// 		{
// 			UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
// 			PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Please_check_your_network_connection_and_try_again, null), PopupManager.Instance.GetText(Localization0.Purchase_Failed, null));
// 		}
// 	}
//
// 	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
// 	{
// 		BPDebug.LogMessage("OnInitialized: PASS", false);
// 		InAppManager.m_StoreController = controller;
// 		InAppManager.m_StoreExtensionProvider = extensions;
// 	}
//
// 	public void OnInitializeFailed(InitializationFailureReason error)
// 	{
// 		BPDebug.LogMessage("OnInitializeFailed InitializationFailureReason:" + error, false);
// 	}
//
// 	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchase)
// 	{
// 		bool flag = true;
// 		CrossPlatformValidator crossPlatformValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
// 		try
// 		{
// 			IPurchaseReceipt[] array = crossPlatformValidator.Validate(purchase.purchasedProduct.receipt);
// 			UnityEngine.Debug.Log("Receipt is valid. Contents:");
// 			foreach (IPurchaseReceipt purchaseReceipt in array)
// 			{
// 				UnityEngine.Debug.Log(purchaseReceipt.productID);
// 				UnityEngine.Debug.Log(purchaseReceipt.purchaseDate);
// 				UnityEngine.Debug.Log(purchaseReceipt.transactionID);
// 			}
// 		}
// 		catch (IAPSecurityException)
// 		{
// 			UnityEngine.Debug.Log("Invalid receipt, not unlocking content");
// 			flag = false;
// 			PopupManager.Instance.ShowDialog(delegate(bool ok)
// 			{
// 				InAppManager.InforCallback inforCallback = new InAppManager.InforCallback();
// 				inforCallback.isSuccess = false;
// 				if (this.OnPurchaseEnded != null)
// 				{
// 					this.OnPurchaseEnded(inforCallback);
// 				}
// 			}, 0, "Purchase failed!", "IN-APP PURCHASE");
// 		}
// 		if (flag)
// 		{
// 			this.GetInappSuccess(purchase.purchasedProduct.definition.id);
// 			this.TrackAppflyerPurchase(purchase.purchasedProduct.definition.id, (decimal)this.GetPriceValue(purchase.purchasedProduct.definition.id), this.GetCurrencyCode(purchase.purchasedProduct.definition.id));
// 		}
// 		return PurchaseProcessingResult.Complete;
// 	}
//
// 	private void GetInappSuccess(string sku)
// 	{
// 		InAppManager.InforCallback inforCallback = new InAppManager.InforCallback();
// 		inforCallback.isSuccess = true;
// 		inforCallback.sku = sku;
// 		if (this.OnPurchaseEnded != null)
// 		{
// 			this.OnPurchaseEnded(inforCallback);
// 		}
// 		float valueSale = SaleManager.Instance.ValueSale;
// 		int idsale = SaleManager.Instance.IDSale;
// 		if (sku.Equals(this.ListSkuCurrent[0]))
// 		{
// 			int num = this.Ms[0];
// 			if (valueSale > 0f && (idsale == -1 || idsale == 0))
// 			{
// 				num += (int)((float)num * valueSale);
// 			}
// 			this.SaveAndShowListReward(InAppManager.SKU.GOLD1, num);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[1]))
// 		{
// 			int num = this.Ms[1];
// 			if (valueSale > 0f && (idsale == -1 || idsale == 1))
// 			{
// 				num += (int)((float)num * valueSale);
// 			}
// 			this.SaveAndShowListReward(InAppManager.SKU.GOLD2, num);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[2]))
// 		{
// 			int num = this.Ms[2];
// 			if (valueSale > 0f && (idsale == -1 || idsale == 2))
// 			{
// 				num += (int)((float)num * valueSale);
// 			}
// 			this.SaveAndShowListReward(InAppManager.SKU.GOLD3, num);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[3]))
// 		{
// 			int num = this.Ms[3];
// 			if (valueSale > 0f && (idsale == -1 || idsale == 3))
// 			{
// 				num += (int)((float)num * valueSale);
// 			}
// 			this.SaveAndShowListReward(InAppManager.SKU.GOLD4, num);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[4]))
// 		{
// 			int num = this.Ms[4];
// 			if (valueSale > 0f && (idsale == -1 || idsale == 4))
// 			{
// 				num += (int)((float)num * valueSale);
// 			}
// 			this.SaveAndShowListReward(InAppManager.SKU.GOLD5, num);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[5]))
// 		{
// 			int num = this.Ms[5];
// 			if (valueSale > 0f && (idsale == -1 || idsale == 5))
// 			{
// 				num += (int)((float)num * valueSale);
// 			}
// 			this.SaveAndShowListReward(InAppManager.SKU.GOLD6, num);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[6]) || sku.Equals(this.ListSkuCurrent[31]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[6]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_0, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_0_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[7]) || sku.Equals(this.ListSkuCurrent[32]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[7]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_1, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_1_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[8]) || sku.Equals(this.ListSkuCurrent[33]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[8]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_2, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_2_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[9]) || sku.Equals(this.ListSkuCurrent[20]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[9]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_0, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_0_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[10]) || sku.Equals(this.ListSkuCurrent[21]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[10]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_1, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_1_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[11]) || sku.Equals(this.ListSkuCurrent[22]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[11]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_2, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_2_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[12]) || sku.Equals(this.ListSkuCurrent[23]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[12]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_3, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_3_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[13]) || sku.Equals(this.ListSkuCurrent[24]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[13]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_4, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_4_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[14]) || sku.Equals(this.ListSkuCurrent[25]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[14]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_5, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_5_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[15]) || sku.Equals(this.ListSkuCurrent[26]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[15]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_6, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_6_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[16]) || sku.Equals(this.ListSkuCurrent[27]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[16]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_7, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_7_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[17]) || sku.Equals(this.ListSkuCurrent[28]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[17]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_8, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_8_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[18]) || sku.Equals(this.ListSkuCurrent[29]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[18]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_9, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_9_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[19]) || sku.Equals(this.ListSkuCurrent[30]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[19]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_10, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_10_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[40]) || sku.Equals(this.ListSkuCurrent[41]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[40]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_11, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_11_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[42]) || sku.Equals(this.ListSkuCurrent[43]))
// 		{
// 			if (sku.Equals(this.ListSkuCurrent[42]))
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_12, 0);
// 			}
// 			else
// 			{
// 				this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_12_SALE, 0);
// 			}
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[34]))
// 		{
// 			this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_M4A1_SALE, 0);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[35]))
// 		{
// 			this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_MACHINE_SALE, 0);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[36]))
// 		{
// 			this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_ICE_SALE, 0);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[37]))
// 		{
// 			this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_SPREAD_SALE, 0);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[38]))
// 		{
// 			this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_MGL_SALE, 0);
// 			
// 			return;
// 		}
// 		if (sku.Equals(this.ListSkuCurrent[39]))
// 		{
// 			this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_MONEY_SALE, 0);
// 			
// 			return;
// 		}
// 	}
//
// 	private void SaveAndShowListReward(InAppManager.SKU sku, int gem = 0)
// 	{
// 		if (!DataLoader.dataPackInApp.ContainsKey(sku.ToString()))
// 		{
// 			UnityEngine.Debug.LogError("Lỗi null Key InAppManager/ShowPopupListReward");
// 			return;
// 		}
// 		InforReward[] array = new InforReward[DataLoader.dataPackInApp[sku.ToString()]["reward"].Count];
// 		for (int i = 0; i < array.Length; i++)
// 		{
// 			int item = DataLoader.dataPackInApp[sku.ToString()]["reward"][i].ToInt();
// 			int amount = DataLoader.dataPackInApp[sku.ToString()]["amount"][i].ToInt();
// 			switch (sku)
// 			{
// 			case InAppManager.SKU.GOLD1:
// 			case InAppManager.SKU.GOLD2:
// 			case InAppManager.SKU.GOLD3:
// 			case InAppManager.SKU.GOLD4:
// 			case InAppManager.SKU.GOLD5:
// 			case InAppManager.SKU.GOLD6:
// 				amount = this.Ms[(int)sku] + (int)(SaleManager.Instance.ValueSale * (float)this.Ms[(int)sku]);
// 				break;
// 			}
// 			PopupManager.Instance.SaveReward((Item)item, amount, "IAP", null);
// 			array[i] = new InforReward();
// 			array[i].amount = amount;
// 			array[i].item = (Item)item;
// 		}
// 		PopupManager.Instance.ShowCongratulation(array, false, null);
// 	}
//
// 	public string GetSku(InAppManager.SKU eSKU)
// 	{
// 		return this.ListSkuCurrent[(int)eSKU];
// 	}
//
// 	public string GetPrice(InAppManager.SKU eSKU)
// 	{
// 		string result = string.Empty;
// 		try
// 		{
// 			result = this.GetPrice(this.GetSku(eSKU));
// 		}
// 		catch
// 		{
// 		}
// 		return result;
// 	}
//
// 	public string GetPrice(string sku)
// 	{
// 		if (InAppManager.m_StoreController == null)
// 		{
// 			return null;
// 		}
// 		Product product = InAppManager.m_StoreController.products.WithID(sku);
// 		if (product != null)
// 		{
// 			return product.metadata.localizedPriceString;
// 		}
// 		return null;
// 	}
//
// 	public float GetPriceValue(string sku)
// 	{
// 		UnityEngine.Debug.Log("IsInitialized(): " + this.IsInitialized());
// 		if (InAppManager.m_StoreController == null)
// 		{
// 			return 0f;
// 		}
// 		Product product = InAppManager.m_StoreController.products.WithID(sku);
// 		if (product != null)
// 		{
// 			decimal localizedPrice = product.metadata.localizedPrice;
// 			return (float)localizedPrice;
// 		}
// 		return 0f;
// 	}
//
// 	public string GetDescription(string sku)
// 	{
// 		if (InAppManager.m_StoreController == null)
// 		{
// 			return "UnKnow";
// 		}
// 		Product product = InAppManager.m_StoreController.products.WithID(sku);
// 		if (product != null)
// 		{
// 			return product.metadata.localizedDescription;
// 		}
// 		return "UnKnow";
// 	}
//
// 	public string GetCurrencyCode(string sku)
// 	{
// 		if (InAppManager.m_StoreController == null)
// 		{
// 			return "USD";
// 		}
// 		Product product = InAppManager.m_StoreController.products.WithID(sku);
// 		if (product != null)
// 		{
// 			return product.metadata.isoCurrencyCode;
// 		}
// 		return "USD";
// 	}
//
// 	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
// 	{
// 	}
//
// 	private static InAppManager instance;
//
// 	public int[] USD;
//
// 	public int[] Ms = new int[]
// 	{
// 		40,
// 		150,
// 		300,
// 		900,
// 		2000,
// 		6000
// 	};
//
// 	private bool _isInitialized;
//
// 	public string lincenseKey;
//
// 	[SerializeField]
// 	[Header("________Android________")]
// 	private List<string> skuListAndroid;
//
// 	[Header("________IOS________")]
// 	[SerializeField]
// 	private List<string> skuListIOS;
//
// 	public List<string> usdList;
//
// 	public List<string> ListSkuCurrent;
//
// 	private static IStoreController m_StoreController;
//
// 	private static IExtensionProvider m_StoreExtensionProvider;
//
// 	private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";
//
// 	private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";
//
// 	private ProductMetadata m_ProductMetadata;
//
// 	private int coin;
//
// 	private bool isClickRestore;
//
// 	private Action<InAppManager.InforCallback> OnPurchaseEnded;
//
// 	[Header("_____________Appflyer____________")]
// 	public string adFlyerKey;
//
// 	public string AppId;
//
// 	public string AppIdIOS;
//
// 	private bool isLoadSku;
//
// 	public class InforCallback
// 	{
// 		public bool isSuccess { get; set; }
//
// 		public string sku { get; set; }
// 	}
//
// 	public enum SKU
// 	{
// 		GOLD1,
// 		GOLD2,
// 		GOLD3,
// 		GOLD4,
// 		GOLD5,
// 		GOLD6,
// 		STARTERPACK_PACK_0,
// 		STARTERPACK_PACK_1,
// 		STARTERPACK_PACK_2,
// 		DAILYSALE_PACK_0,
// 		DAILYSALE_PACK_1,
// 		DAILYSALE_PACK_2,
// 		DAILYSALE_PACK_3,
// 		DAILYSALE_PACK_4,
// 		DAILYSALE_PACK_5,
// 		DAILYSALE_PACK_6,
// 		DAILYSALE_PACK_7,
// 		DAILYSALE_PACK_8,
// 		DAILYSALE_PACK_9,
// 		DAILYSALE_PACK_10,
// 		DAILYSALE_PACK_0_SALE,
// 		DAILYSALE_PACK_1_SALE,
// 		DAILYSALE_PACK_2_SALE,
// 		DAILYSALE_PACK_3_SALE,
// 		DAILYSALE_PACK_4_SALE,
// 		DAILYSALE_PACK_5_SALE,
// 		DAILYSALE_PACK_6_SALE,
// 		DAILYSALE_PACK_7_SALE,
// 		DAILYSALE_PACK_8_SALE,
// 		DAILYSALE_PACK_9_SALE,
// 		DAILYSALE_PACK_10_SALE,
// 		STARTERPACK_PACK_0_SALE,
// 		STARTERPACK_PACK_1_SALE,
// 		STARTERPACK_PACK_2_SALE,
// 		SERVER_PACK_M4A1_SALE,
// 		SERVER_PACK_MACHINE_SALE,
// 		SERVER_PACK_ICE_SALE,
// 		SERVER_PACK_SPREAD_SALE,
// 		SERVER_PACK_MGL_SALE,
// 		SERVER_PACK_MONEY_SALE,
// 		DAILYSALE_PACK_11,
// 		DAILYSALE_PACK_11_SALE,
// 		DAILYSALE_PACK_12,
// 		DAILYSALE_PACK_12_SALE
// 	}
// }
