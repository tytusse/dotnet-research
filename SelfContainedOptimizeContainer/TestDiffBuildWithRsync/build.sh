#!/usr/bin/env bash
set -euo pipefail
set -x

# simulate compiler

OUT_DIR="${1:?expected out dir path as 1st argument}"

# /fake-framework files are supposed to be same on every run
# (provided we dont remove cache) - they simulate "framework dlls"
# and "incremental build unchanged files".
# script is a good enough aproximation what dotnet build likely does
# - copy common files to out (self contained)
#   this also somewhat covers incremental builds to an extent
# - copy app specific "binaries" to out.
# 
# it's result then need to be processed in some way such that
# unchanged files need not be copied to new layer.
# It would be ideal if podman build and 'COPY' instruction did it, 
# but it looks like it wont if `cp` command does not preserve metadata.
# question is - does dotnet build preserve it?
mkdir -p "$OUT_DIR"
cp -t "$OUT_DIR/" /fake-runtime/*
# copy src files to out, add .md to their name
for f in /src/*.txt; do
    OUT_FILE="$OUT_DIR/${f##*/}.md"
    cat $f > "$OUT_FILE"
    cat "$OUT_FILE"
done

ls -l "$OUT_DIR"