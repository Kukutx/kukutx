import type { UserProfileDto } from '@platform/api-client';
import { t, type Locale } from '@platform/i18n';
import { onAuthStateChanged, signInWithEmailAndPassword, signOut } from 'firebase/auth';
import { useEffect, useState } from 'react';
import {
  ActivityIndicator,
  Pressable,
  SafeAreaView,
  ScrollView,
  StyleSheet,
  Text,
  TextInput,
  View,
} from 'react-native';
import { auth } from '@/src/firebase';
import { platformApi } from '@/src/platform-api';

export default function HomeScreen() {
  const [profile, setProfile] = useState<UserProfileDto | null>(null);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [displayName, setDisplayName] = useState('');
  const [locale, setLocale] = useState<Locale>('zh-Hans');
  const [timeZoneId, setTimeZoneId] = useState('Europe/Rome');
  const [busy, setBusy] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => onAuthStateChanged(auth, async (user) => {
    if (!user) {
      setProfile(null);
      setBusy(false);
      return;
    }

    try {
      const current = await platformApi.getCurrentUser();
      setProfile(current);
      setDisplayName(current.displayName ?? '');
      setLocale(current.locale);
      setTimeZoneId(current.timeZoneId);
    } catch (exception) {
      setError(exception instanceof Error ? exception.message : '资料加载失败。');
    } finally {
      setBusy(false);
    }
  }), []);

  async function login() {
    setBusy(true);
    setError(null);
    try {
      await signInWithEmailAndPassword(auth, email, password);
    } catch (exception) {
      setError(exception instanceof Error ? exception.message : '登录失败。');
      setBusy(false);
    }
  }

  async function save() {
    if (!profile) return;
    setBusy(true);
    setError(null);
    try {
      const updated = await platformApi.updateCurrentUser(
        { displayName: displayName || null, locale, timeZoneId, cityId: profile.cityId },
        profile.version,
      );
      setProfile(updated);
    } catch (exception) {
      setError(exception instanceof Error ? exception.message : '保存失败。');
    } finally {
      setBusy(false);
    }
  }

  if (busy && !profile) {
    return <SafeAreaView style={styles.center}><ActivityIndicator accessibilityLabel={t(locale, 'loading')} /></SafeAreaView>;
  }

  return (
    <SafeAreaView style={styles.safeArea}>
      <ScrollView contentContainerStyle={styles.container} keyboardShouldPersistTaps="handled">
        <View style={styles.card}>
          <Text accessibilityRole="header" style={styles.title}>{t(locale, 'appName')}</Text>
          {profile ? (
            <>
              <Text style={styles.heading}>{t(locale, 'profile')}</Text>
              <Text style={styles.label}>{t(locale, 'displayName')}</Text>
              <TextInput
                accessibilityLabel={t(locale, 'displayName')}
                maxLength={80}
                onChangeText={setDisplayName}
                style={styles.input}
                value={displayName}
              />
              <Text style={styles.label}>{t(locale, 'locale')}</Text>
              <View style={styles.row}>
                {(['zh-Hans', 'zh-Hant', 'en'] as const).map((candidate) => (
                  <Pressable
                    accessibilityRole="button"
                    key={candidate}
                    onPress={() => setLocale(candidate)}
                    style={[styles.choice, locale === candidate && styles.choiceSelected]}
                  >
                    <Text>{candidate}</Text>
                  </Pressable>
                ))}
              </View>
              <Text style={styles.label}>{t(locale, 'timeZone')}</Text>
              <TextInput
                accessibilityLabel={t(locale, 'timeZone')}
                autoCapitalize="none"
                onChangeText={setTimeZoneId}
                style={styles.input}
                value={timeZoneId}
              />
              <Pressable accessibilityRole="button" disabled={busy} onPress={save} style={styles.button}>
                <Text style={styles.buttonText}>{busy ? t(locale, 'loading') : t(locale, 'save')}</Text>
              </Pressable>
              <Pressable accessibilityRole="button" onPress={() => signOut(auth)} style={styles.secondaryButton}>
                <Text>{t(locale, 'signOut')}</Text>
              </Pressable>
            </>
          ) : (
            <>
              <Text style={styles.label}>{t(locale, 'email')}</Text>
              <TextInput
                accessibilityLabel={t(locale, 'email')}
                autoCapitalize="none"
                autoComplete="email"
                keyboardType="email-address"
                onChangeText={setEmail}
                style={styles.input}
                value={email}
              />
              <Text style={styles.label}>{t(locale, 'password')}</Text>
              <TextInput
                accessibilityLabel={t(locale, 'password')}
                autoComplete="current-password"
                onChangeText={setPassword}
                secureTextEntry
                style={styles.input}
                value={password}
              />
              <Pressable accessibilityRole="button" disabled={busy} onPress={login} style={styles.button}>
                <Text style={styles.buttonText}>{t(locale, 'signIn')}</Text>
              </Pressable>
            </>
          )}
          {error ? <Text accessibilityRole="alert" style={styles.error}>{error}</Text> : null}
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safeArea: { flex: 1, backgroundColor: '#f3f5f4' },
  center: { flex: 1, alignItems: 'center', justifyContent: 'center' },
  container: { flexGrow: 1, justifyContent: 'center', padding: 20 },
  card: { backgroundColor: '#fff', borderRadius: 18, padding: 22, gap: 12 },
  title: { fontSize: 28, fontWeight: '800', color: '#176b55' },
  heading: { fontSize: 20, fontWeight: '700' },
  label: { fontWeight: '600', marginTop: 4 },
  input: { borderWidth: 1, borderColor: '#d7ddd9', borderRadius: 10, padding: 12 },
  row: { flexDirection: 'row', flexWrap: 'wrap', gap: 8 },
  choice: { paddingHorizontal: 10, paddingVertical: 8, borderWidth: 1, borderColor: '#d7ddd9', borderRadius: 9 },
  choiceSelected: { backgroundColor: '#dcefe8', borderColor: '#176b55' },
  button: { backgroundColor: '#176b55', borderRadius: 10, padding: 13, alignItems: 'center', marginTop: 8 },
  buttonText: { color: '#fff', fontWeight: '700' },
  secondaryButton: { backgroundColor: '#e8eeeb', borderRadius: 10, padding: 13, alignItems: 'center' },
  error: { color: '#b42318' },
});
