'use client';

import { t } from '@platform/i18n';
import { signInWithEmailAndPassword } from 'firebase/auth';
import { useRouter } from 'next/navigation';
import { FormEvent, useState } from 'react';
import { getFirebaseAuth } from '@/lib/firebase';

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [pending, setPending] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const locale = 'zh-Hans' as const;

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setPending(true);
    setError(null);
    try {
      await signInWithEmailAndPassword(getFirebaseAuth(), email, password);
      router.replace('/profile');
    } catch (exception) {
      setError(exception instanceof Error ? exception.message : '登录失败。');
    } finally {
      setPending(false);
    }
  }

  return (
    <main className="shell">
      <form className="card stack" onSubmit={submit}>
        <h1>{t(locale, 'signIn')}</h1>
        <label className="field">
          <span>{t(locale, 'email')}</span>
          <input
            autoComplete="email"
            inputMode="email"
            required
            type="email"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
          />
        </label>
        <label className="field">
          <span>{t(locale, 'password')}</span>
          <input
            autoComplete="current-password"
            minLength={8}
            required
            type="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
          />
        </label>
        {error ? <p className="error" role="alert">{error}</p> : null}
        <button className="button" disabled={pending} type="submit">
          {pending ? t(locale, 'loading') : t(locale, 'signIn')}
        </button>
      </form>
    </main>
  );
}
