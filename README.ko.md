<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Payment

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.payment)](https://github.com/GameFrameX/com.gameframex.unity.payment/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.payment)](https://github.com/GameFrameX/com.gameframex.unity.payment/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#quick-start) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>

## 프로젝트 개요

**Payment 컴포넌트** - Unity 결제 패키지로, 인앱 구매 및 구독을 위한 통합 인터페이스를 제공하며 Google Play 및 Apple App Store를 지원합니다.

### 기능

- 인앱 구매 및 구독을 위한 통합 인터페이스
- Google Play 및 Apple App Store 지원
- 쉬운 통합 및 사용

## 빠른 시작

### 설치

Unity 프로젝트의 `Packages/manifest.json`을 편집하여 `scopedRegistries` 섹션을 추가하세요:

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

`scopes`는 이 레지스트리를 통해 어떤 패키지를 해석할지 제어합니다. `com.gameframex`로 시작하는 패키지만 이 레지스트리에서 가져옵니다.

Then add the package to `dependencies`:

```json
{
  "dependencies": {
    "com.gameframex.unity.payment": "1.1.0"
  }
}
```


## API 참조

### `Init(bool isDebug = false, bool isClientVerify = true)`

결제 매니저를 초기화합니다.

- `isDebug`: 샌드박스 모드 사용 여부.
- `isClientVerify`: 클라이언트 측 구매 검증 수행 여부.

### `IsReady()`

결제 시스템이 준비되었는지 확인합니다.

### `SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds)`

사전 로드를 위한 미리 정의된 상품 ID를 설정합니다.

- `inAppProductIds`: 인앱 상품 ID 목록.
- `subsProductIds`: 구독 상품 ID 목록.

### `QueryPurchases(string productType)`

구매 내역을 조회합니다.

- `productType`: 상품 유형. "inapp" 또는 "subs".

### `ConsumePurchase(string purchaseToken)`

구매를 소비합니다.

- `purchaseToken`: 구매 토큰.

### `BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")`

인앱 구매를 시작합니다.

- `productId`: 상품 ID 또는 SKU.
- `orderId`: 주문 ID.
- `offerToken`: 구독 오퍼 토큰 (구독 전용).
- `customData`: 사용자 정의 데이터.

### `BuySubs(string productId, string orderId, string offerToken = "", string customData = "")`

구독 구매를 시작합니다.

- `productId`: 상품 ID 또는 SKU.
- `orderId`: 주문 ID.
- `offerToken`: 구독 오퍼 토큰.
- `customData`: 사용자 정의 데이터.

### `Buy(string productId, string productType, string offerToken, string orderId, string customData)`

구매를 시작합니다.

- `productId`: 상품 ID 또는 SKU.
- `productType`: 상품 유형. "inapp" 또는 "subs".
- `offerToken`: 구독 오퍼 토큰.
- `orderId`: 주문 ID.
- `customData`: 사용자 정의 데이터.

## 플랫폼 지원

| 플랫폼 | 지원 |
|--------|------|
| Android (Google Play) | 예 |
| iOS (App Store) | 예 |

## 문서 및 자료

- [문서](https://gameframex.doc.alianblank.com)
- [GitHub 리포지토리](https://github.com/GameFrameX/com.gameframex.unity.payment)

## 커뮤니티 및 지원

- QQ 그룹: [QR 코드](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6)에서 가입

## 라이선스

이 프로젝트는 [LICENSE](LICENSE) 파일에 정의된 조건에 따라 라이선스가 부여됩니다.
