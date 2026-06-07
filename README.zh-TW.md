<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Payment

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.payment)](https://github.com/GameFrameX/com.gameframex.unity.payment/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.payment)](https://github.com/GameFrameX/com.gameframex.unity.payment/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#quick-start) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 項目簡介

**Payment 支付組件** - Unity 支付套件，為應用內購買和訂閱提供統一的介面，支援 Google Play 和 Apple App Store。

### 功能特性

- 應用內購買和訂閱的統一介面
- 支援 Google Play 和 Apple App Store
- 易於整合和使用

## 快速開始

### 安裝

選擇以下任一方式：

1. 編輯 Unity 專案的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：
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

   `scopes` 控制哪些套件透過此註冊表解析。只有以 `com.gameframex` 開頭的套件才會從這個註冊表取得。

2. 直接在 `manifest.json` 的 `dependencies` 節點下添加以下內容：
   ```json
   {
      "com.gameframex.unity.payment": "https://github.com/gameframex/com.gameframex.unity.payment.git"
   }
   ```
3. 在 Unity 的 `Package Manager` 中使用 `Git URL` 的方式添加庫，地址為：`https://github.com/gameframex/com.gameframex.unity.payment.git`
4. 直接下載倉庫放置到 Unity 專案的 `Packages` 目錄下，會自動載入識別。
## API 參考

### `Init(bool isDebug = false, bool isClientVerify = true)`

初始化支付管理器。

- `isDebug`：是否使用沙盒模式。
- `isClientVerify`：是否執行客戶端購買驗證。

### `IsReady()`

檢查支付系統是否準備就緒。

### `SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds)`

設定預定義的商品 ID 用於預載入。

- `inAppProductIds`：應用內商品 ID 清單。
- `subsProductIds`：訂閱商品 ID 清單。

### `QueryPurchases(string productType)`

查詢購買歷史。

- `productType`：商品類型，"inapp" 或 "subs"。

### `ConsumePurchase(string purchaseToken)`

消耗購買。

- `purchaseToken`：購買令牌。

### `BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")`

發起應用內購買。

- `productId`：商品 ID 或 SKU。
- `orderId`：訂單 ID。
- `offerToken`：訂閱優惠令牌（僅限訂閱）。
- `customData`：自訂資料。

### `BuySubs(string productId, string orderId, string offerToken = "", string customData = "")`

發起訂閱購買。

- `productId`：商品 ID 或 SKU。
- `orderId`：訂單 ID。
- `offerToken`：訂閱優惠令牌。
- `customData`：自訂資料。

### `Buy(string productId, string productType, string offerToken, string orderId, string customData)`

發起購買。

- `productId`：商品 ID 或 SKU。
- `productType`：商品類型，"inapp" 或 "subs"。
- `offerToken`：訂閱優惠令牌。
- `orderId`：訂單 ID。
- `customData`：自訂資料。

## 平台支援

| 平台 | 支援 |
|------|------|
| Android (Google Play) | 是 |
| iOS (App Store) | 是 |

## 文檔與資源

- [文檔](https://gameframex.doc.alianblank.com)
- [GitHub 倉庫](https://github.com/GameFrameX/com.gameframex.unity.payment)

## 社區與支援

- QQ群：透過 [二維碼](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6) 加入


## 依賴

| 套件 | 說明 |
|------|------|
| `com.gameframex.unity` | 1.1.1 |

## 更新日誌

查看 [Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.payment/releases) 了解更新日誌。
## 開源協議

本專案基於 [LICENSE](LICENSE) 文件中定義的條款授權。
