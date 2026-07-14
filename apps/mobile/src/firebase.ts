import * as SecureStore from 'expo-secure-store';
import { getApp, getApps, initializeApp } from 'firebase/app';
import {
  getAuth,
  initializeAuth,
  type Auth,
  type Persistence,
} from 'firebase/auth';

const secureStorePersistence = {
  type: 'LOCAL',
  async _isAvailable() {
    return SecureStore.isAvailableAsync();
  },
  async _set(key: string, value: string) {
    await SecureStore.setItemAsync(key, value);
  },
  async _get(key: string) {
    return SecureStore.getItemAsync(key);
  },
  async _remove(key: string) {
    await SecureStore.deleteItemAsync(key);
  },
  _addListener() {
    // SecureStore 不提供跨上下文事件；移动端由 Firebase Auth 内部状态事件驱动。
  },
  _removeListener() {
    // 与 _addListener 对称保留。
  },
} as unknown as Persistence;

const firebaseConfig = {
  apiKey: process.env.EXPO_PUBLIC_FIREBASE_API_KEY ?? 'missing',
  authDomain: process.env.EXPO_PUBLIC_FIREBASE_AUTH_DOMAIN ?? 'missing',
  projectId: process.env.EXPO_PUBLIC_FIREBASE_PROJECT_ID ?? 'missing',
};

const app = getApps().length > 0 ? getApp() : initializeApp(firebaseConfig);
let auth: Auth;

try {
  auth = initializeAuth(app, { persistence: secureStorePersistence });
} catch {
  auth = getAuth(app);
}

export { auth };
