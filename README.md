# GameFrameX.Payment

GameFrameX.Payment 是一个 Unity 支付包，为应用内购买和订阅提供统一的接口，支持 Google Play 和 Apple App Store。

## 特性

- 应用内购买和订阅的统一接口。
- 支持 Google Play 和 Apple App Store。
- 易于集成和使用。

## 安装

要使用此包，请将以下行添加到您的 `manifest.json` 文件中：

```json
{
  "dependencies": {
    "com.gameframex.unity.payment": "https://github.com/GameFrameX/com.gameframex.unity.payment.git"
  }
}
```

## 如何使用

1.  将 `PaymentComponent` 添加到场景中的 GameObject。
2.  获取 `PaymentComponent` 实例并初始化它。
3.  使用 `BuyInApp` 或 `BuySubs` 方法发起购买。

### 示例

```csharp
using GameFrameX.Payment.Runtime;
using UnityEngine;

public class PaymentExample : MonoBehaviour
{
    private PaymentComponent _paymentComponent;

    void Start()
    {
        _paymentComponent = FindObjectOfType<PaymentComponent>();
        _paymentComponent.Init();
    }

    public void BuyProduct(string productId)
    {
        _paymentComponent.BuyInApp(productId, "orderId");
    }
}
```

## API

### `Init(bool isDebug = false, bool isClientVerify = true)`

初始化支付管理器。

-   `isDebug`: 是否使用沙盒模式。
-   `isClientVerify`: 是否执行客户端购买验证。

### `IsReady()`

检查支付系统是否准备就绪。

### `SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds)`

设置预定义的商品 ID 用于预加载。

-   `inAppProductIds`: 应用内商品 ID 列表。
-   `subsProductIds`: 订阅商品 ID 列表。

### `QueryPurchases(string productType)`

查询购买历史。

-   `productType`: 商品类型，"inapp" 或 "subs"。

### `ConsumePurchase(string purchaseToken)`

消耗购买。

-   `purchaseToken`: 购买令牌。

### `BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")`

发起应用内购买。

-   `productId`: 商品 ID 或 SKU。
-   `orderId`: 订单 ID。
-   `offerToken`: 订阅优惠令牌（仅限订阅）。
-   `customData`: 自定义数据。

### `BuySubs(string productId, string orderId, string offerToken = "", string customData = "")`

发起订阅购买。

-   `productId`: 商品 ID 或 SKU。
-   `orderId`: 订单 ID。
-   `offerToken`: 订阅优惠令牌。
-   `customData`: 自定义数据。

### `Buy(string productId, string productType, string offerToken, string orderId, string customData)`

发起购买。

-   `productId`: 商品 ID 或 SKU。
-   `productType`: 商品类型，"inapp" 或 "subs"。
-   `offerToken`: 订阅优惠令牌。
-   `orderId`: 订单 ID。
-   `customData`: 自定义数据。

## 许可证

该项目根据 MIT 许可证授权 - 有关详细信息，请参阅 [LICENSE.md](LICENSE.md) 文件。
