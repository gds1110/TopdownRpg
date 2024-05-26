using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }

	public enum State
	{
		Die,
		Moving,
		Idle,
		Skill,
	}

    public enum Layer
    {
        Ground = 3,
        Monster = 8,
        Block = 10,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        LPress,
        LPointerDown,
        LPointerUp,
        LClick,
        RPress,
        RPointerDown,
        RPointerUp,
        RClick,
    }

    public enum CameraMode
    {
        QuarterView,
    }

    public enum SpellType
    {
        Interact,
        Meele,
        Arange,
        Chanelling,
    }
}
