FROM ubuntu:24.04 as fake-sdk
RUN apt-get -y update && \
    apt-get -y install rsync && \
    apt-get clean
RUN mkdir -p /fake-runtime
# create 1MB file to make layer sizes more obvious in later analysys
RUN dd if=/dev/zero of=/fake-runtime/file.bin bs=1M count=1
RUN echo "pretend this is some standard 'dll' that is always copied." \
    > /fake-runtime/standard-lib-fake.txt.md
# compiler simulating command
COPY --link --chmod=+x build.sh .

FROM fake-sdk as build
COPY --link --chmod=+x build-diff.sh .
RUN --mount=type=bind,source=./src/base-app,target=/src ./build.sh /base-app
ARG CACHE_BREAKER="no-breaker"
# break cache if requested
ENV CACHE_BREAKER="$CACHE_BREAKER"
RUN --mount=type=bind,source=./src/desired-app,target=/src ./build.sh /desired-app-full
RUN ./build-diff.sh /base-app /desired-app-full /difference

FROM ubuntu:24.04 as base-app
CMD ["./run.sh"]
COPY  --chmod=+x run.sh .
# pre-built "base" app
COPY --link --from=build /base-app /app

FROM base-app AS app
# files that differ in our "current app" vs. "base app"
COPY --link --from=build /difference /app

