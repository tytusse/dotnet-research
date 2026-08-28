set -x
VERSIONS=('10.0.400' '10.0.111')

for version in "${VERSIONS[@]}"; do
  echo "checking version $version"
  TAG="test-inlining-$version"
  # podman image rm $TAG
  podman build --build-arg sdkversion=$version -t $TAG .
  podman run --rm $TAG
done
