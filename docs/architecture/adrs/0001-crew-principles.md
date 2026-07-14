# ADR-0001：复用 Crew 原则而非实现

状态：Accepted

## 决定

复用以下原则：

- Monorepo 统一工程入口。
- PostgreSQL/PostGIS 作为业务唯一真源。
- Firebase 仅承担认证、App Check、Remote Config、Analytics、Crashlytics 和 Messaging。
- ASP.NET API 作为业务规则和授权事实源。
- OpenAPI 契约、生成客户端和漂移检查。
- Dev、Staging、Production 严格隔离。
- 模块化单体、生产门禁和文档事实源。

拒绝直接复制：

- Flutter 实现；本项目使用 Expo/React Native。
- Crew 的历史兼容脚本和完整工具链。
- 未经重新评估的缓存、支付、后台任务和部署抽象。
- 与新业务边界不匹配的领域模型。

## 理由

Crew 已验证总体工程原则，但直接复制其历史复杂度会引入无关耦合和维护成本。
