function refName(reference) {
  return reference.split('/').at(-1);
}

function renderType(schema) {
  if (schema.$ref) return refName(schema.$ref);
  if (schema.enum) return schema.enum.map((value) => JSON.stringify(value)).join(' | ');
  if (schema.type === 'array') return `Array<${renderType(schema.items ?? {})}>`;
  if (schema.type === 'integer' || schema.type === 'number') return 'number';
  if (schema.type === 'boolean') return 'boolean';
  if (schema.type === 'object') return 'Record<string, unknown>';
  return 'string';
}

export function renderTypes(document) {
  const schemas = document.components?.schemas ?? {};
  const chunks = [
    '/* eslint-disable */',
    '// 本文件由 tools/openapi/generate.mjs 生成，禁止手改。',
    '',
  ];

  for (const [name, schema] of Object.entries(schemas)) {
    if (schema.enum) {
      chunks.push(`export type ${name} = ${renderType(schema)};`, '');
      continue;
    }

    const required = new Set(schema.required ?? []);
    chunks.push(`export interface ${name} {`);
    for (const [propertyName, propertySchema] of Object.entries(schema.properties ?? {})) {
      const optional = required.has(propertyName) ? '' : '?';
      const nullable = propertySchema.nullable ? ' | null' : '';
      chunks.push(`  ${propertyName}${optional}: ${renderType(propertySchema)}${nullable};`);
    }
    if (schema.additionalProperties) {
      chunks.push('  [key: string]: unknown;');
    }
    chunks.push('}', '');
  }

  return `${chunks.join('\n').trim()}\n`;
}
