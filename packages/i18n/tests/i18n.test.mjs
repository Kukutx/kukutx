import assert from 'node:assert/strict';
import test from 'node:test';
import { readFile } from 'node:fs/promises';

const source = await readFile(new URL('../src/index.ts', import.meta.url), 'utf8');

test('all required locales are defined', () => {
  assert.match(source, /'zh-Hans'/);
  assert.match(source, /'zh-Hant'/);
  assert.match(source, /\ben:/);
});
