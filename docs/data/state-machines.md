# 状态机

- OrganizationVerification：Draft → Submitted → UnderReview → NeedsInformation → Approved / Rejected → Suspended。
- Job：Draft → PendingReview → Published → Paused / Closed / Expired；PendingReview → Rejected；Rejected → Draft。
- Application：Submitted → Viewed → Shortlisted → Interview → Offered → Hired；活动状态可转 Rejected 或 Withdrawn。
- ModerationCase：Open → Triaged → InReview → Actioned / NoAction → Appealed → Resolved。
- MediaAsset：PendingUpload → Uploaded → Scanning → Ready / Rejected / Failed → Deleted。
- Subscription：Trialing → Active → PastDue → Canceled → Expired。
- UserAccount：Active → Restricted / Suspended / DeletionPending → Deleted。

状态转换必须由领域代码、数据库约束、API 校验和测试共同保证。
