using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using HUD;
using On;
using IL;
using System.Xml;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using static System.Net.Mime.MediaTypeNames;


namespace RainWorldVoiced;

public static class Overlay
{
    public static FLabel VaTextOverlay;
    public static string VAOverlayString;

    public static void Apply()
    {
        On.PlayerGraphics.InitiateSprites += PlayerGraphics_InitiateSprites;
        //AlwaysOnTopContainer.AddChild(VAOverlayText);

    }
    private static void PlayerGraphics_InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        orig(self, sLeaser, rCam);

        if (!RWVRemixMenu.EnableVAOverlay.Value)
        {
            return;
        }

        VaTextOverlay = new FLabel("font", "");
        VaTextOverlay.x = Screen.width/9 + 0.001f;
        VaTextOverlay.y = Screen.height - Screen.height/11 + 0.001f;
        VaTextOverlay.scale = 2;
        VaTextOverlay.alpha = 0;
        VaTextOverlay.isVisible = true;


        Futile.stage.AddChild(VaTextOverlay);
    }
}