'use client';

import type { UserProfileDto } from '@platform/api-client';
import { t, type Locale } from '@platform/i18n';
import { onAuthStateChanged, signOut } from 'firebase/auth';
import { useRouter } from 'next/navigation';
import { FormEvent, useEffect, useState } from 'react';
import { getFirebaseAuth } from '@/lib/firebase';
import { createPlatformApiClient } from '@/lib/platform-api';

export default function ProfilePage() {
  const router = useRouter();
  const [profile, setProfile] = useState<UserProfileDto | null>(null);
  const [displayName, setDisplayName] = useState('');
  const [locale, setLocale] = useState<Locale>('zh-Hans');
  const [timeZoneId, setTimeZoneId] = useState('Europe/Rome');
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const unsubscribe = onAuthStateChanged(getFirebaseAuth(), async (user) => {
      if (!user) {
        router.replace('/login');
        return;
      }

      try {
        const current = await createPlatformApiClient().getCurrentUser();
        setProfile(current);
        setDisplayName(current.displayName ?? '');
        setLocale(current.locale);
        setTimeZoneId(current.timeZoneId);
      } catch (exception) {
        setError(exception instanceof Error ? exception.message : '资料加载失败。');
      } finally {
        setLoading(false);
      }
    });
    return unsubscribe;
  }, [router]);

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!profile) return;
    setSaving(true);
    setError(null);
    try {
      const updated = await createPlatformApiClient().updateCurrentUser(
        { displayName: displayName || null, locale, timeZoneId, cityId: profile.cityId },
        profile.version,
      );
      setProfile(updated);
    } catch (exception) {
      setError(exception instanceof Error ? exception.message : '保存失败。');
    } finally {
      setSaving(false);
    }
  }

  if (loading) {
    return <main className="shell"><p>{t(locale, 'loading')}</p></main>;
  }

  return (
    <main className="shell">
      <form className="card stack" onSubmit={submit}>
        <div className="row">
          <h1>{t(locale, 'profile')}</h1>
          <button
            className="button secondary"
            type="button"
            onClick={async () => {
              await signOut(getFirebaseAuth());
              router.replace('/login');
            }}
          >
            {t(locale, 'signOut')}
          </button>
        </div>
        <label className="field">
          <span>{t(locale, 'displayName')}</span>
          <input maxLength={80} value={displayName} onChange={(event) => setDisplayName(event.target.value)} />
        </label>
        <label className="field">
          <span>{t(locale, 'locale')}</span>
          <select value={locale} onChange={(event) => setLocale(event.target.value as Locale)}>
            <option value="zh-Hans">简体中文</option>
            <option value="zh-Hant">繁體中文</option>
            <option value="en">English</option>
          </select>
        </label>
        <label className="field">
          <span>{t(locale, 'timeZone')}</span>
          <input required value={timeZoneId} onChange={(event) => setTimeZoneId(event.target.value)} />
        </label>
        {error ? <p className="error" role="alert">{error}</p> : null}
        <button className="button" disabled={saving || !profile} type="submit">
          {saving ? t(locale, 'loading') : t(locale, 'save')}
        </button>
      </form>
    </main>
  );
}
