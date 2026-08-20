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
using System.Net;
using Menu;

namespace RainWorldVoiced;

/// <summary>
/// Handles the actual playing of the voicelines along with the in-game dialogue
/// </summary>
public static class DialogueHandler
{
    //-- TODO: Could make this, along with other things, a Remix option
    //private const int CollectionDelay = 40;

    /// <summary>
    /// Used for chat logs, linear broadcasts and dev commentary, is also used for everything in the collection
    /// </summary>
    //private static string[] CurrentMessages;

    private static readonly Queue<SoundID> CollectionQueue = new();

    //private static MenuMicrophone.MenuSoundObject CollectionCurrentlyPlayingSound;

    private static SoundEmitter CurrentVoicline; //-- The most recent voiceline that has been played in-game

    private static BodyChunk GhostBodyChunk; //-- An invisible player chunk that we map to the position of an echo so we can use ChunkSoundEmmiter.Room.PlaySound()

    //private static int CollectionTimeSinceLastSound;


    private static float TimeSinceLastVoiceline;

    private static float SoundVolume;

    public static bool SoundIsPlaying;

    public static string VoiceActorName;

    public static MenuLabel DatingSimVALabel;

    //private static SimpleButton CollectionsPausePlay;

    //private static SimpleButton CollectionsRestart;

    //private static FSprite CollectionsPausePlaySprite;
    public static void Init()
    {
        //-- Handles dialouge advancing faster than the sound effects finish
        On.HUD.DialogBox.Update += DialogBox_Update;

        //-- Handles regular dialogue, iterators and echoes
        On.HUD.DialogBox.InitNextMessage += DialogBox_InitNextMessage;

        //-- Handles tutorial text
        On.HUD.TextPrompt.InitNextMessage += TextPrompt_InitNextMessage;

        //-- Handles chat logs, linear broadcasts and dev commentary
        //On.MoreSlugcats.ChatLogDisplay.InitNextMessage += ChatLogDisplay_InitNextMessage;

        //-- Handles collection
        //On.MoreSlugcats.CollectionsMenu.InitLabelsFromChatlog += CollectionsMenu_InitLabelsFromChatlog;

        //-- Stores the english version of the current chat log so we can find the correct voicelines
        //On.MoreSlugcats.ChatlogData.DecryptResult += ChatlogData_DecryptResult;

        //-- Stores the english version of the pearl when reading from the collection so we can find the correct voicelines
        //On.MoreSlugcats.CollectionsMenu.InitLabelsFromPearlFile += CollectionsMenu_InitLabelsFromPearlFile;

        //-- The collection menu dumps all text at once instead of line by line, so we have to handle the playback ourselves
        //On.MoreSlugcats.CollectionsMenu.Update += CollectionsMenu_Update;

        //-- Stop playing if we leave the collection menu
        //On.MoreSlugcats.CollectionsMenu.OnExit += CollectionsMenu_OnExit;

        //-- Stores the currently playing voiceline in the collection so we know when to play the next one
        //On.MenuMicrophone.MenuSoundObject.ctor += MenuSoundObject_ctor;

        //-- Sets the volume of our sounds and tells us if any sound is currently playing
        On.VirtualMicrophone.PositionedSound.Update += PositionedSound_Update;

        //-- Whenever a new screen on the dating sim is created, we update our overlay and voicline accordingly
        //On.MoreSlugcats.DatingSim.InitNextFile += DatingSim_InitNextFile;

        //On.MoreSlugcats.CollectionsMenu.Singal += CollectionsMenu_Singal;
    }

    //Collection menu and dating sim stuff, commented code is unfinished and does not entirely work properly to be implemented at a later date
    /*
    private static void MenuSoundObject_ctor(On.MenuMicrophone.MenuSoundObject.orig_ctor orig, MenuMicrophone.MenuSoundObject self, MenuMicrophone mic, SoundLoader.SoundData soundData, bool loop, float initPan, float initVol, float initPitch, bool startAtRandomTime)
    {
        orig(self, mic, soundData, loop, initPan, initVol, initPitch, startAtRandomTime);
        if (VoicelineHandler.IsOurs(soundData.soundID))
        {
            CollectionCurrentlyPlayingSound = self;
            Debug.Log("A new sound has been registered");
        }
    }
    private static void StopCollectionPlayback()
    {
        if (Custom.rainWorld.processManager.currentMainLoop is not CollectionsMenu menu) return;

        CollectionQueue.Clear();

        foreach (var obj in menu.manager.menuMic.soundObjects)
        {
            if (VoicelineHandler.IsOurs(obj.soundData.soundID))
            {
                obj.Destroy();
            }
        }
    }

    private static void CollectionsMenu_Singal(On.MoreSlugcats.CollectionsMenu.orig_Singal orig, CollectionsMenu self, MenuObject sender, string message)
    {
        orig(self, sender, message);

        if (message == "RWV_PAUSE_PLAY"){
            if (SoundIsPlaying)
            {
                CollectionCurrentlyPlayingSound.Stop();
            }
        }
    }

    private static void DatingSim_InitNextFile(On.MoreSlugcats.DatingSim.orig_InitNextFile orig, DatingSim self, string filename)
    {
        orig(self, filename);

        if(DatingSimVALabel == null) //If there is currently no overlay, create one
        {
            DatingSimVALabel = new MenuLabel(self, self.pages[0], GetVoiceActor(filename), new Vector2(300f, 300f), new Vector2(2f, 2f), false);
            DatingSimVALabel.label.alpha = 1.0f;
            DatingSimVALabel.label.color = Color.black;
            self.pages[0].subObjects.Add(DatingSimVALabel);
            DatingSimVALabel.pos.x = DatingSimVALabel.label.textRect.width / 2f;
            DatingSimVALabel.pos.y = DatingSimVALabel.label.textRect.height / 2f;
        } else
        { //Else, update the overlay with the new information
            DatingSimVALabel.label.text = GetVoiceActor(filename);
            DatingSimVALabel.pos.x = DatingSimVALabel.label.textRect.width / 2f;
            DatingSimVALabel.pos.y = DatingSimVALabel.label.textRect.height / 2f;
        }
        if(SoundIsPlaying) //Stop any currently playing sound effects
        {
            CollectionCurrentlyPlayingSound.Stop();
        }

        var sound = GetSoundID(filename);
        self.PlaySound(sound);
    }


    */

    //Tutorial voice / text prompts
    private static void TextPrompt_InitNextMessage(On.HUD.TextPrompt.orig_InitNextMessage orig, TextPrompt self)
    {
        orig(self);

        if (RWVRemixMenu.MuteTutorialText.Value) return;

        try
        {
            var sound = GetSoundID(self.messageString.Replace("\r\n", "<LINE>"));
            string VA = GetVoiceActor(self.messageString.Replace("\r\n", "<LINE>"));
            Overlay.VAOverlayString = VA;
            self.hud.PlaySound(sound);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        SoundVolume = RWVRemixMenu.TutorialsVolume.Value;


        Debug.Log("[RWVF] Playing tutorial voiceline");
    }

    ///Echoes and iterators
    private static void DialogBox_InitNextMessage(On.HUD.DialogBox.orig_InitNextMessage orig, HUD.DialogBox self)
    {
        orig(self);

        var sound = GetSoundID(self.CurrentMessage.text.Replace("\r\n", "<LINE>"));
        VoiceActorName = GetVoiceActor(self.CurrentMessage.text.Replace("\r\n", "<LINE>"));
        if (sound == null)
        {
            Debug.Log("[RWVF] Sound effect returned null");
            Debug.Log(Translator.Untranslate(self.CurrentMessage.text.Replace("\r\n", "<LINE>")));
            return;
        }
        var played = false;
        //-- Can't grab the room directly from the HUD's owner because SplitScreenCoop exists
        if (Custom.rainWorld.processManager.currentMainLoop is RainWorldGame game)
        {
            //-- A bit ugly, but should make it compatible with SplitScreenCoop, might require some testing 
            foreach (var camera in game.cameras)
            {
                if (played) break;

                var room = camera.room;
                foreach (var obj in room.updateList)
                {
                    if (obj is Oracle oracle)
                    {
                        if (RWVRemixMenu.MuteIterators.Value) return;
                        try
                        {
                            CurrentVoicline = room.PlaySound(sound, oracle.bodyChunks[0]);
                            SoundVolume = RWVRemixMenu.IteratorVolume.Value*RWVRemixMenu.VoiceVolume.Value*0.5f;
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e);
                        }
                        played = true;

                        break;
                    }

                    if (obj is Ghost ghost)
                    {
                        if (RWVRemixMenu.MuteEchoes.Value) return;

                        Debug.Log("[RWV] Attempting to play Echo dialouge");

                        //-- Creating an invisible BodyChunk to play a sound from because Echoes don't use BodyChunks
                        GhostBodyChunk = new BodyChunk(room.updateList.OfType<Player>().FirstOrDefault(), 0, ghost.pos, 0f, 0);
                        try
                        {
                            CurrentVoicline = room.PlaySound(sound, GhostBodyChunk);
                            SoundVolume = RWVRemixMenu.EchoVolume.Value * RWVRemixMenu.VoiceVolume.Value * 0.5f;
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e);
                        }

                        played = true;

                        GhostBodyChunk = null;
                        break;

                    }
                }


                //-- Probably won't happen, but should work as a fallback in case something weird happens and the source of the voice can't be found
                if (!played)
                {
                    self.hud.PlaySound(sound);
                }

            }
        }
    }

    private static SoundID GetSoundID(string text)
    {
        var originalText = Translator.Untranslate(text);

        //-- TODO: Log to a file so we can catch mistakes and missing voicelines
        if (string.IsNullOrEmpty(originalText) || !VoicelineHandler.TryGetSoundID(originalText, out var sound)) return null;

        return sound;
    }
    
    private static string GetVoiceActor(string text)
    {
        var originalText = Translator.Untranslate(text);

        if (string.IsNullOrEmpty(originalText) || !VoicelineHandler.TryGetVoiceActor(originalText, out string VA)) return null;

        return VA;
    }
    private static void PositionedSound_Update(On.VirtualMicrophone.PositionedSound.orig_Update orig, VirtualMicrophone.PositionedSound self, float timeStacker, float timeSpeed)
    {
        if (VoicelineHandler.IsOurs(self.soundData.soundID))
        {
            self.volume = RWVRemixMenu.VoiceVolume.Value * SoundVolume;
            timeSpeed = 1; //-- Stops voicelines from being slowed down by echoes like other sounds
            SoundIsPlaying = true;
        }
        else
        {
            SoundIsPlaying = false;
        }
        orig(self, timeStacker, timeSpeed);
    }


    private static void DialogBox_Update(On.HUD.DialogBox.orig_Update orig, DialogBox self)
    {
        orig(self);
        //-- If the sound is still playing do not initiate the next line to prevent overlapping shenanigans
        if (self.CurrentMessage != null && CurrentVoicline != null && CurrentVoicline.soundStillPlaying)
        {
            if(self.showText == self.CurrentMessage.text)
            {
                self.lingerCounter = (self.CurrentMessage.linger - 1);
            }
            try
            {
                if(!RWVRemixMenu.EnableVAOverlay.Value) { return; }

                Overlay.VaTextOverlay.alpha += 5f * Time.deltaTime;
                Overlay.VaTextOverlay.text = VoiceActorName;
                Overlay.VaTextOverlay.x = (Overlay.VaTextOverlay.textRect.width) + 50f;
                Overlay.VaTextOverlay.y = Screen.height - 30.0001f;

                if (Overlay.VaTextOverlay.alpha > 1.5f)
                {
                    Overlay.VaTextOverlay.alpha = 1.5f;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            TimeSinceLastVoiceline = 0;
        }
        else
        {
            if (!RWVRemixMenu.EnableVAOverlay.Value) { return; }
            if (TimeSinceLastVoiceline < 1f)
            {
                TimeSinceLastVoiceline += Time.deltaTime;
                return;
            }           

            Overlay.VaTextOverlay.alpha -= 5f * Time.deltaTime;
            if (Overlay.VaTextOverlay.alpha < 0)
            {
                Overlay.VaTextOverlay.alpha = 0;
            }

        }
    }
}