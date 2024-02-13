cd ~
rm -rf media
rm -rf linux-x64

wget https://fourwindsradio.blob.core.windows.net/linux-x64/linux-x64.zip
unzip linux-x64.zip
rm linux-x64.zip
cd linux-x64

chmod 777 FourWindsRadio.Tools.Media

./FourWindsRadio.Tools.Media
