export type {
  ProblemDetails,
  UpdateUserProfileRequest,
  UserProfileDto,
} from './generated';

import type {
  ProblemDetails,
  UpdateUserProfileRequest,
  UserProfileDto,
} from './generated';

export class ApiProblem extends Error {
  constructor(
    public readonly status: number,
    public readonly problem: ProblemDetails,
  ) {
    super(problem.detail ?? problem.title ?? `HTTP ${status}`);
  }
}

export interface PlatformApiClientOptions {
  baseUrl: string;
  getIdToken: () => Promise<string>;
  fetchImpl?: typeof fetch;
}

export class PlatformApiClient {
  private readonly fetchImpl: typeof fetch;

  constructor(private readonly options: PlatformApiClientOptions) {
    this.fetchImpl = options.fetchImpl ?? fetch;
  }

  async getCurrentUser(): Promise<UserProfileDto> {
    const response = await this.request('/api/v1/me', { method: 'GET' });
    return this.readJson<UserProfileDto>(response);
  }

  async updateCurrentUser(
    profile: UpdateUserProfileRequest,
    version: number,
  ): Promise<UserProfileDto> {
    const response = await this.request('/api/v1/me', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'If-Match': `"${version}"`,
      },
      body: JSON.stringify(profile),
    });
    return this.readJson<UserProfileDto>(response);
  }

  private async request(path: string, init: RequestInit): Promise<Response> {
    const token = await this.options.getIdToken();
    const headers = new Headers(init.headers);
    headers.set('Authorization', `Bearer ${token}`);

    const response = await this.fetchImpl(
      new URL(path, this.options.baseUrl.endsWith('/') ? this.options.baseUrl : `${this.options.baseUrl}/`),
      { ...init, headers },
    );

    if (!response.ok) {
      let problem: ProblemDetails = { status: response.status };
      try {
        problem = (await response.json()) as ProblemDetails;
      } catch {
        problem.detail = response.statusText;
      }
      throw new ApiProblem(response.status, problem);
    }

    return response;
  }

  private async readJson<T>(response: Response): Promise<T> {
    return (await response.json()) as T;
  }
}
