# ADB-Touch-Recorder
Processes raw ADB events into a bat script that can be used repeatedly 

# Usage:
- Open a command window and navigate to adb.exe
- Execute command: adb shell getevent -t > touches.txt
- Wait 5-10 seconds before interacting with the device
- Begin interacting with the device (Do note this tool does not natively support HOME, BACK, MENU button presses)
- Once finished press CRTL+C in the command window to stop the getevent command from executing
- Copy touches.txt file that was created in the adb.exe directory into the repo directory /bin/release
- Execute AdbTouchGenerator.exe
- An output.bat file will be created in /bin/release directory
- Double click this file with a phone tethered to the host PC and it will begin to repeat the sequence of record touches

# Input File  (touches.txt)
[    5686.076508] /dev/input/event2: EV_KEY       BTN_TOUCH            DOWN                
[    5686.076510] /dev/input/event2: EV_ABS       ABS_MT_POSITION_X    00000194            
[    5686.076513] /dev/input/event2: EV_ABS       ABS_MT_POSITION_Y    00000200            
[    5686.076516] /dev/input/event2: EV_ABS       ABS_MT_TOUCH_MINOR   00000006            
[    5686.076536] /dev/input/event2: EV_SYN       SYN_REPORT           00000000            
[    5686.087609] /dev/input/event2: EV_ABS       ABS_MT_POSITION_X    00000193            
[    5686.087614] /dev/input/event2: EV_ABS       ABS_MT_POSITION_Y    000001ff            
[    5686.087619] /dev/input/event2: EV_SYN       SYN_REPORT           00000000            
[    5686.098681] /dev/input/event2: EV_ABS       ABS_MT_POSITION_X    00000192            
[    5686.098688] /dev/input/event2: EV_ABS       ABS_MT_POSITION_Y    000001fe            
[    5686.098694] /dev/input/event2: EV_SYN       SYN_REPORT           00000000            
[    5686.109657] /dev/input/event2: EV_ABS       ABS_MT_POSITION_X    00000191            
[    5686.109662] /dev/input/event2: EV_ABS       ABS_MT_POSITION_Y    000001fd            
[    5686.109666] /dev/input/event2: EV_SYN       SYN_REPORT           00000000            
[    5686.116461] /dev/input/event5: EV_REL       REL_HWHEEL           000006a0            
[    5686.116465] /dev/input/event5: EV_REL       REL_DIAL             000004bf            
[    5686.116466] /dev/input/event5: EV_REL       REL_WHEEL            00000457            
[    5686.116468] /dev/input/event5: EV_REL       REL_MISC             00000e1f            
[    5686.116470] /dev/input/event5: EV_REL       REL_RY               000000ef            
[    5686.116472] /dev/input/event5: EV_REL       REL_RZ               00000004            
[    5686.116474] /dev/input/event5: EV_SYN       SYN_REPORT           00000000            
[    5686.120921] /dev/input/event2: EV_ABS       ABS_MT_POSITION_X    00000190            
[    5686.120929] /dev/input/event2: EV_SYN       SYN_REPORT           00000000            
[    5686.131040] /dev/input/event2: EV_ABS       ABS_MT_TRACKING_ID   ffffffff            
[    5686.131063] /dev/input/event2: EV_KEY       BTN_TOUCH            UP                  
[    5686.131065] /dev/input/event2: EV_SYN       SYN_REPORT           00000000            
[    5686.296074] /dev/input/event5: EV_REL       REL_HWHEEL           0000069f            
[    5686.296079] /dev/input/event5: EV_REL       REL_DIAL             000004c5            
[    5686.296081] /dev/input/event5: EV_REL       REL_WHEEL            00000457            
[    5686.296083] /dev/input/event5: EV_REL       REL_MISC             00000e4a            
[    5686.296086] /dev/input/event5: EV_REL       REL_RY               000000ef         

# Output File (output.bat)
adb shell input tap 741 447
ping 192.0.2.1 -n 1 -w 250 >nul
adb shell input tap 404 512
ping 192.0.2.1 -n 1 -w 250 >nul
adb shell input tap 401 567
ping 192.0.2.1 -n 1 -w 250 >nul
adb shell input tap 814 1170
ping 192.0.2.1 -n 1 -w 250 >nul
adb shell input tap 92 172
ping 192.0.2.1 -n 1 -w 250 >nul
adb shell input tap 743 780
ping 192.0.2.1 -n 1 -w 250 >nul
adb shell input tap 380 557
ping 192.0.2.1 -n 1 -w 250 >nul
adb shell input tap 846 1154

### NOTE: The above input and output do not reflect actual values but an depict the data formats.
