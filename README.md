# HeightMapParser
Terrain party height map parser.<br/>
[Source](http://terrain.party/)

Program upload files to "maps" folder in the same directory as the program.
Every request to server it's config. If you want to save it copy all files from "maps" to another directory.

### Results:<br/>
Box: 34.3927, 48.7852, 35.6726, 48.1294 with scale 0.1<br/>
[ASTER 30m](https://github.com/emptycoder/HeightMapParser/blob/master/Documentation/images/maps-merging%20(ASTER%2030m).jpg)<br/>
[Merged](https://github.com/emptycoder/HeightMapParser/blob/master/Documentation/images/maps-merging%20(Merged).jpg)<br/>
[SRTM3 v4.1](https://github.com/emptycoder/HeightMapParser/blob/master/Documentation/images/maps-merging%20(SRTM3%20v4.1).jpg)<br/>
[SRTM30 Plus](https://github.com/emptycoder/HeightMapParser/blob/master/Documentation/images/maps-merging%20(SRTM30%20Plus).jpg)

### Configs
Config load from folder "maps".<br/>
**Config file syntax:**
```
[Scale]
[Count images of X, image width / every tile (1081px)]
[Path to dir]
[Path to dir]
```

**Example:**
```
1.0
3
C:\maps\map-34.3927-48.7852-35.3927-47.7852
C:\maps\map-34.3927-47.7852-35.3927-46.7852
C:\maps\map-35.3927-48.7852-36.3927-47.7852
C:\maps\map-35.3927-47.7852-36.3927-46.7852
C:\maps\map-36.3927-48.7852-37.3927-47.7852
C:\maps\map-36.3927-47.7852-37.3927-46.7852
```
