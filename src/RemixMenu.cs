using Menu.Remix.MixedUI;
using System;
using UnityEngine;


namespace RainWorldVoiced
{
    public class RWVRemixMenu : OptionInterface
    {
        public static Configurable<bool> MuteIterators;
        public static Configurable<bool> MuteEchoes;
        public static Configurable<bool> MuteTransmissions;
        public static Configurable<bool> MuteTutorialText;

        public static Configurable<float> VoiceVolume;
        public static Configurable<float> EchoVolume;
        public static Configurable<float> IteratorVolume;
        public static Configurable<float> TutorialsVolume;

        public static Configurable<bool> EchoDistortion;

        public static Configurable<bool> EnableVAOverlay;

        public RWVRemixMenu()
        {
            MuteIterators = config.Bind("RWV_Mute_Iterators", defaultValue: false);
            MuteEchoes = config.Bind("RWV_Mute_Echoes", defaultValue: false);
            MuteTransmissions = config.Bind("RWV_Mute_Transmissions", defaultValue: false);
            MuteTutorialText = config.Bind("RWV_Mute_TutorialText", defaultValue: false);
            VoiceVolume = config.Bind("RWV_Voiceline_Volume", 1f);
            EchoVolume = config.Bind("RWV_Echo_Volume", 1f);
            IteratorVolume = config.Bind("RWV_Iterator_Volume", 1f);
            TutorialsVolume = config.Bind("RWV_Tutorial_Volume", 1f);
            EnableVAOverlay = config.Bind("RWV_Enable_VA_Overlay", defaultValue: true);
            EchoDistortion = config.Bind("RWV_Echo_Distortion", defaultValue: false);
        }

        private static Color[] RemixMenuColours = {
            new Color(52f / 255f, 89f / 255f, 171f / 255f),
            new Color(185f / 255f, 238f / 255f, 249f / 255f),
            new Color(232f / 255f, 89f / 255f, 199f / 255f),
            new Color(69f / 255f, 79f / 255f, 191f / 255f)

        };

        private static float[] RWVRemixMenuPosX = {
            25f,
            25f,
            25f,
            325f,
            325f,
            325f,
        };

        private static float[] RWVRemixMenuPosY = {
            440f,
            295f,
            85f,
            440f,
            295f,
            190f,

        };

        public override void Initialize()
        {
            base.Initialize();
            OpTab opTab = new OpTab(this, "Config");
            Tabs = new OpTab[1] { opTab };

            UIelement[] elements = new UIelement[] //--I hate everything about this but I don't think there is any easy-ish way to make it look nice
            {

                // MASTER

                new OpRect(new Vector2(RWVRemixMenuPosX[0], RWVRemixMenuPosY[0]), new Vector2(250f, 120f)) 
                {
                    colorEdge = Color.grey
                },
                new OpLabel(RWVRemixMenuPosX[0] + 10f, RWVRemixMenuPosY[0] + 90f, "Master~", true)
                {
                    color = Color.grey
                },
                new OpFloatSlider(VoiceVolume, new Vector2(RWVRemixMenuPosX[0] + 10, RWVRemixMenuPosY[0] + 50), 175, 1, vertical: false)
                {
                   colorEdge = Color.grey,
                   colorFill = Color.black,
                   colorLine = Color.grey,
                },
                new OpLabel(RWVRemixMenuPosX[0] + 200f, RWVRemixMenuPosY[0] + 55f, "Volume")
                {
                    color = Color.grey
                },
                new OpCheckBox(EnableVAOverlay, new Vector2(RWVRemixMenuPosX[0] + 10f, RWVRemixMenuPosY[0] + 30f))
                {
                    colorEdge = Color.grey,
                    description = "When enabled, the voice actors that are speaking will be displayed in the top left of the screen."
                },
                new OpLabel(RWVRemixMenuPosX[0] + 40f, RWVRemixMenuPosY[0] + 33f, "Show voice actors")
                {
                    color = Color.grey
                },

                //Echoes

                new OpRect(new Vector2(RWVRemixMenuPosX[1], RWVRemixMenuPosY[1]), new Vector2(250f, 130f))
                {
                    colorEdge = RemixMenuColours[0]
                },
                new OpLabel(RWVRemixMenuPosX[1] + 10, RWVRemixMenuPosY[1] + 90f, "Echoes~", true)
                {
                    color = RemixMenuColours[0]
                },
                new OpFloatSlider(EchoVolume, new Vector2(RWVRemixMenuPosX[1] + 10, RWVRemixMenuPosY[1] + 50f), 175, 1, vertical: false)
                {
                   colorEdge = RemixMenuColours[0],
                   colorFill = Color.black,
                   colorLine = RemixMenuColours[0],
                },
                new OpLabel(RWVRemixMenuPosX[1] + 200f, RWVRemixMenuPosY[1] + 55f, "Volume")
                {
                    color = RemixMenuColours[0]
                },
                new OpCheckBox(MuteEchoes, new Vector2(RWVRemixMenuPosX[1] + 10f, RWVRemixMenuPosY[1] + 30f))
                {
                    colorEdge = RemixMenuColours[0]
                },
                new OpLabel(RWVRemixMenuPosX[1] + 40f, RWVRemixMenuPosY[1] + 33f, "Mute")
                {
                    color = RemixMenuColours[0]
                },
                new OpCheckBox(EchoDistortion, new Vector2(RWVRemixMenuPosX[1] + 80f, RWVRemixMenuPosY[1] + 30f))
                {
                    colorEdge = RemixMenuColours[0],
                    description = "Applys the time distortion effect of being near an echo to echo voicelines."
                },
                new OpLabel(RWVRemixMenuPosX[1] + 110f, RWVRemixMenuPosY[1] + 33f, "Enable distortion")
                {
                    color = RemixMenuColours[0]
                },

                //Transmissions

                new OpRect(new Vector2(RWVRemixMenuPosX[2], RWVRemixMenuPosY[2]), new Vector2(250f, 200f))
                {
                    colorEdge = Color.black
                },

                //Iterators

                new OpRect(new Vector2(RWVRemixMenuPosX[3], RWVRemixMenuPosY[3]), new Vector2(250f, 120f))
                {
                    colorEdge = RemixMenuColours[2]
                },
                new OpLabel(RWVRemixMenuPosX[3] + 10, RWVRemixMenuPosY[3] + 90f, "Iterators~", true)
                {
                    color = RemixMenuColours[2]
                },
                new OpFloatSlider(IteratorVolume, new Vector2(RWVRemixMenuPosX[3] + 10, RWVRemixMenuPosY[3] + 50f), 175, 1, vertical: false)
                {
                   colorEdge = RemixMenuColours[2],
                   colorFill = Color.black,
                   colorLine = RemixMenuColours[2],
                },
                new OpLabel(RWVRemixMenuPosX[3] + 200f, RWVRemixMenuPosY[3] + 55f, "Volume")
                {
                    color = RemixMenuColours[2]
                },
                new OpCheckBox(MuteIterators, new Vector2(RWVRemixMenuPosX[3] + 10f, RWVRemixMenuPosY[3] + 30f))
                {
                    colorEdge = RemixMenuColours[2]
                },
                new OpLabel(RWVRemixMenuPosX[3] + 40f, RWVRemixMenuPosY[3] + 33f, "Mute")
                {
                    color = RemixMenuColours[2]
                },

                //Tutorial text

                new OpRect(new Vector2(RWVRemixMenuPosX[4], RWVRemixMenuPosY[4]), new Vector2(250f, 130f))
                {
                    colorEdge = Color.white
                },
                new OpLabel(RWVRemixMenuPosX[4] + 10, RWVRemixMenuPosY[4] + 90f, "Tutorial voice~", true)
                {
                    color = Color.white
                },
                new OpFloatSlider(TutorialsVolume, new Vector2(RWVRemixMenuPosX[4] + 10, RWVRemixMenuPosY[4] + 50f), 175, 1, vertical: false)
                {
                   colorEdge = Color.white,
                   colorFill = Color.black,
                   colorLine = Color.white,
                },
                new OpLabel(RWVRemixMenuPosX[4] + 200f, RWVRemixMenuPosY[4] + 55f, "Volume")
                {
                    color = Color.white
                },
                new OpCheckBox(MuteTutorialText, new Vector2(RWVRemixMenuPosX[4] + 10f, RWVRemixMenuPosY[4] + 30f))
                {
                    colorEdge = Color.white
                },
                new OpLabel(RWVRemixMenuPosX[4] + 40f, RWVRemixMenuPosY[4] + 33f, "Mute")
                {
                    color = Color.white
                },

            };
            opTab.AddItems(elements);


            OpContainer opContainerConfigIcon = new OpContainer(new Vector2(0f, 0f));
            opTab.AddItems(opContainerConfigIcon);
            FSprite nodeConfig = new FSprite("atlases/RWVF/RWVF_Config_Icon")
            {
                x = 300f,
                y = 590f,
                width = 200f,
                height = 200/4f
            };
            opContainerConfigIcon.container.AddChild(nodeConfig);

            OpContainer opContainerComingSoon = new OpContainer(new Vector2(0f, 0f));
            opTab.AddItems(opContainerConfigIcon);
            FSprite nodeComingSoon = new FSprite("atlases/RWVF/RWVF_COMINGSOON_Icon")
            {
                x = RWVRemixMenuPosX[2] + 250 / 2f,
                y = RWVRemixMenuPosY[2] + 120 / 2f +100,
                width = 250f,
                height = 120f
            };
            opContainerConfigIcon.container.AddChild(nodeComingSoon);


            OpContainer opContainerItIcon = new OpContainer(new Vector2(0f, 0f));
            opTab.AddItems(opContainerItIcon);

        }
    }
}