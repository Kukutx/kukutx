#!/usr/bin/env bash
set -o pipefail

label="$1"
shift
log_file="$(mktemp)"

"$@" 2>&1 | tee "$log_file"
status=${PIPESTATUS[0]}

if [[ $status -ne 0 && -n "${GH_TOKEN:-}" && -n "${PR_NUMBER:-}" ]]; then
  comment_file="$(mktemp)"
  {
    printf '### CI failure: %s\n\n' "$label"
    printf '```text\n'
    tail -n 80 "$log_file"
    printf '\n```\n'
  } > "$comment_file"
  gh pr comment "$PR_NUMBER" --body-file "$comment_file" || true
fi

exit "$status"
