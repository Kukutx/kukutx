import { PlatformApiClient } from '@platform/api-client';
import { auth } from './firebase';

export const platformApi = new PlatformApiClient({
  baseUrl: process.env.EXPO_PUBLIC_API_BASE_URL ?? 'http://localhost:5080',
  async getIdToken() {
    if (!auth.currentUser) throw new Error('当前没有已登录用户。');
    return auth.currentUser.getIdToken();
  },
});
