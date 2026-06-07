<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Payment

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.payment)](https://github.com/GameFrameX/com.gameframex.unity.payment/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.payment)](https://github.com/GameFrameX/com.gameframex.unity.payment/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#quick-start) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>

## プロジェクト概要

**Payment コンポーネント** - Unity 支払いパッケージ。アプリ内課金とサブスクリプションのための統一インターフェースを提供し、Google Play と Apple App Store をサポートします。

### 機能

- アプリ内課金とサブスクリプションの統一インターフェース
- Google Play と Apple App Store のサポート
- 簡単な統合と使用

## クイックスタート

### インストール

Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：

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
  ]
}
```

`scopes` は、どのパッケージをこのレジストリから解決するかを制御します。`com.gameframex` で始まるパッケージのみがこのレジストリから取得されます。

Then add the package to `dependencies`:

```json
{
  "dependencies": {
    "com.gameframex.unity.payment": "1.1.0"
  }
}
```


## API リファレンス

### `Init(bool isDebug = false, bool isClientVerify = true)`

支払いマネージャーを初期化します。

- `isDebug`：サンドボックスモードを使用するかどうか。
- `isClientVerify`：クライアント側の購入検証を実行するかどうか。

### `IsReady()`

支払いシステムが準備完了かどうかを確認します。

### `SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds)`

プリロード用の定義済み製品 ID を設定します。

- `inAppProductIds`：アプリ内製品 ID のリスト。
- `subsProductIds`：サブスクリプション製品 ID のリスト。

### `QueryPurchases(string productType)`

購入履歴を照会します。

- `productType`：製品タイプ。"inapp" または "subs"。

### `ConsumePurchase(string purchaseToken)`

購入を消費します。

- `purchaseToken`：購入トークン。

### `BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")`

アプリ内購入を開始します。

- `productId`：製品 ID または SKU。
- `orderId`：注文 ID。
- `offerToken`：サブスクリプションオファートークン（サブスクリプションのみ）。
- `customData`：カスタムデータ。

### `BuySubs(string productId, string orderId, string offerToken = "", string customData = "")`

サブスクリプション購入を開始します。

- `productId`：製品 ID または SKU。
- `orderId`：注文 ID。
- `offerToken`：サブスクリプションオファートークン。
- `customData`：カスタムデータ。

### `Buy(string productId, string productType, string offerToken, string orderId, string customData)`

購入を開始します。

- `productId`：製品 ID または SKU。
- `productType`：製品タイプ。"inapp" または "subs"。
- `offerToken`：サブスクリプションオファートークン。
- `orderId`：注文 ID。
- `customData`：カスタムデータ。

## プラットフォーム対応

| プラットフォーム | 対応 |
|-----------------|------|
| Android (Google Play) | はい |
| iOS (App Store) | はい |

## ドキュメントとリソース

- [ドキュメント](https://gameframex.doc.alianblank.com)
- [GitHub リポジトリ](https://github.com/GameFrameX/com.gameframex.unity.payment)

## コミュニティとサポート

- QQグループ：[QRコード](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6)から参加


## 依存関係

| パッケージ | 説明 |
|----------|------|
| `com.gameframex.unity` | 1.1.1 |

## 変更履歴

[Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.payment/releases) で変更履歴を確認してください。
## ライセンス

このプロジェクトは [LICENSE](LICENSE) ファイルに定義された条件に基づいてライセンスされています。
