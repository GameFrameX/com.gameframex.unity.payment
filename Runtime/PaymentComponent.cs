// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Collections.Generic;
using GameFrameX.Runtime;
using UnityEngine;

namespace GameFrameX.Payment.Runtime
{
    /// <summary>
    /// 支付组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Payment")]
    [UnityEngine.Scripting.Preserve]
    public class PaymentComponent : GameFrameworkComponent
    {
        private IPaymentManager _paymentManager;
        [SerializeField] private string m_componentAndroidType = "GameFrameX.Payment.Google.Runtime.GooglePaymentManager";
        [SerializeField] private string m_componentIOSType = "GameFrameX.Payment.Apple.Runtime.ApplePaymentManager";

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
#if UNITY_ANDROID
            componentType = m_componentAndroidType;
#elif UNITY_IOS
            componentType = m_componentIOSType;
#endif
            ImplementationComponentType = Utility.Assembly.GetType(componentType);
            InterfaceComponentType = typeof(IPaymentManager);
            base.Awake();

            _paymentManager = GameFrameworkEntry.GetModule<IPaymentManager>();
            if (_paymentManager == null)
            {
                Log.Fatal("Pay Manager is invalid.");
                return;
            }
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
        /// <param name="productType">产品类型，inapp/subs</param>
        [UnityEngine.Scripting.Preserve]
        public void QueryPurchases(string productType)
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
            _paymentManager.ConsumePurchase(purchaseToken);
        }

        /// <summary>
        /// 购买 一次性商品
        /// </summary>
        /// <param name="productId">产品ID或SKU</param>
        /// <param name="orderId">订单ID</param>
        /// <param name="offerToken">订阅优惠令牌，仅订阅商品需要</param>
        /// <param name="customData">自定义数据</param>
        [UnityEngine.Scripting.Preserve]
        public void BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")
        {
            _paymentManager.BuyInApp(productId, orderId, offerToken, customData);
        }

        /// <summary>
        /// 购买 订阅商品
        /// </summary>
        /// <param name="productId">产品ID或SKU</param>
        /// <param name="orderId">订单ID</param>
        /// <param name="offerToken">订阅优惠令牌，仅订阅商品需要</param>
        /// <param name="customData">自定义数据</param>
        [UnityEngine.Scripting.Preserve]
        public void BuySubs(string productId, string orderId, string offerToken = "", string customData = "")
        {
            _paymentManager.BuySubs(productId, orderId, offerToken, customData);
        }

        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="productId">产品ID或SKU</param>
        /// <param name="productType">产品类型，inapp/subs</param>
        /// <param name="offerToken">订阅优惠令牌，仅订阅商品需要</param>
        /// <param name="orderId">订单ID</param>
        /// <param name="customData">自定义数据</param>
        [UnityEngine.Scripting.Preserve]
        public void Buy(string productId, string productType, string offerToken, string orderId, string customData)
        {
            _paymentManager.Buy(productId, productType, offerToken, orderId, customData);
        }
    }
}