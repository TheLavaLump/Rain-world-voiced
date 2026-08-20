using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;

namespace RainWorldVoiced;

/// <summary>
/// Registers and holds references to the voicelines
/// </summary>
public static class VoicelineHandler
{
    
    private const string SOUND_PREFIX = "RWVoiced";
    private const string VA_PREFIX = "VA(s):";

    private static readonly Dictionary<string, string> VAs = new();
    private static readonly Dictionary<string, SoundID> Sounds = new();

    public static bool TryGetSoundID(string text, out SoundID sound) => Sounds.TryGetValue(text, out sound);
    public static bool TryGetVoiceActor(string text, out string VA) => VAs.TryGetValue(text, out VA);

    public static bool IsOurs(SoundID sound) => Sounds.Values.Contains(sound);

    private static int VoiceLineCount;


    public static void Init()
    {

        //-- Allows for the registering of SoundIDs without adding to sounds.txt
        IL.SoundLoader.LoadSounds += SoundLoader_LoadSounds;
        LoadVoicelines();

    }

    public static void LoadVoicelines()
    {
        Debug.Log("[RWVF]Loading Voicelines");

        VoiceLineCount = 0;
        foreach (var kvp in Sounds)
        {
            kvp.Value.Unregister();
        }
        Sounds.Clear();

        var lines = File.ReadAllLines(AssetManager.ResolveFilePath("rwvf_voicelines.txt"));

        foreach (var line in lines)
        {
            //-- Comments and empty lines
            if (string.IsNullOrEmpty(line.Trim()) || line.StartsWith("//")) continue;

            var splitLine = line.Split('|');


           
            if (splitLine.Length != 3)
            {
                Debug.LogError($"Invalid voiceline entry! {line}");
                continue;
            }
            //Debug.Log("Sounds[" + splitLine[1] + "] = new SoundID(" + SOUND_PREFIX + " + " + splitLine[0] + ", true");

            Sounds[splitLine[1]] = new SoundID(SOUND_PREFIX + splitLine[0], true);
            VAs[splitLine[1]] = VA_PREFIX + splitLine[2];

            VoiceLineCount++;

        }

        Debug.Log("[RWVF]Loaded " + VoiceLineCount.ToString() + " voicelines");
    }

    private static void SoundLoader_LoadSounds(ILContext il)
    {

        var cursor = new ILCursor(il);

        var loc = -1;
        cursor.GotoNext(MoveType.After,
            i => i.MatchLdstr("Sounds.txt"),
            i => i.MatchCallOrCallvirt<string>(nameof(string.Concat)),
            i => i.MatchCallOrCallvirt<AssetManager>(nameof(AssetManager.ResolveFilePath)),
            i => i.MatchCallOrCallvirt("System.IO.File", nameof(File.ReadAllLines)),
            i => i.MatchStloc(out loc));

        cursor.MoveAfterLabels();
        cursor.Emit(OpCodes.Ldloc, loc);

        cursor.EmitDelegate((string[] strings) =>
        {
            var index = strings.Length;
            Array.Resize(ref strings, strings.Length + Sounds.Count);

            foreach (var kvp in Sounds)
            {
                strings[index] = $"{kvp.Value.value}/dopplerFac=0 : {kvp.Value.value.Substring(SOUND_PREFIX.Length)}";
                index++;
            }

            return strings;
        });

        cursor.Emit(OpCodes.Stloc, loc);
    }
}