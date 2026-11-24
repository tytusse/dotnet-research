One can use `RUN ` with local filesystem mounted.
This is useful in rare cases though, as it still causes issues with cache (it likely wont invalidate if mounted files change)

It is an alternative to running `podman build` with `-v` to mount local directories into the build context.