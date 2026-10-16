// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;
using System.Collections.Generic;
using GameFrameX.Runtime;
using UnityEngine;

namespace GameFrameX.Payment.Runtime
{
    /// <summary>
    /// 支付组件。
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GameFrameXPaymentCroppingHelper))]
    [AddComponentMenu("GameFrameX/Payment")]
    [UnityEngine.Scripting.Preserve]
    [GameFrameXAutoComponent(1100)]
    public class PaymentComponent : GameFrameworkComponent
    {
        private IPaymentManager _paymentManager;
        private readonly HashSet<IPaymentManager> _wiredManagers = new HashSet<IPaymentManager>();

        /// <summary>
        /// 购买成功事件（默认渠道与显式路由渠道的结果均会触发）
        /// </summary>
        public event EventHandler<PaymentEventArgs> OnPurchaseSuccess;

        /// <summary>
        /// 购买失败事件（默认渠道与显式路由渠道的结果均会触发）
        /// </summary>
        public event EventHandler<PaymentEventArgs> OnPurchaseFailed;

        /// <summary>
        /// 查询购买记录结果事件
        /// </summary>
        public event EventHandler<PaymentEventArgs> OnQueryPurchasesResult;

        /// <summary>
        /// 消耗购买结果事件
        /// </summary>
        public event EventHandler<PaymentEventArgs> OnConsumePurchaseResult;


        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            // 与广告域同构：模块槽位固定收敛到主包 DefaultPaymentManager（门面），
            // 渠道选择与路由全部交给 PaymentChannelRegistry，渠道包经各自 Bootstrap 自注册
            ImplementationComponentType = typeof(DefaultPaymentManager);
            InterfaceComponentType = typeof(IPaymentManager);
            base.Awake();

            _paymentManager = GameFrameworkEntry.GetModule<IPaymentManager>();
            if (_paymentManager == null)
            {
                Log.Fatal("Pay Manager is invalid.");
                return;
            }

            WireManagerEvents(_paymentManager);
        }

        /// <summary>
        /// 为支付管理器接线路由结果事件（同一管理器只接线一次）
        /// </summary>
        private void WireManagerEvents(IPaymentManager manager)
        {
            if (manager == null || !_wiredManagers.Add(manager))
            {
                return;
            }

            manager.OnPurchaseSuccess += (sender, e) => OnPurchaseSuccess?.Invoke(this, e);
            manager.OnPurchaseFailed += (sender, e) => OnPurchaseFailed?.Invoke(this, e);
            manager.OnQueryPurchasesResult += (sender, e) => OnQueryPurchasesResult?.Invoke(this, e);
            manager.OnConsumePurchaseResult += (sender, e) => OnConsumePurchaseResult?.Invoke(this, e);
        }

        /// <summary>
        /// 获取已注册的支付渠道列表（含可用性），用于 UI 展示可用支付方式
        /// </summary>
        /// <returns>渠道信息列表</returns>
        [UnityEngine.Scripting.Preserve]
        public IReadOnlyList<PaymentChannelInfo> GetAvailableChannels()
        {
            return PaymentChannelRegistry.GetAllChannelInfos();
        }

        /// <summary>
        /// 渠道是否已注册且当前可用
        /// </summary>
        /// <param name="channelId">渠道唯一标识</param>
        /// <returns>可用返回true，否则返回false</returns>
        [UnityEngine.Scripting.Preserve]
        public bool IsChannelAvailable(string channelId)
        {
            var manager = PaymentChannelRegistry.GetManager(channelId);
            return manager != null && manager.IsReady();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="isDebug">是否是沙盒购买</param>
        /// <param name="isClientVerify">是否执行客户端购买验证</param>
        [UnityEngine.Scripting.Preserve]
        public void Init(bool isDebug = false, bool isClientVerify = true)
        {
            _paymentManager.Init(isDebug, isClientVerify);
        }

        /// <summary>
        /// 支付系统是否准备好
        /// </summary>
        /// <returns>准备好返回true，否则返回false</returns>
        [UnityEngine.Scripting.Preserve]
        public bool IsReady()
        {
            return _paymentManager.IsReady();
        }

        /// <summary>
        /// 设置预加载的预定义商品ID,用于预加载缓存,注意：此方法必须在Initialize()之前调用
        /// </summary>
        /// <param name="inAppProductIds">内购商品ID列表</param>
        /// <param name="subsProductIds">订阅商品ID列表</param>
        [UnityEngine.Scripting.Preserve]
        public void SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds)
        {
            _paymentManager.SetPredefinedProductIds(inAppProductIds, subsProductIds);
        }

        /// <summary>
        /// 查询购买记录
        /// </summary>
        /// <param name="productType">产品类型</param>
        [UnityEngine.Scripting.Preserve]
        public void QueryPurchases(PaymentProductType productType)
        {
            _paymentManager.QueryPurchases(productType);
        }

        /// <summary>
        /// 消耗购买
        /// </summary>
        /// <param name="purchaseToken">购买令牌</param>
        [UnityEngine.Scripting.Preserve]
        public void ConsumePurchase(string purchaseToken)
        {
            if (string.IsNullOrEmpty(purchaseToken))
            {
                Log.Warning("Purchase token is null or empty.");
                return;
            }

            _paymentManager.ConsumePurchase(purchaseToken);
        }

        /// <summary>
        /// 购买 一次性商品
        /// </summary>
        /// <param name="productId">产品ID或SKU</param>
        /// <param name="orderId">订单ID</param>
        /// <param name="offerToken">订阅优惠令牌，仅订阅商品需要</param>
        /// <param name="customData">自定义数据</param>
        [Obsolete("请使用 Buy(PurchaseParams) 替代")]
        [UnityEngine.Scripting.Preserve]
        public void BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")
        {
            if (string.IsNullOrEmpty(productId))
            {
                Log.Warning("Product ID is null or empty.");
                return;
            }

            if (string.IsNullOrEmpty(orderId))
            {
                Log.Warning("Order ID is null or empty.");
                return;
            }

            _paymentManager.BuyInApp(productId, orderId, offerToken, customData);
        }

        /// <summary>
        /// 购买 订阅商品
        /// </summary>
        /// <param name="productId">产品ID或SKU</param>
        /// <param name="orderId">订单ID</param>
        /// <param name="offerToken">订阅优惠令牌，仅订阅商品需要</param>
        /// <param name="customData">自定义数据</param>
        [Obsolete("请使用 Buy(PurchaseParams) 替代")]
        [UnityEngine.Scripting.Preserve]
        public void BuySubs(string productId, string orderId, string offerToken = "", string customData = "")
        {
            if (string.IsNullOrEmpty(productId))
            {
                Log.Warning("Product ID is null or empty.");
                return;
            }

            if (string.IsNullOrEmpty(orderId))
            {
                Log.Warning("Order ID is null or empty.");
                return;
            }

            _paymentManager.BuySubs(productId, orderId, offerToken, customData);
        }

        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="productId">产品ID或SKU</param>
        /// <param name="productType">产品类型</param>
        /// <param name="offerToken">订阅优惠令牌，仅订阅商品需要</param>
        /// <param name="orderId">订单ID</param>
        /// <param name="customData">自定义数据</param>
        [Obsolete("请使用 Buy(PurchaseParams) 替代")]
        [UnityEngine.Scripting.Preserve]
        public void Buy(string productId, PaymentProductType productType, string orderId, string offerToken = "", string customData = "")
        {
            if (string.IsNullOrEmpty(productId))
            {
                Log.Warning("Product ID is null or empty.");
                return;
            }

            if (string.IsNullOrEmpty(orderId))
            {
                Log.Warning("Order ID is null or empty.");
                return;
            }

            _paymentManager.Buy(productId, productType, orderId, offerToken, customData);
        }

        /// <summary>
        /// 购买（推荐使用）
        /// </summary>
        /// <param name="purchaseParams">购买参数，各渠道可使用对应子类传入渠道特有参数；子类绑定的 ChannelId 会自动路由到对应渠道</param>
        [UnityEngine.Scripting.Preserve]
        public void Buy(PurchaseParams purchaseParams)
        {
            if (purchaseParams == null)
            {
                Log.Warning("Purchase params is null.");
                return;
            }

            if (string.IsNullOrEmpty(purchaseParams.ProductId))
            {
                Log.Warning("Product ID is null or empty.");
                return;
            }

            if (string.IsNullOrEmpty(purchaseParams.OrderId))
            {
                Log.Warning("Order ID is null or empty.");
                return;
            }

            IPaymentManager manager;
            if (string.IsNullOrEmpty(purchaseParams.ChannelId))
            {
                // 未指定渠道：按注册顺序取首个注册渠道；无注册渠道时回落主包门面（Editor 模拟 / 真机告警）
                manager = PaymentChannelRegistry.ResolveManager() ?? _paymentManager;
            }
            else
            {
                manager = PaymentChannelRegistry.GetManager(purchaseParams.ChannelId);
                if (manager == null)
                {
                    Log.Warning("Payment channel '{0}' is not registered.", purchaseParams.ChannelId);
                    return;
                }
            }

            WireManagerEvents(manager);
            manager.Buy(purchaseParams);
        }
    }
}