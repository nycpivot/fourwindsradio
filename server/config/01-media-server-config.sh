#!/bin/bash

sudo apt update
sudo apt install libnginx-mod-rtmp

cat <<EOF | tee rtmp-config.json

rtmp {
  server {
    listen 1935;
    chunk_size 4096;
    allow publish 127.0.0.1;
    deny publish all;
    application live {
      live on;
      record off;
    }
  }
}
EOF

cat rtmp-config.json >> /etc/nginx/nginx.conf

sudo ufw allow 1935/tcp
sudo systemctl reload nginx.service

sudo apt install ffmpeg

#ffmpeg -re -i "VIDEO NAME" -c:v copy -c:a aac -ar 44100 -ac 1 -f flv rtmp://localhost/live/stream
