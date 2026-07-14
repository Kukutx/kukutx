import test from 'node:test';
import { access } from 'node:fs/promises';

test('mobile root route exists', async () => {
  await access(new URL('../app/index.tsx', import.meta.url));
});

test('secure Firebase persistence adapter exists', async () => {
  await access(new URL('../src/firebase.ts', import.meta.url));
});
