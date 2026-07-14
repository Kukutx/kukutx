# Overseas Chinese Community & Direct Recruitment Platform

面向海外华人的本地社区与可信直聘平台。当前分支为独立工程分支，不修改 GitHub Profile 的 `main` 分支。

## 当前范围

- Phase 0：产品边界、架构、权限、数据分类、威胁模型和 ADR。
- Phase 1：pnpm + Turborepo Monorepo、Expo、Next.js、ASP.NET Core、PostgreSQL/PostGIS、Redis、本地 Docker 和 CI 基础。
- Phase 2 首个切片：Firebase 身份验证、幂等用户建档与个人资料 API。

## 环境

- Node.js `24.18.0`
- pnpm `11.13.0`
- TypeScript `6.0.3`
- Expo SDK `56.0.5` / React Native `0.86.0`
- Next.js `16.2.9`
- .NET SDK `10.0.109` / .NET `10.0.9`
- PostgreSQL `18` + PostGIS `3.6`

## 常用命令

```bash
pnpm install
pnpm dev
pnpm check
pnpm db:migrate
pnpm api:generate
pnpm api:check-drift
```

本地基础设施：

```bash
docker compose up -d
```

项目事实与实施状态见 `docs/`。
