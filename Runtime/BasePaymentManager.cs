// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;
using System.Collections.Generic;
using GameFrameX.Runtime;

namespace GameFrameX.Payment.Runtime
{
    [UnityEngine.Scripting.Preserve]
    public abstract class BasePaymentManager : GameFrameworkModule, IPaymentManager
    {
        /// <summary>
        /// 购买成功事件
        /// </summary>
        public event EventHandler<PaymentEventArgs> OnPurchaseSuccess;

        /// <summary>
        /// 购买失败事件
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

        [UnityEngine.Scripting.Preserve]
        protected BasePaymentManager()
        {
        }

        protected void RaisePurchaseSuccess(PaymentEventArgs e)
        {
            OnPurchaseSuccess?.Invoke(this, e);
        }

        protected void RaisePurchaseFailed(PaymentEventArgs e)
        {
            OnPurchaseFailed?.Invoke(this, e);
        }

        protected void RaiseQueryPurchasesResult(PaymentEventArgs e)
        {
            OnQueryPurchasesResult?.Invoke(this, e);
        }

        protected void RaiseConsumePurchaseResult(PaymentEventArgs e)
        {
            OnConsumePurchaseResult?.Invoke(this, e);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="isDebug">是否是沙盒模式</param>
        /// <param name="isClientVerify">是否执行客户端验证购买成功，如果是强联网验证。不需要开启，设置为false，进行服务器验证</param>
        [UnityEngine.Scripting.Preserve]
        public abstract void Init(bool isDebug = false, bool isClientVerify = false);

        /// <summary>
        /// 支付系统是否准备好
        /// </summary>
        /// <returns>准备好返回true，否则返回false</returns>
        [UnityEngine.Scripting.Preserve]
        public abstract bool IsReady();

        /// <summary>
        /// 设置预加载的预定义商品ID,用于预加载缓存,注意：此方法必须在Initialize()之前调用
        /// </summary>
        /// <param name="inAppProductIds">内购商品ID列表</param>
        /// <param name="subsProductIds">订阅商品ID列表</param>
        [UnityEngine.Scripting.Preserve]
        public abstract void SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds);

        /// <summary>
        /// 查询购买记录
        /// </summary>
        /// <param name="productType">产品类型</param>
        [UnityEngine.Scripting.Preserve]
        public abstract void QueryPurchases(PaymentProductType productType);

        /// <summary>
        /// 消耗购买
        /// </summary>
        /// <param name="purchaseToken">购买令牌</param>
        [UnityEngine.Scripting.Preserve]
        public abstract void ConsumePurchase(string purchaseToken);

        /// <summary>
        /// 购买 一次性商品
        /// </summary>
        /// <param name="productId">产品ID或SKU</param>
        /// <param name="offerToken">订阅优惠令牌，仅订阅商品需要</param>
        /// <param name="orderId">订单ID</param>
        /// <param name="customData">自定义数据</param>
        [UnityEngine.Scripting.Preserve]
        public abstract void BuyInApp(string productId, string orderId, string offerToken = "", string customData = "");

        /// <summary>
        /// 购买 订阅商品
        /// </summary>
        /// <param name="productId">产品ID或SKU</param>
        /// <param name="offerToken">订阅优惠令牌，仅订阅商品需要</param>
        /// <param name="orderId">订单ID</param>
        /// <param name="customData">自定义数据</param>
        [UnityEngine.Scripting.Preserve]
        public abstract void BuySubs(string productId, string orderId, string offerToken = "", string customData = "");

        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="productId">产品ID或SKU</param>
        /// <param name="productType">产品类型</param>
        /// <param name="offerToken">订阅优惠令牌，仅订阅商品需要</param>
        /// <param name="orderId">订单ID</param>
        /// <param name="customData">自定义数据</param>
        [UnityEngine.Scripting.Preserve]
        public abstract void Buy(string productId, PaymentProductType productType, string orderId, string offerToken = "", string customData = "");

        protected override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        protected override void Shutdown()
        {
        }
    }
}