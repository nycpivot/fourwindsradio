STEP 1
sudo apt update
sudo apt install libnginx-mod-rtmp
sudo nano /etc/nginx/nginx.conf
WRITE IN
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

sudo ufw allow 1935/tcp
sudo systemctl reload nginx.service

STEP 2
sudo apt install python3-pip
sudo pip install youtube-dl
youtube-dl address -f mp4
sudo apt install ffmpeg
ffmpeg -re -i "VIDEO NAME" -c:v copy -c:a aac -ar 44100 -ac 1 -f flv rtmp://localhost/live/stream