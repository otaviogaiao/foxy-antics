using Godot;
using System;
using System.Collections.Generic;

public partial class SoundManager : Node
{
    public const string SoundLaser = "laser";
    public const string SoundCheckpoint = "checkpoint";
    public const string SoundDamage = "damage";
    public const string SoundKill = "kill";
    public const string SoundGameover = "gameover1";
    public const string SoundImpact = "impact";
    public const string SoundLand = "land";
    public const string SoundMusic1 = "music1";
    public const string SoundMusic2 = "music2";
    public const string SoundPickup = "pickup";
    public const string SoundBossArrive = "boss_arrive";
    public const string SoundJump = "jump";
    public const string SoundWin = "win";

    private Dictionary<string, AudioStream> _sounds = new Dictionary<string, AudioStream>
    {
        { SoundCheckpoint, GD.Load<AudioStream>("res://Assets/sound/checkpoint.wav") },
        { SoundDamage, GD.Load<AudioStream>("res://Assets/sound/damage.wav") },
        { SoundKill, GD.Load<AudioStream>("res://Assets/sound/pickup5.ogg") },
        { SoundGameover, GD.Load<AudioStream>("res://Assets/sound/game_over.ogg") },
        { SoundImpact, GD.Load<AudioStream>("res://Assets/sound/impact.wav") },
        { SoundJump, GD.Load<AudioStream>("res://Assets/sound/jump.wav") },
        { SoundLand, GD.Load<AudioStream>("res://Assets/sound/land.wav") },
        { SoundLaser, GD.Load<AudioStream>("res://Assets/sound/laser.wav") },
        { SoundMusic1, GD.Load<AudioStream>("res://Assets/sound/Farm Frolics.ogg") },
        { SoundMusic2, GD.Load<AudioStream>("res://Assets/sound/Flowing Rocks.ogg") },
        { SoundPickup, GD.Load<AudioStream>("res://Assets/sound/pickup5.ogg") },
        { SoundBossArrive, GD.Load<AudioStream>("res://Assets/sound/boss_arrive.wav") },
        { SoundWin, GD.Load<AudioStream>("res://Assets/sound/you_win.ogg") }
    };
    
    public static SoundManager Instance { get; private set; }
	
    public override void _Ready()
	{
        Instance = this;
	}
    
    public static void PlayClip(AudioStreamPlayer2D player, string clipKey)
    {
        if (!Instance._sounds.ContainsKey(clipKey))
        {
            return;
        }

        player.Stream = Instance._sounds[clipKey];
        player.Play();
    }
}
