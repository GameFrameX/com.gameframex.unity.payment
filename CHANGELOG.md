# [1.1.0](https://github.com/gameframex/com.gameframex.unity.payment/compare/1.0.0...1.1.0) (2026-04-21)


### Bug Fixes

* **payment:** DefaultPaymentManager 所有方法添加未实现警告日志 ([0abf730](https://github.com/gameframex/com.gameframex.unity.payment/commit/0abf730c29891096cfd69dc4f1206693d60e7692))
* **payment:** DefaultPaymentManager.IsReady() 返回 false ([90a8022](https://github.com/gameframex/com.gameframex.unity.payment/commit/90a8022bcf86642e9585d3f3cb754fe6fdbda330))
* **payment:** 修复 Buy() 方法参数顺序与接口不一致 ([aad2e9a](https://github.com/gameframex/com.gameframex.unity.payment/commit/aad2e9a1975a51481655556392b757dc9f91f3fc))
* **payment:** 将 macOS 从 PC 平台独立为单独的支付配置 ([233ad58](https://github.com/gameframex/com.gameframex.unity.payment/commit/233ad5874e0f37efab24d44fb591bebadb60327e))
* **payment:** 添加 RequireComponent 确保 CroppingHelper 自动挂载 ([b139261](https://github.com/gameframex/com.gameframex.unity.payment/commit/b139261e76b71fdd0fc83f22f282c69f44a0d787))
* **payment:** 添加关键方法的输入参数验证 ([acfcdf9](https://github.com/gameframex/com.gameframex.unity.payment/commit/acfcdf9db4be0aa33eca19b76380cd9c4c43f103))
* **payment:** 添加非移动平台的降级策略 ([6ad5e80](https://github.com/gameframex/com.gameframex.unity.payment/commit/6ad5e803a6bde6576519b5fed7c6e6b6661612a1))
* **payment:** 补充 CroppingHelper 新增类型的防裁剪引用 ([3d11bf6](https://github.com/gameframex/com.gameframex.unity.payment/commit/3d11bf64e97adea4a579001f19af432a7a46eca4))


### Features

* **payment:** 添加 PurchaseParams 购买参数基类 ([2fec55f](https://github.com/gameframex/com.gameframex.unity.payment/commit/2fec55f3fbab6e2bfb0f8ae165b2d0ecb4963fbb))
* **payment:** 添加 WebGL 和 PC 平台支付组件配置 ([702efd1](https://github.com/gameframex/com.gameframex.unity.payment/commit/702efd1dd25a64f6158acc766bd3fa8fad54b0de))
* **payment:** 添加基于 PurchaseParams 的购买方法，旧方法标记过期 ([9a02df3](https://github.com/gameframex/com.gameframex.unity.payment/commit/9a02df3db2c4c3cbb4a0a46447df436675ab2983))
* **payment:** 添加支付事件回调系统 ([4011efb](https://github.com/gameframex/com.gameframex.unity.payment/commit/4011efb9507c4dc353dd98e4da7d6ea6f6af8f91))


### Reverts

* **payment:** 回滚 link.xml 改用属性标记防裁剪 ([2cc7ee2](https://github.com/gameframex/com.gameframex.unity.payment/commit/2cc7ee2980193ee47e5d9b855057032402e4ee1b))

# 1.0.0 (2025-12-23)


### Features

* 添加支付模块基础实现 ([2275064](https://github.com/gameframex/com.gameframex.unity.payment/commit/2275064ba3d0014f080ae053a7fab48f8ddd0b16))
