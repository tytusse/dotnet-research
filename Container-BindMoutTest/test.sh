#!/usr/bin/env bash
set -euo pipefail
set -x

SCRIPT_DIR=$(realpath $(dirname $0))
TAG=test-bind-mount
podman image build -t "$TAG" "$SCRIPT_DIR"
podman run --rm "$TAG"