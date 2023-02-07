# Let Me Sleep!
A simple server-side mod that allows players to skip to dawn when only some players are in bed.

## How to install
- If using Thunderstore Mod Manager, go to Settings, select "Import local mod", and select the "blockchaaain-LetMeSleep.zip" file.
- Otherwise, extract the .dll from the .zip file and place it in ``<Install directory>\BepInEx\plugins``

## Configuration
When running this mod for the first time, ``LetMeSleep.cfg`` will be generated within ``<Install directory>\BepInEx\config``

### Ratio (``ratio = 0.5``)
This ratio is the fraction of online players that must "vote" to sleep. Configurable between 1% (0.01) and 100% (1.0).

### Message (``showMessage = true``)
When enabled, the server will "shout" to all players whenever anyone "votes" to sleep. The message displays the current number of players in bed.

## Requirements
 - [BepInExPack for Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) installed on the server and client.

## Credits
[SkipSleep](https://github.com/RinseV/valheim-skipsleep) - Original code for counting the number of players in bed.  
[JotunnModStub](https://github.com/Valheim-Modding/JotunnModStub) - Mod template.  

### LetMeSleep attempts to improve upon SkipSleep
- Compatible with Mistlands!
  - The RPC_ChatMessage parameters have changed, causing exceptions in older mods.
- Server message is sent to all players only once when someone gets in bed.
  - Does not continue to send messages each time Valheim checks sleep conditions.
- Server message is not duplicated for each player online.
- Source includes everything needed to build the project.
- Intends to make the code more streamlined and readable at the same time.


### Thanks
[Ancastasia](https://www.twitch.tv/ancastasia) - Hosting modded Valheim.  
[radicalpi](https://www.twitch.tv/radicalpi) - Testing the mod.

## Changelog
- 1.0.0:
  - Initial release

## Source
https://github.com/blockchaaain/LetMeSleep
