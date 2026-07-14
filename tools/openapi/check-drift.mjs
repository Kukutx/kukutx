import { readFile } from 'node:fs/promises';
import { fileURLToPath } from 'node:url';
import path from 'node:path';
import { renderTypes } from './render.mjs';

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '../..');
const sourcePath = path.join(root, 'openapi/platform.v1.json');
const outputPath = path.join(root, 'packages/api-client/src/generated.ts');
const document = JSON.parse(await readFile(sourcePath, 'utf8'));
const expected = renderTypes(document);
const actual = await readFile(outputPath, 'utf8');

if (actual !== expected) {
  console.error('OpenAPI 生成文件已漂移。请运行 pnpm api:generate。');
  process.exitCode = 1;
} else {
  console.log('OpenAPI 生成文件无漂移。');
}
