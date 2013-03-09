GameUpdater
===========

A game updater library created in C#.  It can be used for any type of file, whether that be .exe, .jar, or .txt.


How to use:

First, you will need to reference the library in your C# project.  Before doing that, you will need to reference my FileHelper library also, which is located at https://github.com/Sammidysam/FileHelper .
This FileHelper library is used in this library.  From there you can create a new Downloader in your Main method.  You will need to pass four arguments into making a new Downloader:
the file name, the date file name, the file internet address, and the date file internet address.  The file names are not an absolute path.  The program will automatically add the file name to the local directory, so if you put in "text.txt" and your program is located in C:\\Users\\Sam\\Downloads\\ the program will end up with the path C:\\Users\\Sam\\Downloads\\text.txt .
The internet address is better to be copied down than written yourself.  An example of an internet address is http://sammidysam.github.com/text.txt .  From there you can call the CheckForUpdate() method in Downloader to download an update if one is available.

To update, you must make a new Updater.  There are only three arguments in making a new Updater: the file name, the date file name, and the path of where to update the files to.
The path is an absolute path.  It must be absolute or else an error will arise.  The reason why it is absolute and not relative is because this will not be called in games that are played by non-developers.
You will call this method if you are a developer and make sure it is not called when you release your game.  If it were non-absolute, then it would be much harder to update the files to the right path.
Files cannot be uploaded directly to the website.  Instead, you will update it to your website location on your computer and from there update the website.
After creating a new Updater, you will call the Update() method in Updater and it will update the file and date file at the absolute location specified.

This program will detect if the computer in use has internet.  If it does, it will attempt to download the files necessary.  If it doesn't it will not.  This allows you, the developer, to only need to call the CheckForUpdate() method.
The program detects internet connection via trying to download the source at https://www.google.com/ .  If it succeeds, it will determine that internet connection is available.  If it fails, it will not attempt to download anything.
I choose https://www.google.com/ as my website for determining internet connection because I feel like it is one of the least likely websites to be down at a given moment.  There may be even less likely websites, however, so I may change this in the future.