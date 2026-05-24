<p align="center">
  <img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="GameFrameX Logo" width="160" />
</p>

<h1 align="center">Game Frame X Payment</h1>

<p align="center">
  <a href="https://github.com/GameFrameX/com.gameframex.unity.payment/releases">
    <img src="https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.payment" alt="Version" />
  </a>
  <a href="https://github.com/GameFrameX/com.gameframex.unity.payment/blob/main/LICENSE">
    <img src="https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.payment" alt="License" />
  </a>
  <a href="https://gameframex.doc.alianblank.com">
    <img src="https://img.shields.io/badge/Documentation-online-blue" alt="Documentation" />
  </a>
</p>

<p align="center">
  All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams
</p>

<p align="center">
  <a href="https://gameframex.doc.alianblank.com">Documentation</a> ·
  <a href="#quick-start">Quick Start</a> ·
  <a href="https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6">QQ Group</a> ·
  Language: <strong>English</strong> | <a href="README.zh-CN.md">简体中文</a> | <a href="README.zh-TW.md">繁體中文</a> | <a href="README.ja.md">日本語</a> | <a href="README.ko.md">한국어</a>
</p>

---

## Project Overview

**Payment Component** - A Unity payment package that provides a unified interface for in-app purchases and subscriptions, supporting Google Play and Apple App Store.

### Features

- Unified interface for in-app purchases and subscriptions
- Support for Google Play and Apple App Store
- Easy to integrate and use

## Quick Start

### Installation

Add the following to the `dependencies` section of your project's `manifest.json`:

```json
{
  "dependencies": {
    "com.gameframex.unity.payment": "https://github.com/GameFrameX/com.gameframex.unity.payment.git"
  }
}
```

Alternatively, use `Git URL` in Unity's Package Manager or download and place in the `Packages` directory.

### Usage Examples

1. Add `PaymentComponent` to a GameObject in your scene.
2. Get the `PaymentComponent` instance and initialize it.
3. Use `BuyInApp` or `BuySubs` methods to initiate purchases.

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

## API Reference

### `Init(bool isDebug = false, bool isClientVerify = true)`

Initialize the payment manager.

- `isDebug`: Whether to use sandbox mode.
- `isClientVerify`: Whether to perform client-side purchase verification.

### `IsReady()`

Check if the payment system is ready.

### `SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds)`

Set predefined product IDs for preloading.

- `inAppProductIds`: List of in-app product IDs.
- `subsProductIds`: List of subscription product IDs.

### `QueryPurchases(string productType)`

Query purchase history.

- `productType`: Product type, "inapp" or "subs".

### `ConsumePurchase(string purchaseToken)`

Consume a purchase.

- `purchaseToken`: The purchase token.

### `BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")`

Initiate an in-app purchase.

- `productId`: Product ID or SKU.
- `orderId`: Order ID.
- `offerToken`: Subscription offer token (subscriptions only).
- `customData`: Custom data.

### `BuySubs(string productId, string orderId, string offerToken = "", string customData = "")`

Initiate a subscription purchase.

- `productId`: Product ID or SKU.
- `orderId`: Order ID.
- `offerToken`: Subscription offer token.
- `customData`: Custom data.

### `Buy(string productId, string productType, string offerToken, string orderId, string customData)`

Initiate a purchase.

- `productId`: Product ID or SKU.
- `productType`: Product type, "inapp" or "subs".
- `offerToken`: Subscription offer token.
- `orderId`: Order ID.
- `customData`: Custom data.

## Platform Support

| Platform | Supported |
|----------|-----------|
| Android (Google Play) | Yes |
| iOS (App Store) | Yes |

## Documentation & Resources

- [Documentation](https://gameframex.doc.alianblank.com)
- [GitHub Repository](https://github.com/GameFrameX/com.gameframex.unity.payment)

## Community & Support

- QQ Group: Join via [QR Code](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6)

## License

This project is licensed under the terms of the [LICENSE](LICENSE) file.
