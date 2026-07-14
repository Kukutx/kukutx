import test from 'node:test';
import { access } from 'node:fs/promises';

for (const route of ['../app/page.tsx', '../app/login/page.tsx', '../app/profile/page.tsx']) {
  test(`route exists: ${route}`, async () => {
    await access(new URL(route, import.meta.url));
  });
}
