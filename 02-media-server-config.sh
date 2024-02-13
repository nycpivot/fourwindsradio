#!/bin/bash

# https://www.youtube.com/watch?v=PhbVScnEGug
# https://opensource.com/article/19/1/basic-live-video-streaming-server

sudo apt update
sudo apt install nginx -y
sudo apt install libnginx-mod-rtmp -y
sudo apt install unzip -y
sudo apt install ffmpeg -y
# sudo apt install net-tools -y

sudo bash -c 'cat <<EOF | tee rtmp-config.json

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
EOF'

sudo bash -c 'cat rtmp-config.json >> /etc/nginx/nginx.conf'

sudo rm rtmp-config.json

sudo ufw allow 1935/tcp
sudo systemctl reload nginx.service
