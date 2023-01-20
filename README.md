# pong
(not the video game)

This is a simple cross-platform CLI ping tool written using .NET Core. It's primary feature is the ability to play a console beep on a succesful ping or on an unsuccesful ping. This can be especially helpful if you're waiting for a system to go up / down and want an audible alert once it has started / stopped pinging.

### How to use pong
For now, you'll need to download the source code and build the application yourself for your respective OS.

Once you've built the binary you'll probably want to place it somewhere in your $PATH so you can use it easily from the command line.

```
Options:
    -b        Play a console beep when a ping is succesful.
    -B        Reverse of -b. Play a console beep when a ping is not succesful.
    -D        Print timestamp at the start of each line.
```
### Examples
Ping a device, displaying timestamps, and play the console beep noise when a ping is succesful.
```
pong -D -b 192.168.0.1
```
