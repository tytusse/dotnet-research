##########
### dotnet runtime deps (no runtime itself)
FROM ubuntu:24.04 AS dotnet-runtime-deps
RUN apt-get -y update && \
    apt-get -y install -y gnupg tzdata libicu74 libssl3t64 ca-certificates && \
    apt-get clean

RUN update-ca-certificates
RUN ln -fs /usr/share/zoneinfo/Europe/Brussels /etc/localtime

##########
### dotnet SDK and GIT
FROM dotnet-runtime-deps AS dotnet-builder
# no apt update, we want it to be consistent with base which has its own update.
# dotnet SDK
RUN apt-get -y install software-properties-common dotnet-sdk-10.0 gpg apt-utils git && \
    apt-get clean

# CACHE SELF CONTAINED build artifacts here
FROM dotnet-builder as build-base
COPY --link --exclude=(bin|obj) /TheApp /base-src
RUN dotnet publish /base-src -o /app --self-contained=true

# use artifacts from previous step and overwrite differences
FROM dotnet-builder as build
# /src expected to be mounted # COPY --link --exclude=(bin|obj) /TheOtherApp /src
COPY --from=build-base /app /app
RUN dotnet publish /src -o /app --self-contained=false

# the actual app
FROM dotnet-runtime-deps
WORKDIR /app
COPY --link --from=build /app .
CMD ./program


