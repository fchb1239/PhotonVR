# ![](Visuals/SmallerText.png)
A Unity Package containing all the necessary components to do VR networking with [Photon](https://photonengine.com)

[![Download](https://img.shields.io/badge/Download-blue.svg)](https://github.com/fchb1239/PhotonVR/releases)
[![Discord](https://img.shields.io/badge/Discord-blue.svg)](https://discord.gg/rRvnU846Bf)

# Documentation
By default everything is set up for a super simple system, obvisouly you can code some stuff yourself to make everything work for your application.

You need to have [PUN 2](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922) and [Photon Voice 2](https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518) in your project before importing this
You also need TextMeshPro - that's built into unity, you just gotta go into the Player prefab and press Import TMP Essentials.

![](https://user-images.githubusercontent.com/29258204/178261709-e87f2177-d4bc-4878-91ae-5d2d52d5081c.png)


Start off by going in Resources/PhotonVR/Prefabs and dragging everything in there into the scene.

![](https://user-images.githubusercontent.com/29258204/178261831-ee9e4744-5b80-443f-9dcc-5913dcaaca49.png)


Then, put your Photon AppId into AppId and Photon Voice AppId into VoiceAppId, then put in a region - the default is "eu". Click [here](https://doc.photonengine.com/en-us/pun/current/connection-and-authentication/regions) to see a list of regions

Then drag in your controllers and headset into Head, Left Hand and Right Hand - set the colour to the default colour.

Set Default Queue to the queue you want to automatically load into when the game starts and set the Default Room Limit to the maximum amount of players you want in a default room.

Connect On Awake makes it so when the game loads you instantly try to connect, Join Room On Connect makes it so it instantly joins the Default Queue with the Default Room Limit when you connect to the server.

![](https://user-images.githubusercontent.com/29258204/178260207-79da9ffe-efbb-44cc-a648-1cd40900c82d.png)

Including Photon.VR
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

Connecting to the servers authenticated
```cs
// These will not actually work, you need to set this up with PlayFab or something else
string username = "MYID";
string token = "MYTOKEN";
PhotonVRManager.ConnectAuthenticated(username, token);
```

Switching Photon servers authenticated
```cs
// These will not actually work, you need to set this up with PlayFab or something else
string username = "MYID";
string token = "MYTOKEN";
PhotonVRManager.ChangeServersAuthenticated("AppId", "VoiceAppId", username, token);
```

[PlayFab Photon authentication documentation](https://docs.microsoft.com/en-us/gaming/playfab/sdks/photon/quickstart)

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
PhotonVRManager.JoinPrivateRoom(roomCode, maxPlayers);
```

Switching scenes
```cs
int sceneIndex = 1;
// Optional
int maxPlayers = 8;
PhotonVRManager.SwitchScenes(SceneIndex, maxPlayers);
```

Setting name
```cs
PhotonVRManager.SetUsername("fchb1239");
```


Setting colour
```cs
Color myColour = new Color(0, 0, 1);
PhotonVRManager.SetColour(myColour);
```

<b>Cosmetics</b>

Start off by including Photon.VR.Cosmetics
```cs
using Photon.VR.Cosmetics;
```

To put on cosmetics you can use two functions to do the job. You can put on an entire set like so
```cs
PhotonVRManager.SetCosmetics(new PhotonVRCosmeticsData()
{
    Head = "VRTopHat",
    Face = "VRSunglasses",
    LeftHand = "VRGlove",
    RightHand = "VRGlove"
});
```

Or if you want to do one at a time (like if you have a button with a specefic cosmetic) then do like so
```cs
PhotonVRManager.SetCosmetic(CosmeticType.Head, "VRTopHat");
```

If you set a cosmetic part to a cosmetic that doesn't exit, it won't equip anything. So if you want to clear the head of cosmetics do like so
```cs
PhotonVRManager.SetCosmetic(CosmeticType.Head, "");
```
It's the same story with SetCosmetics.

Every body part on the player has a child named something with "Cosmetics", under those you put the models of the cosmetics you want.
You have to rename the object to the ID of the cosmetic, let's say you put on a hat with the ID "VRTopHat" then under the Cosmetics child of the head you put your model and name it "VRTopHat", like this:

![](https://user-images.githubusercontent.com/29258204/178257224-254c10c5-e68a-4fd9-97f4-308896e62bf7.png)


