#!/usr/bin/env bash
set -euo pipefail
set -x

SCRIPT_DIR=$(realpath $(dirname $0))
cd "$SCRIPT_DIR"

TAG="simple-app"


podman image build -t "$TAG" \
    --build-arg=CACHE_BREAKER="$(date)" \
    "$SCRIPT_DIR"
podman run --rm -it "$TAG" || echo "failed" # bash
podman image history "$TAG" --format json | yq -o json -P