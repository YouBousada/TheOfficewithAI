using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Scriptable Objects/CharacterProfile")]
public class CharacterProfile : ScriptableObject
{
    [SerializeField] private CharacterName characterName;
    [SerializeField] private float speed;
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float patience;
    [SerializeField] private RoomName nightStartingRoom;
    [SerializeField] private float willingnessToSwitchJobs;

    [Header("Idle Behavior")]
    [SerializeField] private float baseIdleTime;

    [Header("Social Behavior")]
    [SerializeField] private float socialRepelStrength;
    [SerializeField] private float socialAttractStrength;

    [SerializeField] private List<JobName> jobsByPreference = new();
    [SerializeField] private List<RoomName> possibleIdleRooms = new();
    [SerializeField] private List<CharacterName> likedCharacters = new();
    [SerializeField] private List<CharacterName> dislikedCharacters = new();

    public CharacterName CharacterName         => characterName;
    public float Speed                         => speed;
    public float CameraSensitivity             => cameraSensitivity;
    public float Patience                      => patience;
    public RoomName NightStartingRoom          => nightStartingRoom;
    public float WillingnessToSwitchJobs       => willingnessToSwitchJobs;
    public float BaseIdleTime                  => baseIdleTime;
    public float SocialRepelStrength           => socialRepelStrength;
    public float SocialAttractStrength         => socialAttractStrength;
    public IReadOnlyList<JobName> JobsByPreference       => jobsByPreference;
    public IReadOnlyList<RoomName> PossibleIdleRooms     => possibleIdleRooms;
    public IReadOnlyList<CharacterName> LikedCharacters    => likedCharacters;
    public IReadOnlyList<CharacterName> DislikedCharacters => dislikedCharacters;
}
