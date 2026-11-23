#!/usr/bin/env bash
set -euo pipefail
set -x

mkdir /result /candidate /base

echo "foo" > /base/constant-file.txt
echo "foo" > /base/volatile-file.txt
echo "foo" > /base/deleted-file.txt

cp /base/constant-file.txt /candidate/

echo "bar" > /candidate/volatile-file.txt
echo "bar" > /candidate/new-file.txt

rsync -aic --compare-dest="/base/" "/candidate/" "/result/"

ls -l /base /candidate /result
diff base/ candidate/   || : diff-found
diff candidate/ result/ || : diff-found
diff base/ result/ || : diff-found
