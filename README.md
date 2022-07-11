# ![](Visuals/SmallerText.png)
A Unity Package containing all the necessary components to do VR networking with Photon

![](https://user-images.githubusercontent.com/29258204/178152451-dc96ea62-ead2-4ee1-a23f-71c14a3765e1.png)

# Documentation
By default everything is set up for a super simple system, obvisouly you can code some stuff yourself to make everything work for your application.

Start off by including Photon.VR
```cs
using Photon.VR;
```

Connecting to the servers
```cs
PhotonVRManager.Connect();
```

Switching Photon servers
```cs
PhotonVRManager.ChangeServers("AppId", "VoiceAppId");
```

Joining rooms
```cs
// It will only join people on the same queue but the room codes themselves are random
string queue = "Space";
// Optional
int maxPlayers = 8;
PhotonVRManager.JoinRandomRoom(queue, maxPlayers);
```

Joining private rooms
```cs
string roomCode = "1234";
// Optional
int maxPlayers = 8;
PhotonVRManagerJoinPrivateRoom(roomName, maxPlayers);
```

Switching scenes
```cs
int sceneIndex = 1;
// Optional
int maxPlayers = 8;
PhotonVRManager.SwitchScenes(SceneIndex, maxPlayers);
```

<b>Cosmetics</b>
Every body part on the player has a child named "Cosmetics", under those you put the models of the cosmetics you want.
You have to rename the object to the ID of the cosmetic, let's say you put on a hat with the ID "VRTopHat" then under the Cosmetics child of the head you put your model and name it "VRTopHat", like this:

![](https://user-images.githubusercontent.com/29258204/178257224-254c10c5-e68a-4fd9-97f4-308896e62bf7.png)


