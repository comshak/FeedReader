[11/05/2007]
Distilled the way interops should be named and generated from the command line, so I made a script for each .NET version
that needs to be run BEFORE building the solution. The scripts are in interop/dotnet11 and interop/dotnet20.

[10/22/2007]
- made it so that when importing a feed, the temporary downloaded feed (from which feed information is 
  auto-discovered) is not disregarded, but transformed into the xml of the feed.

[9/14/2006]
- rename the FileUtils into something like FolderSupport
- add a method which makes the concatenation for me, like: FolderSupport.GetFolder(FolderEnum.Feeds, myfilename); This method should make sure all the folders in the hierarchy are present;
in other words it should not crash. Also should allow customizable folders (to change the feeds folder location for example).



[3/27/2007]
- The Import feed functionality works now. The ImportFeedForm dialog spawns another thread for the Autodiscover button, and everything seems to work good.

- Added functionality: New Folder, Delete folder.

[3/28/2007]
- Refactored the FeedNode class to allow custom sorting (in my case it's allowing sub-folders to appear before feeds in a parent folder).

[4/18]
- Implemented Drag-and-Drop for the TreeView nodes.

[4/19]
- Implemented the WriteFeedsThread and the mutual exclusion pattern.

[4/30]
- Fixed an error with saving the feeds.xml before shutting down the WriteFeeds thread, when closing the app.
- Added an About dialog box, under the Help menu.

[9/1/2009]
Thinking of changing the template to:

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">

<html>
<body style="margin:0px">
<div style="padding:5px 5px 10px 5px; background-color:#C0FFC8"><a href="$FEED_URL">$FEED_TITLE</a></div>
<div style="margin:8px">
$FEED_BODY
</div>
</body>
</html>

