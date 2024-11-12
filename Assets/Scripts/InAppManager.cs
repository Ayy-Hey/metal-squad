using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


public class InAppManager : MonoBehaviour
{
    public static InAppManager Instance
    {
        get
        {
            if (InAppManager.instance == null)
            {
                InAppManager.instance = UnityEngine.Object.FindObjectOfType<InAppManager>();
            }
            return InAppManager.instance;
        }
    }
	
	
    [SerializeField]
    [Header("________Android________")]
    private List<string> skuListAndroid;
	
    public List<string> ListSkuCurrent;


    private void OnEnable()
    {
        IAPManager.OnPurchaseCompleted += ProcessPurchase;
        IAPManager.OnPurchaseFailed += OnPurchaseFailed;
    }

    private void OnDisable()
    {
        IAPManager.OnPurchaseCompleted -= ProcessPurchase;
        IAPManager.OnPurchaseFailed -= OnPurchaseFailed;

    }

    private void ProcessPurchase(Product product)
    {
        this.GetInappSuccess(product.definition.id);
        this.TrackAppflyerPurchase(product.definition.id, (decimal)this.GetPriceValue(product.definition.id), this.GetCurrencyCode(product.definition.id));
    }
	
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
		
    }

    private bool IsInitialized()
    {
        return IAPManager.Instance.IsInitialized;
    }

    private void GetListSku()
    {
        if (this.isLoadSku)
        {
            return;
        }
        this.isLoadSku = true;
        this.ListSkuCurrent = new List<string>();
        foreach (string item in this.skuListAndroid)
        {
            this.ListSkuCurrent.Add(item);
        }
    }

    private void Start()
    {
        GetListSku();
        InitializePurchasing();

        try
        {
            AppsFlyer.setAppsFlyerKey(this.adFlyerKey);
            AppsFlyer.setAppID(this.AppId);
            AppsFlyer.setCollectIMEI(false);
            AppsFlyer.init(this.adFlyerKey);
            AppsFlyer.trackAppLaunch();
        }
        catch
        {
        }
    }

    private void InitializePurchasing()
    {
        IAPManager.Instance.Initialize(ListSkuCurrent);
    }

    private void TrackAppflyerPurchase(string purchaseId, decimal cost, string currency)
    {
        try
        {
            AppsFlyer.trackRichEvent("af_purchase", new Dictionary<string, string>
            {
                {
                    "af_revenue",
                    cost.ToString()
                },
                {
                    "af_content_id",
                    purchaseId
                },
                {
                    "af_currency",
                    currency
                },
                {
                    "af_quantity",
                    "1"
                }
            });
        }
        catch (Exception ex)
        {
        }
    }

    public void FlyerTrackingEvent(string _Event, string _EventValue)
    {
        try
        {
            AppsFlyer.trackRichEvent(_Event, new Dictionary<string, string>
            {
                {
                    _Event,
                    _EventValue
                }
            });
        }
        catch
        {
        }
    }

    public void RestorePurchases()
    {
        this.isClickRestore = true;
        PopupManager.Instance.ShowMiniLoading();
        if (!this.IsInitialized())
        {
            BPDebug.LogMessage("RestorePurchases FAIL. Not initialized.", false);
            PopupManager.Instance.CloseMiniLoading();
            PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Restore_purchase_failed, null), PopupManager.Instance.GetText(Localization0.Restore_Purchase, null));
            return;
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            BPDebug.LogMessage("RestorePurchases started ...", false);
            IAppleExtensions extension = IAPManager.Instance.StoreExtensions.GetExtension<IAppleExtensions>();
            extension.RestoreTransactions(delegate(bool result)
            {
                PopupManager.Instance.CloseMiniLoading();
                if (result)
                {
                    for (int i = 0; i < IAPManager.Instance.StoreController.products.all.Length; i++)
                    {
                        if (IAPManager.Instance.StoreController.products.all[i].hasReceipt)
                        {
                            if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[33], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Machine_Gun, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[14], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Yoo_na, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[25], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Yoo_na, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[15], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Ice_Gun, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[26], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Ice_Gun, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[16], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.MGL_140, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                                PopupManager.Instance.SaveReward(Item.Rocket, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[27], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.MGL_140, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                                PopupManager.Instance.SaveReward(Item.Rocket, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[17], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Sniper, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                                PopupManager.Instance.SaveReward(Item.Laser_Gun, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[28], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Sniper, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                                PopupManager.Instance.SaveReward(Item.Laser_Gun, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[18], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Sword, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                                PopupManager.Instance.SaveReward(Item.Thunder_Shot, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[29], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Sword, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                                PopupManager.Instance.SaveReward(Item.Thunder_Shot, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[19], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Dvornikov, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            else if (string.Equals(IAPManager.Instance.StoreController.products.all[i].definition.id, this.ListSkuCurrent[30], StringComparison.Ordinal))
                            {
                                PopupManager.Instance.SaveReward(Item.Dvornikov, 1, base.name + "_" + IAPManager.Instance.StoreController.products.all[i].definition.id, null);
                            }
                            PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Restore_purchase_success, null), PopupManager.Instance.GetText(Localization0.Restore_Purchase, null));
                        }
                    }
                }
                BPDebug.LogMessage("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.", false);
            });
        }
        else
        {
            PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Restore_purchase_failed, null), PopupManager.Instance.GetText(Localization0.Restore_Purchase, null));
        }
    }

    public void PurchaseProduct(string sku, Action<InforCallback> OnPurchaseEnded)
    {
        this.OnPurchaseEnded = OnPurchaseEnded;

        if (IsInitialized())
        {
            IAPManager.Instance.Purchase(sku, result =>
            {

                if (result)
                {
                    var infoCallback = new InAppManager.InforCallback
                    {
                        isSuccess = true,
                        sku = sku
                    };
                    OnPurchaseEnded?.Invoke(infoCallback);
                }
                else
                {
                    Debug.Log(
                        "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    PopupManager.Instance.ShowDialog(null, 0,
                        PopupManager.Instance.GetText(Localization0.Not_purchasing_product, null),
                        PopupManager.Instance.GetText(Localization0.Purchase_Failed, null));
                }
            });
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
            PopupManager.Instance.ShowDialog(null, 0, 
                PopupManager.Instance.GetText(Localization0.Please_check_your_network_connection_and_try_again, null),
                PopupManager.Instance.GetText(Localization0.Purchase_Failed, null));
        }

        return;
        if (this.IsInitialized())
        {
            this.OnPurchaseEnded = OnPurchaseEnded;
            var product = IAPManager.Instance.StoreController.products.WithID(sku);
            product = IAPManager.Instance.StoreController.products.WithID(sku);
            if (product != null && product.availableToPurchase)
            {
                UnityEngine.Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                IAPManager.Instance.StoreController.InitiatePurchase(product);
            }
            else
            {
                UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Not_purchasing_product, null), PopupManager.Instance.GetText(Localization0.Purchase_Failed, null));
            }
        }
        else
        {
            UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
            PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.Please_check_your_network_connection_and_try_again, null), PopupManager.Instance.GetText(Localization0.Purchase_Failed, null));
        }
    }
	

    private void GetInappSuccess(string sku)
    {
        InAppManager.InforCallback inforCallback = new InAppManager.InforCallback();
        inforCallback.isSuccess = true;
        inforCallback.sku = sku;
        if (this.OnPurchaseEnded != null)
        {
            this.OnPurchaseEnded(inforCallback);
        }
        float valueSale = SaleManager.Instance.ValueSale;
        int idsale = SaleManager.Instance.IDSale;
        if (sku.Equals(this.ListSkuCurrent[0]))
        {
            int num = this.Ms[0];
            if (valueSale > 0f && (idsale == -1 || idsale == 0))
            {
                num += (int)((float)num * valueSale);
            }
            this.SaveAndShowListReward(InAppManager.SKU.GOLD1, num);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[1]))
        {
            int num = this.Ms[1];
            if (valueSale > 0f && (idsale == -1 || idsale == 1))
            {
                num += (int)((float)num * valueSale);
            }
            this.SaveAndShowListReward(InAppManager.SKU.GOLD2, num);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[2]))
        {
            int num = this.Ms[2];
            if (valueSale > 0f && (idsale == -1 || idsale == 2))
            {
                num += (int)((float)num * valueSale);
            }
            this.SaveAndShowListReward(InAppManager.SKU.GOLD3, num);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[3]))
        {
            int num = this.Ms[3];
            if (valueSale > 0f && (idsale == -1 || idsale == 3))
            {
                num += (int)((float)num * valueSale);
            }
            this.SaveAndShowListReward(InAppManager.SKU.GOLD4, num);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[4]))
        {
            int num = this.Ms[4];
            if (valueSale > 0f && (idsale == -1 || idsale == 4))
            {
                num += (int)((float)num * valueSale);
            }
            this.SaveAndShowListReward(InAppManager.SKU.GOLD5, num);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[5]))
        {
            int num = this.Ms[5];
            if (valueSale > 0f && (idsale == -1 || idsale == 5))
            {
                num += (int)((float)num * valueSale);
            }
            this.SaveAndShowListReward(InAppManager.SKU.GOLD6, num);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[6]) || sku.Equals(this.ListSkuCurrent[31]))
        {
            if (sku.Equals(this.ListSkuCurrent[6]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_0, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_0_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[7]) || sku.Equals(this.ListSkuCurrent[32]))
        {
            if (sku.Equals(this.ListSkuCurrent[7]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_1, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_1_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[8]) || sku.Equals(this.ListSkuCurrent[33]))
        {
            if (sku.Equals(this.ListSkuCurrent[8]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_2, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.STARTERPACK_PACK_2_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[9]) || sku.Equals(this.ListSkuCurrent[20]))
        {
            if (sku.Equals(this.ListSkuCurrent[9]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_0, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_0_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[10]) || sku.Equals(this.ListSkuCurrent[21]))
        {
            if (sku.Equals(this.ListSkuCurrent[10]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_1, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_1_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[11]) || sku.Equals(this.ListSkuCurrent[22]))
        {
            if (sku.Equals(this.ListSkuCurrent[11]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_2, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_2_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[12]) || sku.Equals(this.ListSkuCurrent[23]))
        {
            if (sku.Equals(this.ListSkuCurrent[12]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_3, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_3_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[13]) || sku.Equals(this.ListSkuCurrent[24]))
        {
            if (sku.Equals(this.ListSkuCurrent[13]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_4, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_4_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[14]) || sku.Equals(this.ListSkuCurrent[25]))
        {
            if (sku.Equals(this.ListSkuCurrent[14]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_5, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_5_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[15]) || sku.Equals(this.ListSkuCurrent[26]))
        {
            if (sku.Equals(this.ListSkuCurrent[15]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_6, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_6_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[16]) || sku.Equals(this.ListSkuCurrent[27]))
        {
            if (sku.Equals(this.ListSkuCurrent[16]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_7, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_7_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[17]) || sku.Equals(this.ListSkuCurrent[28]))
        {
            if (sku.Equals(this.ListSkuCurrent[17]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_8, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_8_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[18]) || sku.Equals(this.ListSkuCurrent[29]))
        {
            if (sku.Equals(this.ListSkuCurrent[18]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_9, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_9_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[19]) || sku.Equals(this.ListSkuCurrent[30]))
        {
            if (sku.Equals(this.ListSkuCurrent[19]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_10, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_10_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[40]) || sku.Equals(this.ListSkuCurrent[41]))
        {
            if (sku.Equals(this.ListSkuCurrent[40]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_11, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_11_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[42]) || sku.Equals(this.ListSkuCurrent[43]))
        {
            if (sku.Equals(this.ListSkuCurrent[42]))
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_12, 0);
            }
            else
            {
                this.SaveAndShowListReward(InAppManager.SKU.DAILYSALE_PACK_12_SALE, 0);
            }
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[34]))
        {
            this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_M4A1_SALE, 0);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[35]))
        {
            this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_MACHINE_SALE, 0);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[36]))
        {
            this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_ICE_SALE, 0);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[37]))
        {
            this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_SPREAD_SALE, 0);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[38]))
        {
            this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_MGL_SALE, 0);
			
            return;
        }
        if (sku.Equals(this.ListSkuCurrent[39]))
        {
            this.SaveAndShowListReward(InAppManager.SKU.SERVER_PACK_MONEY_SALE, 0);
			
            return;
        }
    }

    private void SaveAndShowListReward(InAppManager.SKU sku, int gem = 0)
    {
        if (!DataLoader.dataPackInApp.ContainsKey(sku.ToString()))
        {
            UnityEngine.Debug.LogError("Lỗi null Key InAppManager/ShowPopupListReward");
            return;
        }
        InforReward[] array = new InforReward[DataLoader.dataPackInApp[sku.ToString()]["reward"].Count];
        for (int i = 0; i < array.Length; i++)
        {
            int item = DataLoader.dataPackInApp[sku.ToString()]["reward"][i].ToInt();
            int amount = DataLoader.dataPackInApp[sku.ToString()]["amount"][i].ToInt();
            switch (sku)
            {
                case InAppManager.SKU.GOLD1:
                case InAppManager.SKU.GOLD2:
                case InAppManager.SKU.GOLD3:
                case InAppManager.SKU.GOLD4:
                case InAppManager.SKU.GOLD5:
                case InAppManager.SKU.GOLD6:
                    amount = this.Ms[(int)sku] + (int)(SaleManager.Instance.ValueSale * (float)this.Ms[(int)sku]);
                    break;
            }
            PopupManager.Instance.SaveReward((Item)item, amount, "IAP", null);
            array[i] = new InforReward();
            array[i].amount = amount;
            array[i].item = (Item)item;
        }
        PopupManager.Instance.ShowCongratulation(array, false, null);
    }

    public string GetSku(InAppManager.SKU eSKU)
    {
        return this.ListSkuCurrent[(int)eSKU];
    }

    public string GetPrice(InAppManager.SKU eSKU)
    {
        string result = string.Empty;
        try
        {
            result = this.GetPrice(this.GetSku(eSKU));
        }
        catch
        {
        }
        return result;
    }

    public string GetPrice(string sku)
    {
        if (IAPManager.Instance.StoreController == null)
        {
            return null;
        }
        Product product = IAPManager.Instance.StoreController.products.WithID(sku);
        if (product != null)
        {
            return product.metadata.localizedPriceString;
        }
        return null;
    }

    public float GetPriceValue(string sku)
    {
        UnityEngine.Debug.Log("IsInitialized(): " + this.IsInitialized());
        if (IAPManager.Instance.StoreController == null)
        {
            return 0f;
        }
        Product product = IAPManager.Instance.StoreController.products.WithID(sku);
        if (product != null)
        {
            decimal localizedPrice = product.metadata.localizedPrice;
            return (float)localizedPrice;
        }
        return 0f;
    }

    public string GetDescription(string sku)
    {
        if (IAPManager.Instance.StoreController == null)
        {
            return "UnKnow";
        }
        Product product = IAPManager.Instance.StoreController.products.WithID(sku);
        if (product != null)
        {
            return product.metadata.localizedDescription;
        }
        return "UnKnow";
    }

    public string GetCurrencyCode(string sku)
    {
        if (IAPManager.Instance.StoreController == null)
        {
            return "USD";
        }
        Product product = IAPManager.Instance.StoreController.products.WithID(sku);
        if (product != null)
        {
            return product.metadata.isoCurrencyCode;
        }
        return "USD";
    }



    private static InAppManager instance;

    public int[] USD;

    public int[] Ms = new int[]
    {
        40,
        150,
        300,
        900,
        2000,
        6000
    };

    private bool _isInitialized;

    public string lincenseKey;



    [Header("________IOS________")]
    [SerializeField]
    private List<string> skuListIOS;

    public List<string> usdList;
	

    private ProductMetadata m_ProductMetadata;

    private int coin;

    private bool isClickRestore;

    private Action<InforCallback> OnPurchaseEnded;

    [Header("_____________Appflyer____________")]
    public string adFlyerKey;

    public string AppId;

    public string AppIdIOS;

    private bool isLoadSku;

    public class InforCallback
    {
        public bool isSuccess { get; set; }

        public string sku { get; set; }
    }

    public enum SKU
    {
        GOLD1,
        GOLD2,
        GOLD3,
        GOLD4,
        GOLD5,
        GOLD6,
        STARTERPACK_PACK_0,
        STARTERPACK_PACK_1,
        STARTERPACK_PACK_2,
        DAILYSALE_PACK_0,
        DAILYSALE_PACK_1,
        DAILYSALE_PACK_2,
        DAILYSALE_PACK_3,
        DAILYSALE_PACK_4,
        DAILYSALE_PACK_5,
        DAILYSALE_PACK_6,
        DAILYSALE_PACK_7,
        DAILYSALE_PACK_8,
        DAILYSALE_PACK_9,
        DAILYSALE_PACK_10,
        DAILYSALE_PACK_0_SALE,
        DAILYSALE_PACK_1_SALE,
        DAILYSALE_PACK_2_SALE,
        DAILYSALE_PACK_3_SALE,
        DAILYSALE_PACK_4_SALE,
        DAILYSALE_PACK_5_SALE,
        DAILYSALE_PACK_6_SALE,
        DAILYSALE_PACK_7_SALE,
        DAILYSALE_PACK_8_SALE,
        DAILYSALE_PACK_9_SALE,
        DAILYSALE_PACK_10_SALE,
        STARTERPACK_PACK_0_SALE,
        STARTERPACK_PACK_1_SALE,
        STARTERPACK_PACK_2_SALE,
        SERVER_PACK_M4A1_SALE,
        SERVER_PACK_MACHINE_SALE,
        SERVER_PACK_ICE_SALE,
        SERVER_PACK_SPREAD_SALE,
        SERVER_PACK_MGL_SALE,
        SERVER_PACK_MONEY_SALE,
        DAILYSALE_PACK_11,
        DAILYSALE_PACK_11_SALE,
        DAILYSALE_PACK_12,
        DAILYSALE_PACK_12_SALE
    }
}