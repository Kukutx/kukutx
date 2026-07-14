import * as SecureStore from 'expo-secure-store';
import { getApp, getApps, initializeApp } from 'firebase/app';
import {
  getAuth,
  initializeAuth,
  type Auth,
  type Persistence,
} from 'firebase/auth';

const secureStorePersistence: Persistence = {
  type: 'LOCAL',
  async _isAvailable() {
    return SecureStore.isAvailableAsync();
  },
  async _set(key, value) {
    await SecureStore.setItemAsync(key, value);
  },
  async _get(key) {
    return SecureStore.getItemAsync(key);
  },
  async _remove(key) {
    await SecureStore.deleteItemAsync(key);
  },
};

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
