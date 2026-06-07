<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Payment

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.payment)](https://github.com/GameFrameX/com.gameframex.unity.payment/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.payment)](https://github.com/GameFrameX/com.gameframex.unity.payment/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#quick-start) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 项目简介

**Payment 支付组件** - Unity 支付包，为应用内购买和订阅提供统一的接口，支持 Google Play 和 Apple App Store。

### 功能特性

- 应用内购买和订阅的统一接口
- 支持 Google Play 和 Apple App Store
- 易于集成和使用

## 快速开始

### 安装

选择以下任一方式：

1. 编辑 Unity 项目的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.payment": "1.1.0"
     }
   }
   ```

   `scopes` 控制哪些包通过此注册表解析。只有以 `com.gameframex` 开头的包才会从这个注册表获取。

2. 直接在 `manifest.json` 的 `dependencies` 节点下添加以下内容：
   ```json
   {
      "com.gameframex.unity.payment": "https://github.com/gameframex/com.gameframex.unity.payment.git"
   }
   ```
3. 在 Unity 的 `Package Manager` 中使用 `Git URL` 的方式添加库，地址为：`https://github.com/gameframex/com.gameframex.unity.payment.git`
4. 直接下载仓库放置到 Unity 项目的 `Packages` 目录下，会自动加载识别。
## API 参考

### `Init(bool isDebug = false, bool isClientVerify = true)`

初始化支付管理器。

- `isDebug`：是否使用沙盒模式。
- `isClientVerify`：是否执行客户端购买验证。

### `IsReady()`

检查支付系统是否准备就绪。

### `SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds)`

设置预定义的商品 ID 用于预加载。

- `inAppProductIds`：应用内商品 ID 列表。
- `subsProductIds`：订阅商品 ID 列表。

### `QueryPurchases(string productType)`

查询购买历史。

- `productType`：商品类型，"inapp" 或 "subs"。

### `ConsumePurchase(string purchaseToken)`

消耗购买。

- `purchaseToken`：购买令牌。

### `BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")`

发起应用内购买。

- `productId`：商品 ID 或 SKU。
- `orderId`：订单 ID。
- `offerToken`：订阅优惠令牌（仅限订阅）。
- `customData`：自定义数据。

### `BuySubs(string productId, string orderId, string offerToken = "", string customData = "")`

发起订阅购买。

- `productId`：商品 ID 或 SKU。
- `orderId`：订单 ID。
- `offerToken`：订阅优惠令牌。
- `customData`：自定义数据。

### `Buy(string productId, string productType, string offerToken, string orderId, string customData)`

发起购买。

- `productId`：商品 ID 或 SKU。
- `productType`：商品类型，"inapp" 或 "subs"。
- `offerToken`：订阅优惠令牌。
- `orderId`：订单 ID。
- `customData`：自定义数据。

## 平台支持

| 平台 | 支持 |
|------|------|
| Android (Google Play) | 是 |
| iOS (App Store) | 是 |

## 文档与资源

- [文档](https://gameframex.doc.alianblank.com)
- [GitHub 仓库](https://github.com/GameFrameX/com.gameframex.unity.payment)

## 社区与支持

- QQ群：通过 [二维码](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6) 加入


## 依赖

| 包 | 说明 |
|----|------|
| `com.gameframex.unity` | 1.1.1 |

## 更新日志

查看 [Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.payment/releases) 了解更新日志。
## 开源协议

详见 [LICENSE.md](LICENSE.md) 文件。
