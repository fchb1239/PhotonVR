# ![](Visuals/SmallerText.png)
A Unity Package containing all the necessary components to do VR networking with Photon

![](https://user-images.githubusercontent.com/29258204/178152451-dc96ea62-ead2-4ee1-a23f-71c14a3765e1.png)

# Documentation
By default everything is set up for a super simple system, obvisouly you can code some stuff yourself to make everything work for your application.

Start off by including Photon.VR;
```cs
using Photon.VR;
```

Connecting to the servers:
```cs
PhotonVRManager.Connect();
```

Switching Photon servers:
```cs
PhotonVRManager.ChangeServers("AppId", "VoiceAppId");
```

```cs
// It will only join people on the same queue but the room codes themselves are random
string queue = "Space";
// Optional
int maxPlayers = 8;
PhotonVRManager.JoinRandomRoom(queue, maxPlayers);
```

```cs
string roomCode = "1234";
// Optional
int maxPlayers = 8;
PhotonVRManagerJoinPrivateRoom(roomName, maxPlayers);
```

Switching scenes:
```cs
int sceneIndex = 1;
// Optional
int maxPlayers = 8;
PhotonVRManager.ChangeServers(SceneIndex, maxPlayers);
```
