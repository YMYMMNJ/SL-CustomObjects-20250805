using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioPlayerComponent : MERToolboxBlocks.Block
{
    [Tooltip("The path to the audio file on your server. Example: .config/SCP Secret Labratory/Labapi/configs/Audio/Example.ogg")]
    public string AudioPath;

    [Range(0f, 100f)]
    [Tooltip("The volume of the AudioPlayer.")]
    public float Volume;
    
    [Tooltip("The max distance the audio will be able to be heard from.")]
    public float AudibleDistance;

    [Tooltip("Whether or not the audio will loop once it ends")]
    public bool Loop;

    public override void Compile(Transform rootTransform)
    {
        this.Properties = new Dictionary<string, object>
		{
			{ "AudioPath", AudioPath },
            { "Volume", Volume },
			{ "AudibleDistance", AudibleDistance },
            { "Loop", Loop },
		};

        base.Compile(rootTransform);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, AudibleDistance);
    }
}