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
    /// <summary>
    /// 默认支付管理器，主包模块槽位的默认实现（渠道调用经 PaymentChannelRegistry 转发）
    /// </summary>
    /// <remarks>
    /// 模块级解析轴的落点：<c>IPaymentManager</c> 的模块注册收敛到本主包类型，渠道实现不再各自直接占用模块槽位
    /// （与广告域 <c>AdvertisementManager</c> 同构）。实际的渠道调用经 <see cref="PaymentChannelRegistry.ResolveManager"/> 转发：
    /// 未指定渠道时按注册顺序取首个注册渠道；无注册渠道时，Editor 下退化为编辑器模拟（弹窗选择支付结果并回调真实事件），
    /// 真机下告警（每次会话只告警一次）。渠道管理器的事件经本门面中继后再对外广播。
    /// </remarks>
    [UnityEngine.Scripting.Preserve]
    public sealed class DefaultPaymentManager : BasePaymentManager
    {
#if UNITY_EDITOR
        /// <summary>
        /// 模拟购买令牌前缀
        /// </summary>
        private const string MockTokenPrefix = "editor_mock_";

        /// <summary>
        /// 编辑器模拟对话框选择器，入参为 (标题, 提示内容)，返回 0=支付成功、1=取消支付、2=支付失败。
        /// 默认弹出 <see cref="UnityEditor.EditorUtility.DisplayDialogComplex"/>；测试可注入委托以避免模态阻塞。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static Func<string, string, int> EditorDialogSelector { get; set; } = ShowDefaultDialog;

        private bool _editorReady;

        private static int ShowDefaultDialog(string title, string message)
        {
            return UnityEditor.EditorUtility.DisplayDialogComplex(title, message, "支付成功", "取消支付", "支付失败");
        }

        private void SimulatePurchase(string productId, string orderId)
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

            var selector = EditorDialogSelector ?? ShowDefaultDialog;
            int choice = selector("编辑器模拟支付", "商品：" + productId + "\n订单：" + orderId + "\n\n请选择模拟的支付结果：");
            var eventArgs = new PaymentEventArgs
            {
                ProductId = productId,
                OrderId = orderId,
                PurchaseToken = MockTokenPrefix + Guid.NewGuid().ToString("N"),
            };

            switch (choice)
            {
                case 0: // 支付成功
                    eventArgs.ResultCode = PaymentResultCode.Success;
                    eventArgs.PurchaseData = Utility.Text.Format("{{\"productId\":\"{0}\",\"orderId\":\"{1}\",\"purchaseToken\":\"{2}\",\"simulated\":true}}", productId, orderId, eventArgs.PurchaseToken);
                    RaisePurchaseSuccess(eventArgs);
                    break;

                case 1: // 取消支付
                    eventArgs.ResultCode = PaymentResultCode.Cancelled;
                    eventArgs.ErrorMessage = "Payment cancelled in editor simulation.";
                    RaisePurchaseFailed(eventArgs);
                    break;

                default: // 支付失败
                    eventArgs.ResultCode = PaymentResultCode.Failed;
                    eventArgs.ErrorMessage = "Payment failed in editor simulation.";
                    RaisePurchaseFailed(eventArgs);
                    break;
            }
        }

        private void SimulateQueryPurchases()
        {
            // 编辑器模拟没有历史购买记录，返回空列表（与渠道实现的 JSON 数组契约一致）
            RaiseQueryPurchasesResult(new PaymentEventArgs
            {
                ResultCode = PaymentResultCode.Success,
                PurchaseData = "[]",
            });
        }

        private void SimulateConsumePurchase(string purchaseToken)
        {
            RaiseConsumePurchaseResult(new PaymentEventArgs
            {
                PurchaseToken = purchaseToken,
                ResultCode = PaymentResultCode.Success,
            });
        }
#endif

        private bool _noChannelWarned;
        private readonly HashSet<IPaymentManager> _wiredChannels = new HashSet<IPaymentManager>();

        [UnityEngine.Scripting.Preserve]
        public DefaultPaymentManager()
        {
        }

        /// <summary>
        /// 解析渠道管理器并接线事件中继；无可用渠道时告警（每次会话只告警一次）并返回 null
        /// </summary>
        /// <param name="channelId">渠道唯一标识，为空时按注册顺序取首个</param>
        /// <returns>渠道管理器；无可用渠道返回null</returns>
        private IPaymentManager ResolveChannel(string channelId)
        {
            var manager = PaymentChannelRegistry.ResolveManager(channelId);
            if (manager == null)
            {
                if (!_noChannelWarned)
                {
                    _noChannelWarned = true;
                    Log.Warning("No payment channel registered. Install a channel package (google / apple / alipay ...) or register one via PaymentChannelRegistry.");
                }

                return null;
            }

            if (_wiredChannels.Add(manager))
            {
                manager.OnPurchaseSuccess += (sender, e) => RaisePurchaseSuccess(e);
                manager.OnPurchaseFailed += (sender, e) => RaisePurchaseFailed(e);
                manager.OnQueryPurchasesResult += (sender, e) => RaiseQueryPurchasesResult(e);
                manager.OnConsumePurchaseResult += (sender, e) => RaiseConsumePurchaseResult(e);
            }

            return manager;
        }

        [UnityEngine.Scripting.Preserve]
        public override void Init(bool isDebug = false, bool isClientVerify = false)
        {
            var manager = ResolveChannel(null);
            if (manager != null)
            {
                manager.Init(isDebug, isClientVerify);
                return;
            }

#if UNITY_EDITOR
            _editorReady = true;
            Log.Info("[DefaultPaymentManager] 无注册支付渠道，编辑器支付模拟已启用：Buy 将弹出结果选择对话框并回调支付事件。");
#else
            Log.Warning("You are using DefaultPaymentManager which is a stub. Please integrate a channel-specific implementation (e.g. Google, Apple).");
#endif
        }

        [UnityEngine.Scripting.Preserve]
        public override bool IsReady()
        {
            var manager = ResolveChannel(null);
            if (manager != null)
            {
                return manager.IsReady();
            }

#if UNITY_EDITOR
            return _editorReady;
#else
            return false;
#endif
        }

        [UnityEngine.Scripting.Preserve]
        public override void QueryPurchases(PaymentProductType productType)
        {
            var manager = ResolveChannel(null);
            if (manager != null)
            {
                manager.QueryPurchases(productType);
                return;
            }

#if UNITY_EDITOR
            SimulateQueryPurchases();
#else
            Log.Warning("You are using DefaultPaymentManager which is a stub. Please integrate a channel-specific implementation (e.g. Google, Apple).");
#endif
        }

        [UnityEngine.Scripting.Preserve]
        public override void ConsumePurchase(string purchaseToken)
        {
            if (string.IsNullOrEmpty(purchaseToken))
            {
                Log.Warning("Purchase token is null or empty.");
                return;
            }

            var manager = ResolveChannel(null);
            if (manager != null)
            {
                manager.ConsumePurchase(purchaseToken);
                return;
            }

#if UNITY_EDITOR
            SimulateConsumePurchase(purchaseToken);
#else
            Log.Warning("You are using DefaultPaymentManager which is a stub. Please integrate a channel-specific implementation (e.g. Google, Apple).");
#endif
        }

        [UnityEngine.Scripting.Preserve]
        [Obsolete("请使用 Buy(PurchaseParams) 替代")]
        public override void BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")
        {
#pragma warning disable CS0618
            var manager = ResolveChannel(null);
            if (manager != null)
            {
                manager.BuyInApp(productId, orderId, offerToken, customData);
                return;
            }
#pragma warning restore CS0618

#if UNITY_EDITOR
            SimulatePurchase(productId, orderId);
#else
            Log.Warning("You are using DefaultPaymentManager which is a stub. Please integrate a channel-specific implementation (e.g. Google, Apple).");
#endif
        }

        [UnityEngine.Scripting.Preserve]
        [Obsolete("请使用 Buy(PurchaseParams) 替代")]
        public override void BuySubs(string productId, string orderId, string offerToken = "", string customData = "")
        {
#pragma warning disable CS0618
            var manager = ResolveChannel(null);
            if (manager != null)
            {
                manager.BuySubs(productId, orderId, offerToken, customData);
                return;
            }
#pragma warning restore CS0618

#if UNITY_EDITOR
            SimulatePurchase(productId, orderId);
#else
            Log.Warning("You are using DefaultPaymentManager which is a stub. Please integrate a channel-specific implementation (e.g. Google, Apple).");
#endif
        }

        [UnityEngine.Scripting.Preserve]
        [Obsolete("请使用 Buy(PurchaseParams) 替代")]
        public override void Buy(string productId, PaymentProductType productType, string orderId, string offerToken = "", string customData = "")
        {
#pragma warning disable CS0618
            var manager = ResolveChannel(null);
            if (manager != null)
            {
                manager.Buy(productId, productType, orderId, offerToken, customData);
                return;
            }
#pragma warning restore CS0618

#if UNITY_EDITOR
            SimulatePurchase(productId, orderId);
#else
            Log.Warning("You are using DefaultPaymentManager which is a stub. Please integrate a channel-specific implementation (e.g. Google, Apple).");
#endif
        }

        [UnityEngine.Scripting.Preserve]
        public override void Buy(PurchaseParams purchaseParams)
        {
            if (purchaseParams == null)
            {
                Log.Warning("Purchase params is null.");
                return;
            }

            var manager = ResolveChannel(purchaseParams.ChannelId);
            if (manager != null)
            {
                manager.Buy(purchaseParams);
                return;
            }

            if (!string.IsNullOrEmpty(purchaseParams.ChannelId))
            {
                Log.Warning("Payment channel '{0}' is not registered.", purchaseParams.ChannelId);
                return;
            }

#if UNITY_EDITOR
            SimulatePurchase(purchaseParams.ProductId, purchaseParams.OrderId);
#else
            Log.Warning("You are using DefaultPaymentManager which is a stub. Please integrate a channel-specific implementation (e.g. Google, Apple).");
#endif
        }

        [UnityEngine.Scripting.Preserve]
        public override void SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds)
        {
            var manager = ResolveChannel(null);
            if (manager != null)
            {
                manager.SetPredefinedProductIds(inAppProductIds, subsProductIds);
            }

            // 无可用渠道时无需预加载商品缓存：Editor 模拟直接接受，真机已告警
        }
    }
}
