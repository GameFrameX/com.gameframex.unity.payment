// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任。

using System.Collections.Generic;
using GameFrameX.Payment.Runtime;
using NUnit.Framework;

namespace GameFrameX.Payment.Tests
{
    public class PaymentChannelRegistryTests
    {
        private class StubChannelManager : IPaymentManager
        {
            public event System.EventHandler<PaymentEventArgs> OnPurchaseSuccess;
            public event System.EventHandler<PaymentEventArgs> OnPurchaseFailed;
            public event System.EventHandler<PaymentEventArgs> OnQueryPurchasesResult;
            public event System.EventHandler<PaymentEventArgs> OnConsumePurchaseResult;

            public int FactoryCallCount;

            public void Init(bool isDebug = false, bool isClientVerify = false) { }
            public bool IsReady() { return true; }
            public void SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds) { }
            public void QueryPurchases(PaymentProductType productType) { }
            public void ConsumePurchase(string purchaseToken) { }
            public void BuyInApp(string productId, string orderId, string offerToken = "", string customData = "") { }
            public void BuySubs(string productId, string orderId, string offerToken = "", string customData = "") { }
            public void Buy(string productId, PaymentProductType productType, string orderId, string offerToken = "", string customData = "") { }
            public void Buy(PurchaseParams purchaseParams) { }
        }

        private sealed class OtherChannelManager : StubChannelManager
        {
        }

        private sealed class TestPurchaseParams : PurchaseParams
        {
            public TestPurchaseParams(string channelId = null) : base("product", "order", PaymentProductType.InApp)
            {
                ChannelId = channelId;
            }
        }

        [SetUp]
        public void SetUp()
        {
            PaymentChannelRegistry.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            PaymentChannelRegistry.Reset();
        }

        [Test]
        public void Register_ThenGetManager_CreatesInstanceLazilyAndCaches()
        {
            var factoryCallCount = 0;
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("stub", "存根渠道", () =>
            {
                factoryCallCount++;
                return new StubChannelManager();
            }));

            Assert.IsTrue(PaymentChannelRegistry.Contains("stub"));
            Assert.AreEqual(0, factoryCallCount, "工厂在首次获取前不应被调用");

            var first = PaymentChannelRegistry.GetManager("stub");
            var second = PaymentChannelRegistry.GetManager("stub");

            Assert.IsNotNull(first);
            Assert.IsInstanceOf<StubChannelManager>(first);
            Assert.AreSame(first, second, "同一渠道应复用实例");
            Assert.AreEqual(1, factoryCallCount);
        }

        [Test]
        public void Register_DuplicateChannelId_KeepsFirstRegistration()
        {
            var first = new StubChannelManager();
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("stub", "第一个", () => first));
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("stub", "第二个", () => new OtherChannelManager()));

            Assert.AreSame(first, PaymentChannelRegistry.GetManager("stub"), "重复注册应保留首个");
        }

        [Test]
        public void GetManager_UnknownChannel_ReturnsNull()
        {
            Assert.IsNull(PaymentChannelRegistry.GetManager("not-exists"));
            Assert.IsFalse(PaymentChannelRegistry.Contains("not-exists"));
        }

        [Test]
        public void Register_NullArguments_AreIgnoredOrThrown()
        {
            PaymentChannelRegistry.Register(null);
            Assert.IsFalse(PaymentChannelRegistry.Contains("stub"));

            Assert.Throws<System.ArgumentNullException>(() => new PaymentChannelRegistration(null, "无效", () => new StubChannelManager()));
            Assert.Throws<System.ArgumentNullException>(() => new PaymentChannelRegistration("stub", "无效", null));
        }

        [Test]
        public void Unregister_RemovesChannelAndCachedInstance()
        {
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("stub", "存根渠道", () => new StubChannelManager()));
            var manager = PaymentChannelRegistry.GetManager("stub");
            Assert.IsNotNull(manager);

            PaymentChannelRegistry.Unregister("stub");
            Assert.IsFalse(PaymentChannelRegistry.Contains("stub"));
            Assert.IsNull(PaymentChannelRegistry.GetManager("stub"));

            PaymentChannelRegistry.Register(new PaymentChannelRegistration("stub", "存根渠道", () => new StubChannelManager()));
            Assert.IsInstanceOf<StubChannelManager>(PaymentChannelRegistry.GetManager("stub"), "重新注册后可再次获取");
        }

        [Test]
        public void GetAllChannelInfos_ReturnsRegistrationWithAvailability()
        {
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("stub", "存根渠道", () => new StubChannelManager()));

            var infos = PaymentChannelRegistry.GetAllChannelInfos();

            Assert.AreEqual(1, infos.Count);
            Assert.AreEqual("stub", infos[0].ChannelId);
            Assert.AreEqual("存根渠道", infos[0].DisplayName);
            Assert.IsTrue(infos[0].IsAvailable, "StubChannelManager.IsReady 恒为 true");
        }

        [Test]
        public void ResolveManager_NoRegistration_ReturnsNull()
        {
            Assert.IsNull(PaymentChannelRegistry.ResolveManager());
            Assert.IsNull(PaymentChannelRegistry.ResolveManager(null));
            Assert.IsNull(PaymentChannelRegistry.ResolveManager(""));
        }

        [Test]
        public void ResolveManager_EmptyChannel_ReturnsFirstRegistered()
        {
            var first = new StubChannelManager();
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("first", "第一个", () => first));
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("other", "其他渠道", () => new OtherChannelManager()));

            Assert.AreSame(first, PaymentChannelRegistry.ResolveManager());
        }

        [Test]
        public void ResolveManager_ExplicitChannel_ReturnsChannelManager()
        {
            var first = new StubChannelManager();
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("first", "第一个", () => first));
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("other", "其他渠道", () => new OtherChannelManager()));

            var resolved = PaymentChannelRegistry.ResolveManager("other");

            Assert.IsInstanceOf<OtherChannelManager>(resolved);
            Assert.AreNotSame(first, resolved);
        }

        [Test]
        public void ResolveManager_UnknownChannel_ReturnsNull()
        {
            Assert.IsNull(PaymentChannelRegistry.ResolveManager("not-exists"));
        }

        [Test]
        public void PurchaseParams_ChannelId_RoutesViaSubclassBinding()
        {
            var first = new StubChannelManager();
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("first", "第一个", () => first));
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("other", "其他渠道", () => new OtherChannelManager()));

            var routedParams = new TestPurchaseParams("other");
            Assert.IsInstanceOf<OtherChannelManager>(PaymentChannelRegistry.ResolveManager(routedParams.ChannelId));

            var defaultParams = new TestPurchaseParams(null);
            Assert.AreSame(first, PaymentChannelRegistry.ResolveManager(defaultParams.ChannelId));
        }

        [Test]
        public void Reset_ClearsAllRegistrations()
        {
            PaymentChannelRegistry.Register(new PaymentChannelRegistration("stub", "存根渠道", () => new StubChannelManager()));
            PaymentChannelRegistry.Reset();

            Assert.IsFalse(PaymentChannelRegistry.Contains("stub"));
            Assert.IsNull(PaymentChannelRegistry.GetManager("stub"));
        }

        [Test]
        public void AlipayBootstrap_PlatformGuard_DoesNotRegisterInEditor()
        {
            // Editor 下渠道不注册（DefaultPaymentManager 的编辑器模拟不被原生桥渠道抢占默认槽位）；
            // 未安装 alipay 渠道包时类型不存在，同样不注册
            var bootstrapType = System.Type.GetType("GameFrameX.Payment.Alipay.Runtime.AlipayPaymentBootstrap, GameFrameX.Payment.Alipay.Runtime");
            if (bootstrapType != null)
            {
                var registerMethod = bootstrapType.GetMethod("Register", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                registerMethod.Invoke(null, null);
            }

            Assert.IsFalse(PaymentChannelRegistry.Contains("alipay"), "Editor 下支付宝渠道不应注册");
        }
    }
}
