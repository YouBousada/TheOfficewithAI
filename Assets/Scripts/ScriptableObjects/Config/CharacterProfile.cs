using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Scriptable Objects/CharacterProfile")]
public class CharacterProfile : ScriptableObject
{
    public CharacterName characterName;
    public float speed;
    public float cameraSensitivity;
    public float patience;
    public RoomName nightStartingRoom;
    public float willingnessToSwitchJobs;

    [Header("Idle Behavior")]
    public float baseIdleTime;

    [Header("Social Behavior")]
    public float socialRepelStrength;
    public float socialAttractStrength;

    public List<JobName> jobsByPreference = new List<JobName>();
    public List<RoomName> possibleIdleRooms = new List<RoomName>();
    public List<CharacterName> likedCharacters = new List<CharacterName>();
    public List<CharacterName> dislikedCharacters = new List<CharacterName>();
}

