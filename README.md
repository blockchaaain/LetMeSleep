# Let Me Sleep!
A simple server-side mod that allows sleep when only some players are in bed.

## How to install
- Use the [Thunderstore Mod Manager](https://valheim.thunderstore.io/package/Blockheim/LetMeSleep/)
- Only the server needs this mod installed!

## Configuration
When running this mod for the first time, ``blockchaaain.LetMeSleep.cfg`` will be generated within ``<Install directory>\BepInEx\config``

### Ratio (``ratio = 0.5``)
This ratio is the fraction of online players that must "vote" to sleep. Configurable between 1% (0.01) and 100% (1.0).

### Message (``showMessage = true``)
When enabled, the server shows a large center-screen HUD message to all players whenever anyone "votes" to sleep. The message displays the current number of players in bed.

Older releases posted this as a chat shout from "Server". Valheim no longer shows chat that does not come from a player, so the vote is shown on screen instead.

## Requirements
 - [BepInExPack for Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) installed on the server.

## Credits
[SkipSleep](https://github.com/RinseV/valheim-skipsleep) - Original code for counting the number of players in bed.  
[JotunnModStub](https://github.com/Valheim-Modding/JotunnModStub) - Mod template.  

### LetMeSleep attempts to improve upon SkipSleep
- Compatible with Valheim 1.0!
  - Various updates to Valheim have broken older mods.
- Server message is sent to all players only once when someone gets in bed.
  - Does not continue to send messages each time Valheim checks sleep conditions.
- Server message is not duplicated for each player online.
- Mod continues to work even if the chat message fails (and will only issue one error message).
- Source includes everything needed to build the project.
- Intends to make the code more concise and readable.


### Thanks
[Ancastasia](https://www.twitch.tv/ancastasia) - Hosting modded Valheim.  
[radicalpi](https://www.twitch.tv/radicalpi) - Testing the mod.

## Changelog
- 1.0.5:
  - Compatible with Valheim 1.0
  - Show the sleep vote via HUD message
- 1.0.4:
	- Modify for Hildir's Request
- 1.0.3:
	- Catch exceptions while sending messages.
- 1.0.2:
  - Modify for Valheim 0.214
- 1.0.1:
  - Limit ratio value between 0.01 and 1.0.
  - Upload to Thunderstore.
- 1.0.0:
  - Initial release

## Source
https://github.com/blockchaaain/LetMeSleep
