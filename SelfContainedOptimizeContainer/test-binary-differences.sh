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
rm -rf "$CONTAINED_FALSE"
rm -rf "$CONTAINED_TRUE"

dotnet publish "$APP" -o "$CONTAINED_FALSE"
dotnet publish "$APP" -o "$CONTAINED_TRUE" --self-contained

diff --recursive -qs "$CONTAINED_TRUE" "$CONTAINED_FALSE"