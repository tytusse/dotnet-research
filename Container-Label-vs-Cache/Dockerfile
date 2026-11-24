FROM ubuntu
ARG MY_ARG="bar"
LABEL Foo=$MY_ARG
RUN echo "will I be from cache if label changes?"
RUN echo "My arg was $MY_ARG" > /report.txt
CMD ["cat", "/report.txt"]