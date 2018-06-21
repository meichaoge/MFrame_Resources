#!/bin/sh

OUT_DIR="packet/"
PACKET_FILE="net_packet.proto"
ATTR_FILE="public_attr.proto"

WRITE_TOOLS="write_config"

CFLAGS="-I. --cpp_out"

help()
{
  echo "first parameter for input file if '--build-all' build all"
  exit 0
}

if [ "$1" = '--build-all' ]
then
  protoc $PACKET_FILE $CFLAGS $OUT_DIR
  protoc $ATTR_FILE $CFLAGS $OUT_DIR

  cp packet/* ../server/client/include/protobuf
  cp packet/* ../server/gw_srv/include/protobuf
  cp packet/* ../server/lobby_srv/include/protobuf
  exit 0
elif [ "$1" = '--help' ]
then
  help
  exit 0
else
  protoc $1 $CFLAGS $OUT_DIR
fi
