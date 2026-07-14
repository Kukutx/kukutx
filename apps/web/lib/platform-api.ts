import { PlatformApiClient } from '@platform/api-client';
import { getFirebaseAuth } from './firebase';

export function createPlatformApiClient() {
  return new PlatformApiClient({
    baseUrl: process.env.NEXT_PUBLIC_API_BASE_URL ?? 'http://localhost:5080',
    async getIdToken() {
      const user = getFirebaseAuth().currentUser;
      if (!user) throw new Error('当前没有已登录用户。');
      return user.getIdToken();
    },
  });
}
