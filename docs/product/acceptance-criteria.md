# 验收原则

每个功能必须同时满足：

1. 业务规则、权限、不变量和滥用场景明确。
2. 数据约束、索引、Migration、并发和幂等策略完整。
3. API 契约、稳定错误码、生成客户端和漂移检查完整。
4. 适用的 Web、Mobile 和 Admin 流程完整。
5. Loading、Empty、Error、Offline、Permission、RateLimit 和 Deleted 状态完整。
6. i18n、时区、无障碍和隐私处理完整。
7. 单元、真实 PostgreSQL 集成、安全和端到端测试通过。
8. 日志、Trace、Metrics、审计、告警和 Runbook 完整。
9. 不存在生产路径 TODO、Mock、假数据或临时权限绕过。
