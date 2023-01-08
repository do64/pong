# pong
(not the video game)

This is a simple CLI ping tool. The main addition over the standard Windows ping.exe is the **-b** switch which will play a console beep when a ping is succesful. This is handy if you're waiting for something to come online and want to be notified once it's reachable. Pong also runs until you quit the application with Ctrl+C, similar to ping on *NIX systems. 

Example: ping a device by IP and play a console beep when the ping is succesful.
```
pong -b 192.168.0.1
```

Currently the application is very bare bones. More functionality will be added as time permits.
