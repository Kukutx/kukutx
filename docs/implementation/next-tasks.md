# 下一自动任务

1. 将当前功能分支迁移到独立项目仓库，保留完整提交历史和 CI 证据；继续禁止合并到 Profile `main`。
2. 加固 Identity/Profile 切片：认证 Handler 集成测试、Suspended/Deleted 状态拦截、Firebase Emulator E2E、账号关联与最后登录方式不变量。
3. 实现 Geography/Reference Data：国家、行政区、城市、IANA 时区、货币和基础 PostGIS 查询，为用户城市资料提供真实外键。
4. 完成数据审计、Outbox、IdempotencyRecord、Feature Flag 和通知基础设施。
5. 开始 Organization/Membership/Invitation/Permission/Verification 的完整垂直切片，包括管理审核入口和 PostgreSQL 集成测试。
6. 扩展 CI：Secret Scan、依赖许可证、CodeQL/SAST、SBOM、容器扫描、Migration 空库/升级验证和 Playwright/Expo E2E。
