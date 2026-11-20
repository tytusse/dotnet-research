#!/usr/bin/env bash
set -euo pipefail
set -x

# see if self-contained unique artifacts match regular ones
# spoiler alert - they do not.
# only "exe" files are same, but I will guess they only refer to dlls that are in turn different

SCRIPT_DIR=$(realpath $(dirname $0))
cd "$SCRIPT_DIR"
APP="$SCRIPT_DIR/TheApp"
CONTAINED_TRUE="/tmp/test-contained-true"
CONTAINED_FALSE="/tmp/test-contained-false"
CONTAINED_TRUE_2="/tmp/test-contained-true-2"
rm -rf "$CONTAINED_FALSE"
rm -rf "$CONTAINED_TRUE"
rm -rf "$CONTAINED_TRUE_2"

dotnet publish "$APP" -o "$CONTAINED_FALSE"
dotnet publish "$APP" -o "$CONTAINED_TRUE" --self-contained
dotnet publish "$APP" -o "$CONTAINED_TRUE_2" --self-contained
echo "normal vs big"
diff --recursive -qs "$CONTAINED_TRUE" "$CONTAINED_FALSE" || echo "diffs found"
echo "big vs big again"
diff --recursive -qs "$CONTAINED_TRUE" "$CONTAINED_TRUE_2"|| echo "diffs found"

echo "$CONTAINED_TRUE"
echo "$CONTAINED_TRUE_2"