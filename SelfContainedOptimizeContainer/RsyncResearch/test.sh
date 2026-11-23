#!/usr/bin/env bash
set -euo pipefail
set -x

SCRIPT_DIR=$(realpath $(dirname $0))
cd "$SCRIPT_DIR"

TAG="test-rsync"

podman image build -t "$TAG" "$SCRIPT_DIR"
podman run --rm -it "$TAG" || echo "failed" # bash