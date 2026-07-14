# 组件边界

## 后端

- Domain：聚合、值对象、状态机和不变量，不依赖基础设施。
- Application：用例、端口接口、事务语义和授权需求。
- Infrastructure：EF Core、Firebase、对象存储、消息和外部 Provider Adapter。
- API：认证、路由、Problem Details、版本化和请求/响应边界。

## 规则

- 模块间同步调用通过明确接口。
- 异步副作用通过事务 Outbox。
- DTO 不直接暴露 EF Entity。
- 关键权限由 API 强制执行，前端保护只改善体验。
- 生成契约和客户端只能从唯一源更新。
