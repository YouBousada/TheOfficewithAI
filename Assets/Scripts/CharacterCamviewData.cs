using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterCamviewData", menuName = "Scriptable Objects/CharacterCamviewData")]
public class CharacterCamviewData : ScriptableObject
{
    public CharacterName characterName;

    [Header("FreeRoam Renders")]
    public List<Sprite> Layer1_FreeRoamRenders;
    public List<Sprite> Layer2_FreeRoamRenders;
    public List<Sprite> Layer3_FreeRoamRenders;
    public List<Sprite> Layer4_FreeRoamRenders;

    [Header("Idle Renders")]
    public List<Sprite> Layer1_IdleRenders;
    public List<Sprite> Layer2_IdleRenders;
    public List<Sprite> Layer3_IdleRenders;
    public List<Sprite> Layer4_IdleRenders;

    [Header("Running Renders")]    
    public List<Sprite> Layer1_RunningRenders;
    public List<Sprite> Layer2_RunningRenders;
    public List<Sprite> Layer3_RunningRenders;
    public List<Sprite> Layer4_RunningRenders;

    public Sprite GetRandomRender(int layer, Behaviour behaviour)
    {
        switch(behaviour)
        {
            case Behaviour.Encroaching:
            case Behaviour.Leaving:
            case Behaviour.InvisibleRoaming:
            case Behaviour.None:
                switch(layer)
                {
                    case 1:
                        if(Layer1_FreeRoamRenders.Count == 0) return null;
                        int randomIndexLayer1Free = Random.Range(0, Layer1_FreeRoamRenders.Count);
                        return Layer1_FreeRoamRenders[randomIndexLayer1Free];
                    case 2:
                        if(Layer2_FreeRoamRenders.Count == 0) return null;
                        int randomIndexLayer2Free = Random.Range(0, Layer2_FreeRoamRenders.Count);
                        return Layer2_FreeRoamRenders[randomIndexLayer2Free];
                    case 3:
                        if(Layer3_FreeRoamRenders.Count == 0) return null;
                        int randomIndexLayer3Free = Random.Range(0, Layer3_FreeRoamRenders.Count);
                        return Layer3_FreeRoamRenders[randomIndexLayer3Free];
                    case 4:
                        if(Layer4_FreeRoamRenders.Count == 0) return null;
                        int randomIndexLayer4Free = Random.Range(0, Layer4_FreeRoamRenders.Count);
                        return Layer4_FreeRoamRenders[randomIndexLayer4Free];
                    default:
                        Debug.LogWarning("GetRandomRender called with invalid layer: " + layer + "layers must be between 1 and 4.");
                        return null;
                }
            case Behaviour.Running:
                switch(layer)
                {
                    case 1:
                        if(Layer1_RunningRenders.Count == 0) return null;
                        int randomIndexLayer1Run = Random.Range(0, Layer1_RunningRenders.Count);
                        return Layer1_RunningRenders[randomIndexLayer1Run];
                    case 2:
                        if(Layer2_RunningRenders.Count == 0) return null;
                        int randomIndexLayer2Run = Random.Range(0, Layer2_RunningRenders.Count);
                        return Layer2_RunningRenders[randomIndexLayer2Run];
                    case 3:
                        if(Layer3_RunningRenders.Count == 0) return null;
                        int randomIndexLayer3Run = Random.Range(0, Layer3_RunningRenders.Count);
                        return Layer3_RunningRenders[randomIndexLayer3Run];
                    case 4:
                        if(Layer4_RunningRenders.Count == 0) return null;
                        int randomIndexLayer4Run = Random.Range(0, Layer4_RunningRenders.Count);
                        return Layer4_RunningRenders[randomIndexLayer4Run];
                    default:
                        Debug.LogWarning("GetRandomRender called with invalid layer: " + layer + "layers must be between 1 and 4.");
                        return null;
                }
            case Behaviour.Idling:
                switch(layer)
                {
                    case 1:
                        if(Layer1_IdleRenders.Count == 0) return null;
                        int randomIndexLayer1Idle = Random.Range(0, Layer1_IdleRenders.Count);
                        return Layer1_IdleRenders[randomIndexLayer1Idle];
                    case 2:
                        if(Layer2_IdleRenders.Count == 0) return null;
                        int randomIndexLayer2Idle = Random.Range(0, Layer2_IdleRenders.Count);
                        return Layer2_IdleRenders[randomIndexLayer2Idle];
                    case 3:
                        if(Layer3_IdleRenders.Count == 0) return null;
                        int randomIndexLayer3Idle = Random.Range(0, Layer3_IdleRenders.Count);
                        return Layer3_IdleRenders[randomIndexLayer3Idle];
                    case 4:
                        if(Layer4_IdleRenders.Count == 0) return null;
                        int randomIndexLayer4Idle = Random.Range(0, Layer4_IdleRenders.Count);
                        return Layer4_IdleRenders[randomIndexLayer4Idle];
                    default:
                        Debug.LogWarning("GetRandomRender called with invalid layer: " + layer + "layers must be between 1 and 4.");
                        return null;
                }
            default:
                Debug.LogWarning("GetRandomRender called with invalid behaviour: " + behaviour);
                return null;
        }
        
    }
}

