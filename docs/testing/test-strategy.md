# 测试策略

- Domain/Application：状态机、不变量、验证、幂等和并发单元测试。
- Infrastructure：PostgreSQL/PostGIS Testcontainers 集成测试，不使用 EF InMemory 替代数据库语义。
- API：认证、Problem Details、契约、资源级授权和安全回归。
- Web：组件、表单、可访问性、SEO 和 Playwright 业务闭环。
- Mobile：组件、路由、深链、认证、推送和核心 E2E。
- 性能：K6 覆盖公开读取、搜索、申请、消息、后台列表和 Webhook。
- CI：所有测试必须可重复，不依赖个人机器状态。
