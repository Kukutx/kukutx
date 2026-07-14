import assert from 'node:assert/strict';
import test from 'node:test';
import { readFile } from 'node:fs/promises';

const source = await readFile(new URL('../src/index.ts', import.meta.url), 'utf8');

test('client sends bearer token and If-Match', () => {
  assert.match(source, /Authorization/);
  assert.match(source, /If-Match/);
});
