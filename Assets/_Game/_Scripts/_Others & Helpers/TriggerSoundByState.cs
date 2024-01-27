using System.Collections.Generic;
using UnityEngine;



public class TriggerSoundByState : StateMachineBehaviour
{
    public GameObject audioSource;
    public List<AudioClip> sounds;
    public float triggerTime;
    private FisherYatesRandom _random;
    private bool isTrigger;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isTrigger = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime % 1 >= triggerTime && !isTrigger)
        {
            TriggerSound(animator, stateInfo, layerIndex);
        }
        else if (stateInfo.normalizedTime % 1 < triggerTime && isTrigger) isTrigger = false;
    }

    void TriggerSound(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_random == null)
            _random = new FisherYatesRandom();
        isTrigger = true;
        GameObject audioObject = null;
        if (audioSource != null)
            audioObject = Instantiate(audioSource.gameObject, animator.transform.position, Quaternion.identity) as GameObject;
        else
        {
            audioObject = new GameObject("audioObject");
            audioObject.transform.position = animator.transform.position;
        }
        if (audioObject != null)
        {
            var source = audioObject.GetComponent<AudioSource>();
            var clip = sounds[_random.Next(sounds.Count)];
            source.PlayOneShot(clip);
        }
    }
}
