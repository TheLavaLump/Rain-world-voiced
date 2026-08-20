using System;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using UnityEngine;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace RainWorldVoiced;

[BepInPlugin(MOD_ID, "Rain World Voice Framework", "0.1.0")]
public class Plugin : BaseUnityPlugin
{
    public const string MOD_ID = "daszombes.rainworldvoiceframework";

    public bool IsInit;
    public void OnEnable()
    {
        On.RainWorld.OnModsInit += RainWorld_OnModsInit;
    }


    private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);
        try
        {
            VoicelineHandler.Init();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
        }
        MachineConnector.SetRegisteredOI(MOD_ID, new RWVRemixMenu());

        try
        {
            if (IsInit) return;
            IsInit = true;

            DialogueHandler.Init();
            Translator.Init();
            VoicelineHandler.Init();
            Overlay.Apply();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        Futile.atlasManager.LoadImage("atlases/RWVF/RWVF_Config_Icon");
        Futile.atlasManager.LoadImage("atlases/RWVF/RWVF_COMINGSOON_Icon");
    }
}