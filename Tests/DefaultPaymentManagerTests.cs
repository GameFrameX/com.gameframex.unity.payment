// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;
using System.Collections.Generic;
using GameFrameX.Payment.Runtime;
using NUnit.Framework;

namespace GameFrameX.Payment.Tests
{
    /// <summary>
    /// DefaultPaymentManager 门面行为测试
    /// </summary>
    /// <remarks>
    /// 覆盖两条路径：有注册渠道时转发/事件中继；无注册渠道时（Editor）退化为编辑器模拟。
    /// 模拟对话框通过注入 <see cref="DefaultPaymentManager.EditorDialogSelector"/> 固定选择，避免模态阻塞；
    /// 约定返回值：0=支付成功、1=取消支付、2=支付失败。
    /// </remarks>
    public class DefaultPaymentManagerTests
    {
        private sealed class TestPurchaseParams : PurchaseParams
        {
            public TestPurchaseParams(string productId = "product.test", string orderId = "order.test", string channelId = null)
                : base(productId, orderId, PaymentProductType.InApp)
            {
                ChannelId = channelId;
            }
        }

        /// <summary>
        /// 记录调用并可控回调的渠道管理器桩
        /// </summary>
        private sealed class RecordingChannelManager : IPaymentManager
        {
            public event EventHandler<PaymentEventArgs> OnPurchaseSuccess;
            public event EventHandler<PaymentEventArgs> OnPurchaseFailed;
            public event EventHandler<PaymentEventArgs> OnQueryPurchasesResult;
            public event EventHandler<PaymentEventArgs> OnConsumePurchaseResult;

            public int InitCallCount;
            public int BuyCallCount;
            public PurchaseParams LastBuyParams;
            public bool ReadyState = true;

            public void Init(bool isDebug = false, bool isClientVerify = false)
            {
                InitCallCount++;
            }

            public bool IsReady()
            {
                return ReadyState;
            }

            public void SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds) { }
            public void QueryPurchases(PaymentProductType productType) { }
            public void ConsumePurchase(string purchaseToken) { }
            public void BuyInApp(string productId, string orderId, string offerToken = "", string customData = "") { }
            public void BuySubs(string productId, string orderId, string offerToken = "", string customData = "") { }
            public void Buy(string productId, PaymentProductType productType, string orderId, string offerToken = "", string customData = "") { }

            public void Buy(PurchaseParams purchaseParams)
            {
                BuyCallCount++;
                LastBuyParams = purchaseParams;
            }

            public void RaiseSuccess(PaymentEventArgs e)
            {
                OnPurchaseSuccess?.Invoke(this, e);
            }
        }

        private DefaultPaymentManager _manager;
        private Func<string, string, int> _previousSelector;

        [SetUp]
        public void SetUp()
        {
            PaymentChannelRegistry.Reset();
            _manager = new DefaultPaymentManager();
            _previousSelector = DefaultPaymentManager.EditorDialogSelector;
        }

        [TearDown]
        public void TearDown()
        {
            DefaultPaymentManager.EditorDialogSelector = _previousSelector;
            PaymentChannelRegistry.Reset();
        }

        [Test]
        public void Init_WithoutChannel_EditorSimulationBecomesReady()
        {
            Assert.IsFalse(_manager.IsReady());
            _manager.Init();
            Assert.IsTrue(_manager.IsReady());
        }

        [Test]
        public void Init_WithRegisteredChannel_ForwardsToChannel()
        {
            var channel = new RecordingChannelManager();
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("stub", "存根渠道", () => channel));

            _manager.Init();

            Assert.AreEqual(1, channel.InitCallCount);
            Assert.IsTrue(_manager.IsReady());
        }

        [Test]
        public void Buy_WithoutChannel_DialogSuccess_RaisesPurchaseSuccessWithMockToken()
        {
            DefaultPaymentManager.EditorDialogSelector = (title, message) => 0;
            PaymentEventArgs received = null;
            _manager.OnPurchaseSuccess += (sender, e) => received = e;

            _manager.Buy(new TestPurchaseParams());

            Assert.NotNull(received);
            Assert.AreEqual("product.test", received.ProductId);
            Assert.AreEqual("order.test", received.OrderId);
            Assert.AreEqual(PaymentResultCode.Success, received.ResultCode);
            Assert.IsTrue(received.PurchaseToken.StartsWith("editor_mock_"), "模拟购买令牌应带 editor_mock_ 前缀，实际值：" + received.PurchaseToken);
            Assert.IsTrue(received.PurchaseData.Contains("\"simulated\":true"));
        }

        [Test]
        public void Buy_WithoutChannel_DialogCancel_RaisesPurchaseFailedWithCancelled()
        {
            DefaultPaymentManager.EditorDialogSelector = (title, message) => 1;
            PaymentEventArgs received = null;
            _manager.OnPurchaseFailed += (sender, e) => received = e;

            _manager.Buy(new TestPurchaseParams());

            Assert.NotNull(received);
            Assert.AreEqual(PaymentResultCode.Cancelled, received.ResultCode);
        }

        [Test]
        public void Buy_WithoutChannel_DialogFailure_RaisesPurchaseFailedWithFailed()
        {
            DefaultPaymentManager.EditorDialogSelector = (title, message) => 2;
            PaymentEventArgs received = null;
            _manager.OnPurchaseFailed += (sender, e) => received = e;

            _manager.Buy(new TestPurchaseParams());

            Assert.NotNull(received);
            Assert.AreEqual(PaymentResultCode.Failed, received.ResultCode);
            Assert.IsNotEmpty(received.ErrorMessage);
        }

        [Test]
        public void Buy_WithRegisteredChannel_ForwardsParamsAndRelaysEvents()
        {
            var channel = new RecordingChannelManager();
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("stub", "存根渠道", () => channel));
            var raised = 0;
            _manager.OnPurchaseSuccess += (sender, e) => raised++;

            var params_ = new TestPurchaseParams();
            _manager.Buy(params_);

            Assert.AreEqual(1, channel.BuyCallCount, "购买应转发到注册渠道");
            Assert.AreSame(params_, channel.LastBuyParams);

            channel.RaiseSuccess(new PaymentEventArgs { ResultCode = PaymentResultCode.Success });
            Assert.AreEqual(1, raised, "渠道事件应经门面中继后广播");
        }

        [Test]
        public void Buy_UnknownChannel_DoesNotRaiseAnyEvent()
        {
            DefaultPaymentManager.EditorDialogSelector = (title, message) =>
            {
                Assert.Fail("未知渠道不应弹出模拟对话框");
                return 0;
            };

            var raised = 0;
            _manager.OnPurchaseSuccess += (sender, e) => raised++;
            _manager.OnPurchaseFailed += (sender, e) => raised++;

            _manager.Buy(new TestPurchaseParams(channelId: "not-exists"));

            Assert.AreEqual(0, raised);
        }

        [Test]
        public void Buy_NullParams_DoesNotRaiseAnyEvent()
        {
            DefaultPaymentManager.EditorDialogSelector = (title, message) =>
            {
                Assert.Fail("空参数不应弹出模拟对话框");
                return 0;
            };

            var raised = 0;
            _manager.OnPurchaseSuccess += (sender, e) => raised++;
            _manager.OnPurchaseFailed += (sender, e) => raised++;

            _manager.Buy(null);

            Assert.AreEqual(0, raised);
        }

        [Test]
        public void ConsumePurchase_WithoutChannel_RaisesConsumeResultSuccessWithToken()
        {
            PaymentEventArgs received = null;
            _manager.OnConsumePurchaseResult += (sender, e) => received = e;

            _manager.ConsumePurchase("editor_mock_token_001");

            Assert.NotNull(received);
            Assert.AreEqual(PaymentResultCode.Success, received.ResultCode);
            Assert.AreEqual("editor_mock_token_001", received.PurchaseToken);
        }

        [Test]
        public void QueryPurchases_WithoutChannel_RaisesQueryResultSuccessWithEmptyList()
        {
            PaymentEventArgs received = null;
            _manager.OnQueryPurchasesResult += (sender, e) => received = e;

            _manager.QueryPurchases(PaymentProductType.InApp);

            Assert.NotNull(received);
            Assert.AreEqual(PaymentResultCode.Success, received.ResultCode);
            Assert.AreEqual("[]", received.PurchaseData);
        }
    }
}
