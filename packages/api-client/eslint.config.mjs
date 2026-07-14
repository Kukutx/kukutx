import js from '@eslint/js';

export default [
  js.configs.recommended,
  {
    files: ['src/**/*.ts'],
    languageOptions: {
      parserOptions: { sourceType: 'module' },
    },
  },
  { ignores: ['src/generated.ts'] },
];
