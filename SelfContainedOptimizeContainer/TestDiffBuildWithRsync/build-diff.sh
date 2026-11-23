#!/usr/bin/env bash
set -euo pipefail
set -x

IN_BASE_APP_DIR="${1:?expected base app dir path as 1st argument}"
IN_FULL_DESIRED_APP_DIR="${2:?expected desired app dir path as 2nd argument}"
OUT_DIFF_DIR="${3:?expected out dir path as 3rd argument}"

rm -rf "$OUT_DIFF_DIR" && mkdir "$OUT_DIFF_DIR" 
# problem with this approach: does not delete obsolete files.
# TBD if this is a problem.
rsync -rci --delete \
    --compare-dest="$IN_BASE_APP_DIR"/ \
    "$IN_FULL_DESIRED_APP_DIR"/ \
    "$OUT_DIFF_DIR"/ 
ls -l "$OUT_DIFF_DIR"
diff "$IN_BASE_APP_DIR" "$OUT_DIFF_DIR" || : has difference