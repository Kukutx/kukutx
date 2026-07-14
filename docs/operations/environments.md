# 环境隔离

Local、Dev、Staging、Production 使用独立数据库、Firebase 项目、Bucket、密钥、域名和监控。生产凭据不得进入非生产环境。

本地通过 Docker Compose 提供 PostgreSQL/PostGIS、Redis、MinIO 和 Mailpit。Firebase Auth 使用独立开发项目或 Emulator；生产启动在缺少关键配置时必须失败。
