# MathYourWay

1. In Unity, find the location of the Android SDK. From the Edit menu, choose Preferences/External Tools
2. Look for the Android SDK. On my system it was C:\Users\[username]\AppData\Local\Android\sdk
3. Open a command prompt, and change to this directory.
4. Change directory to the platform-tools subdirectory.
5. You should see adb.exe in this directory.
6. With the device connected via USB, run the following command:
adb devices (Mac is ./adb devices)
7. This should return device serial number followed by the word "device"
8. Run "adb logcat | findstr -i unity" (use grep on Mac) in the terminal window. This will find anything with the word "unity" in the output, case insensitive.
9. This will provide the contents of the device log that are associated with a running Unity application
