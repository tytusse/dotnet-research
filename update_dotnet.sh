#!/usr/bin/env bash
set -euo pipefail

usage() {
  echo "Usage: $0 <target-framework>"
  echo "Example: $0 net8.0"
  exit 1
}

[[ $# -eq 1 ]] || usage
NEW_TFM="$1"

# Accepts examples like:
# net8.0, net9.0, net8.0-windows, net8.0-android
# netstandard2.1, netcoreapp3.1
TFM_REGEX='^(net[0-9]+(\.[0-9]+)?|netstandard[0-9]+\.[0-9]+|netcoreapp[0-9]+\.[0-9]+)(-[A-Za-z0-9.-]+)?$'

if ! [[ "$NEW_TFM" =~ $TFM_REGEX ]]; then
  echo "Invalid target framework: '$NEW_TFM'"
  echo "Examples: net8.0, net9.0, net8.0-windows, netstandard2.1"
  exit 2
fi

find . -type f \( -name '*.csproj' -o -name '*.fsproj' \) -print0 |
  xargs -0 -r sed -i -E \
    -e "s#(<TargetFramework>)[^<]+(</TargetFramework>)#\1${NEW_TFM}\2#g" \
    -e "s#(<TargetFrameworks>)[^<]+(</TargetFrameworks>)#\1${NEW_TFM}\2#g"

echo "Updated projects to: $NEW_TFM"