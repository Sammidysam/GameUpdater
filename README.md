GameUpdater
===========

A game updater library created in C#.  It can be used for any type of file, whether that be .exe, .jar, or .txt.

This library figures out if an update is necessary by checking the time of the most recent update and updating if the time of the most recent update is later than the time when last updated (the time on the file at the same directory as the game). When using the update() method, the updater will automatically timestamp the update with the current system time at the time of the update.


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
The program detects if internet is available by pinging google.com .  This is a lot faster and more efficient than the previous method of downloading google.com's source code.

If you are using this library, please make your launcher application a different application than your actual game.  This will allow the library to update it, as it can't update itself!  Here is some example code that will show you what I did to get the library going:
<pre>
  	Downloader downloader = new Downloader("text.txt", "date.txt", "http://sammidysam.github.com/text.txt", "http://sammidysam.github.com/date.txt");
	downloader.CheckForUpdate();
	Updater updater = new Updater("text.txt", "date.txt", "C:\\Users\\Sam\\Documents\\Website\\Sammidysam.github.com\\");
	updater.Update();
	Console.ReadKey(true);
</pre>

That's how simple it is.

In a recent update the functionality for progress to be printed on the console was added.  It will always do so when the file is being downloaded.  You do not need to add any arguments to the Downloader instance, as it will update the progress automatically.  However, the next lines in your Main() method will run before the download is finished unless you put in a while loop after calling CheckForUpdate().  It's bad if it runs before the download is finished because it can cause the game to try to launch an unfinished download game.  That would cause an error.  This is what the while loop should look like, with the method GameCode() being launching the game and other things to do before that:

<pre>
	Downloader downloader = new Downloader("hgbg1.0.2.zip", "date.txt", "http://sammidysam.github.com/hgbg1.0.2.zip", "http://sammidysam.github.com/date.txt");
	downloader.CheckForUpdate();
	while(downloader.GetHasInternet() && !downloader.GetDownloadComplete());
	GameCode();
</pre>

CheckForUpdate() will print out the progress value that it is currently at, such as "57.5% done".  The library will not make a progress bar for you, however, because I assume that you might want to customize that and I don't want to get in the way of that.

License
=======

This program is licensed under the GNU Lesser General Public License version 3. To see the license, visit the file [LICENSE](LICENSE).
