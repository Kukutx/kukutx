import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 1,
  duration: '10s',
  thresholds: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<500'],
  },
};

export default function () {
  const response = http.get(`${__ENV.API_BASE_URL ?? 'http://localhost:5080'}/health/live`);
  check(response, { 'health is 200': (result) => result.status === 200 });
}
