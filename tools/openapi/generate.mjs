import { readFile, writeFile, mkdir } from 'node:fs/promises';
import { fileURLToPath } from 'node:url';
import path from 'node:path';
import { renderTypes } from './render.mjs';

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '../..');
const sourcePath = path.join(root, 'openapi/platform.v1.json');
const outputPath = path.join(root, 'packages/api-client/src/generated.ts');
const document = JSON.parse(await readFile(sourcePath, 'utf8'));

await mkdir(path.dirname(outputPath), { recursive: true });
await writeFile(outputPath, renderTypes(document), 'utf8');
console.log(`Generated ${path.relative(root, outputPath)}`);
