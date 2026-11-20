#!/usr/bin/env bash
set -euo pipefail
set -x

SCRIPT_DIR=$(realpath $(dirname $0))
cd "$SCRIPT_DIR"
APPS=('TheOtherApp' 'TheOtherApp2')

for app in "${APPS[@]}"; do
    TAG=$(echo "self-contained-experiment-$app" | tr '[:upper:]' '[:lower:]')
    echo "Tag: $TAG"
    podman image build -v "$SCRIPT_DIR/$app:/src" -t  "$TAG" .
    podman run --rm -it "$TAG" || echo "failed" # bash
done
