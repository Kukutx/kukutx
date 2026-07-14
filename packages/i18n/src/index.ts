export type Locale = 'zh-Hans' | 'zh-Hant' | 'en';

const messages = {
  'zh-Hans': {
    appName: '邻里直聘',
    signIn: '登录',
    email: '邮箱',
    password: '密码',
    profile: '个人资料',
    displayName: '昵称',
    locale: '语言',
    timeZone: '时区',
    save: '保存',
    loading: '正在加载…',
    signOut: '退出登录',
    retry: '重试',
  },
  'zh-Hant': {
    appName: '鄰里直聘',
    signIn: '登入',
    email: '電郵',
    password: '密碼',
    profile: '個人資料',
    displayName: '暱稱',
    locale: '語言',
    timeZone: '時區',
    save: '儲存',
    loading: '載入中…',
    signOut: '登出',
    retry: '重試',
  },
  en: {
    appName: 'NeighbourHire',
    signIn: 'Sign in',
    email: 'Email',
    password: 'Password',
    profile: 'Profile',
    displayName: 'Display name',
    locale: 'Language',
    timeZone: 'Time zone',
    save: 'Save',
    loading: 'Loading…',
    signOut: 'Sign out',
    retry: 'Retry',
  },
} as const;

export type MessageKey = keyof (typeof messages)['zh-Hans'];

export function t(locale: Locale, key: MessageKey): string {
  return messages[locale][key];
}
