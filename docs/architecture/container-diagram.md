# 容器边界

- `apps/mobile`：Expo Router 移动应用，只消费生成 API Client。
- `apps/web`：Next.js App Router，承载公开 SEO、账户、企业和管理后台。
- `backend/api`：模块化单体，按 API、Application、Domain、Infrastructure 分层。
- `packages/api-client`：由 OpenAPI 生成，禁止手改生成文件。
- `packages/i18n`：跨端消息键和语言资源。
- `packages/shared`：纯 TypeScript 工具、枚举和格式化函数。
- PostgreSQL：业务数据、Outbox、审计和幂等记录。
- Redis：SignalR backplane、限流、短期缓存和必要的分布式协调，不作为业务真源。
