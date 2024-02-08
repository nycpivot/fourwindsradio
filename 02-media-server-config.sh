#!/bin/bash

# https://www.youtube.com/watch?v=PhbVScnEGug
# https://opensource.com/article/19/1/basic-live-video-streaming-server

sudo apt update
sudo apt install nginx -y
sudo apt install libnginx-mod-rtmp -y
sudo apt install unzip -y
sudo apt install ffmpeg -y

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

rm rtmp-config.json

ufw allow 1935/tcp
systemctl reload nginx.service

wget https://fourwindsradio.blob.core.windows.net/linux-x64/linux-x64.zip
unzip linux-x64.zip
rm linux-x64.zip
cd linux-x64

chmod 777 FourWindsRadio.Tools.Media

./FourWindsRadio.Tools.Media

cd ~
