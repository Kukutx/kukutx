# 依赖与安装脚本策略

- 所有直接依赖使用精确版本并提交 `pnpm-lock.yaml`。
- pnpm 安装脚本默认禁止执行；仅在 `pnpm-workspace.yaml` 的 `allowBuilds` 中把经过审阅的包标记为 `true`。
- 当前批准项：`@firebase/util`、`protobufjs`、`sharp`、`unrs-resolver`，分别由 Firebase、协议序列化、Next.js 图片处理和模块解析链使用。
- 新增批准项必须说明调用链、维护状态、许可证和替代方案，并经过 CI 构建验证。
- CI 不允许通过全局放开生命周期脚本来绕过供应链门禁。
