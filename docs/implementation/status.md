# 实施状态

更新时间：2026-07-14

## 当前结论

当前仅完成 Phase 0、Phase 1 工程基础，以及 Phase 2 的首个 Identity/Profile 垂直切片。项目远未达到完整产品或上线状态，不得合并到 Profile `main`，也不得宣称已完成总控提示词中的全部业务模块。

## 已完成

- 在独立 GitHub 功能分支建立 pnpm + Turborepo Monorepo，不修改现有 Profile 主分支。
- 锁定 Node.js 24.18.0、pnpm 11.13.0、TypeScript 6.0.3、Expo SDK 56、React Native 0.86、Next.js 16.2.9、.NET 10.0.9、EF Core 10.0.9 和 PostgreSQL 18 基线。
- 完成产品边界、角色旅程、权限矩阵、状态机、数据分类、威胁模型、容器边界和核心 ADR。
- 建立 Expo、Next.js、ASP.NET Core、PostgreSQL/PostGIS、Redis、MinIO、Mailpit、本地 Docker Compose 和统一根命令。
- 完成 Firebase ID Token 验证、Token 撤销检查、取消令牌传播和业务认证主体映射。
- 完成基于 Firebase UID 的幂等用户建档、唯一索引、账户状态、资料读取/更新、IANA 时区与语言校验。
- 完成 PostgreSQL `xmin` 乐观并发、`If-Match` ETag、标准 Problem Details 和稳定错误码。
- 完成 EF Core Migration、OpenAPI 契约、TypeScript Client 生成及漂移检查。
- 完成 Web 邮箱登录和资料流程、Expo 登录和资料流程、SecureStore 认证持久化适配。
- 提交 `pnpm-lock.yaml`；CI 使用 `--frozen-lockfile`，依赖发布至少经过 1440 分钟观察期，安装脚本通过 `allowBuilds` 最小 Allowlist。
- 修复 Microsoft.OpenApi 高危漏洞并保持 ASP.NET Core 10.0.9 源生成器兼容，固定为 2.7.5。

## 真实验证

GitHub Actions CI Run `29319370187` 已在冻结锁文件模式下通过全部门禁：

- JavaScript 依赖安装：通过。
- NuGet Restore：通过，漏洞警告仍按错误处理。
- Docker Compose 配置验证：通过。
- OpenAPI Client 漂移检查：通过。
- Lint 与 .NET Analyzer/Format：通过。
- 全仓库 TypeScript Type Check：通过。
- 测试：10 项通过。
  - API Client：1 项。
  - i18n：1 项。
  - Web 路由：3 项。
  - Mobile 路由与安全持久化入口：2 项。
  - .NET Unit：2 项。
  - PostgreSQL Testcontainers Integration：1 项。
- 生产构建：通过。
  - Next.js Production Build。
  - Expo Android Export。
  - ASP.NET Core Release Build。

## 尚未完成

- Firebase Dev/Staging/Production 项目、真实凭据、App Check、FCM 和 Emulator E2E 尚未配置。
- 当前 Web/Mobile 测试仍是基础路由和代码入口测试，尚未达到总控提示词要求的真实浏览器、设备和完整认证 E2E。
- Identity 尚缺账号关联/解绑、MFA、设备会话、撤销、删除冷静期、导出、同意版本和安全通知。
- Geography、Organization、Verification、Jobs、Applications、Messaging、Community、Business、Events、Moderation、Admin、Billing 等业务模块尚未实施。
- IaC、部署、监控、告警、备份恢复、负载测试和上线门禁尚未实施。
- 当前代码暂存在 Profile 仓库的隔离分支，正式开发前应迁移到独立项目仓库。
