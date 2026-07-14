/* eslint-disable */
// 本文件由 tools/openapi/generate.mjs 生成，禁止手改。

export interface UserProfileDto {
  id: string;
  email?: string | null;
  displayName?: string | null;
  locale: "zh-Hans" | "zh-Hant" | "en";
  timeZoneId: string;
  cityId?: string | null;
  status: "Active" | "Restricted" | "Suspended" | "DeletionPending" | "Deleted";
  version: number;
  createdAt: string;
  updatedAt: string;
}

export interface UpdateUserProfileRequest {
  displayName?: string | null;
  locale: "zh-Hans" | "zh-Hant" | "en";
  timeZoneId: string;
  cityId?: string | null;
}

export interface ProblemDetails {
  type?: string | null;
  title?: string | null;
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  code?: string | null;
  traceId?: string | null;
  [key: string]: unknown;
}
