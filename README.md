# Rain World Voice Framework
*(Formerly Rain World Voiced)*

A Rain World Mod that allows the easy creation of fan dubs for in-game dialogue. The mod currently supports creating dubs for Echo, Iterator and "Tutorial" text on the in-game UI.

Project headed by [Daszombes](https://www.youtube.com/@Daszombes) and maintained by [TheLavaLump](https://bsky.app/profile/thelavalump.dev).

## How to create your own dubs:

Firstly, download the Rain World Voice Framework Template that can be found [here](https://github.com/TheLavaLump/Rain-World-Voice-Framework-Template) tab. To add your own voicelines you need to edit rwf_voicelines.txt, stored in `Modify/`. Below is the syntax for registering a new voiceline:

`[ADD]AUDIOFILENAME|DIALOGUETRIGGER|VOICEACTOR`

`[ADD]` - Each new voiceline must begin with [ADD] for it to be properly registered by the mod

- `AUDIOFILENAME` - The name of the sound file that will be played when the dialogue trigger occurs, located in `/soundeffects`.
**NOTE: The name of this audio file should not contain any spaces or underscores as it will prevent the game from properly registering them.**

- `DIALOGUETRIGGER` - The line of in-game text that that will cause the voiceline to trigger when spoken.
**NOTE: If there is a line break in the dialouge, the position of this linebreak within the text must be notated with `<LINE>` in this trigger.**

- `VOICEACTOR` - The voice actor for the voiceline, will be used if the user has the `"Show voice actors"` setting enabled in the remix menu.

The last 3 sections should be separated by a vertical line character, `|`

An example of what a completed voiceline registration would look like is this:

`[ADD]it-5p-MeetSurv3|Know that this does not make you special - every living thing shares that same frustration.<LINE>From the microbes in the processing strata to me, who am, if you excuse me, godlike in comparison.|Daszombes`

*(From the Pebbles Voiced Mod)*
