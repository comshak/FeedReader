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
