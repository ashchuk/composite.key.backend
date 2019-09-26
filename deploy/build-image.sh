#!/bin/bash 

while test $# -gt 0; do
  case "$1" in
    -h|--help)
      echo "$package - push image to yandex cloud"
      echo " "
      echo "$package [options] application [arguments]"
      echo " "
      echo "options:"
      echo "-h, --help          show brief help"
      echo "-t, --tag=TAG       specify an image tag"
      exit 0
      ;;
    -t)
      shift
      if test $# -gt 0; then
        export TAG=$1
      else
        echo "tag is empty"
        exit 1
      fi
      shift
      ;;
    --tag*)
      export TAG=`echo $1 | sed -e 's/^[^=]*=//g'`
      shift
      ;;
    *)
      break
      ;;
  esac
done

if [ -z "$TAG" ]
  then
    echo "No arguments supplied, use --tag flag to set image tag"
    exit
fi

echo "build" cr.yandex/crp7ffcu7rptsffd1crm/composite.key.backend:$TAG "image..."
docker build -t cr.yandex/crp7ffcu7rptsffd1crm/composite.key.backend:$TAG -f ../Dockerfile ../

echo "push " cr.yandex/crp7ffcu7rptsffd1crm/composite.key.backend:$TAG " image..."
docker push cr.yandex/crp7ffcu7rptsffd1crm/composite.key.backend:$TAG
