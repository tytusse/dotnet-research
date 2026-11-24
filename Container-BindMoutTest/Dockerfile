FROM ubuntu
WORKDIR /app
RUN --mount=type=bind,source=.,target=/src cat /src/file.txt > file.txt
CMD ["cat", "/app/file.txt"]