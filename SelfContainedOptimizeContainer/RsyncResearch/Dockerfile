FROM ubuntu:24.04 as fake-sdk
RUN apt-get -y update && \
    apt-get -y install rsync && \
    apt-get clean
COPY --chmod=+x run.sh .
CMD ["./run.sh"]