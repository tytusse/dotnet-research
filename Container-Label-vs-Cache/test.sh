#!/usr/bin/env bash
set -euo pipefail
set -x

SCRIPT_DIR=$(realpath $(dirname $0))
TAG=$(basename "$SCRIPT_DIR" | tr '[:upper:]' '[:lower:]')
podman image build -t "$TAG" "$SCRIPT_DIR" --build-arg=MY_ARG="$(date)"
podman run --rm "$TAG"
# podman image history "$TAG" --format json | yq -o json -P
podman inspect "$TAG"