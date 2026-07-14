import assert from 'node:assert/strict';
import test from 'node:test';
import { access } from 'node:fs/promises';

test('mobile root route exists', async () => {
  await assert.doesNotReject(access(new URL('../app/index.tsx', import.meta.url)));
});

test('secure Firebase persistence adapter exists', async () => {
  await assert.doesNotReject(access(new URL('../src/firebase.ts', import.meta.url)));
});
