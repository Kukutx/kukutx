# ADR-0002：2026-07 技术版本基线

状态：Accepted

## 锁定版本

- Node.js `24.13.2`
- pnpm `11.7.0`
- TypeScript `6.0.3`
- Turborepo `2.10.0`
- Expo SDK `56.0.5`
- React Native `0.86.0`
- React `19.2.x`
- Next.js `16.2.9`
- .NET SDK `10.0.109`，运行时与 EF Core `10.0.9`
- Firebase Admin .NET `3.5.0`
- Npgsql EF Provider `10.0.2`
- PostgreSQL `18` + PostGIS `3.6`

## 依据

版本通过 GitHub 中的官方 Expo、.NET 发布资料和 Crew 当前生产依赖交叉核验。禁止使用 canary、RC 或 beta。依赖升级必须经过兼容性、许可证、Migration 和回归测试。
