#!/bin/bash

cd ~/media/playlist

playlist=playlist.txt

# stream playlist immediately with ffmpeg
# ffmpeg -f concat -safe 0 -stream_loop -1 -i "${playlist}" -c copy -f mpegts rtmp://localhost/live/stream

ffmpeg -f concat -re -safe 0 -stream_loop -1 -i "${playlist}" -c copy -f flv rtmp://localhost/live/stream

# ffmpeg -re -i "${playlist}" -c:v copy -c:a aac -ar 44100 -ac 1 -f flv rtmp://localhost/live/stream
