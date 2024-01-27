using UnityEngine;


namespace VTemplate.UI
{
    public class TutoTrailUI : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _outlineTrail;
        [SerializeField] private TrailRenderer _trail;



        public void Show ()
        {
            _outlineTrail.Clear();
            _trail.Clear();
            
            _outlineTrail.enabled = true;
            _trail.enabled = true;
        }

        public void Hide ()
        {
            _outlineTrail.Clear();
            _trail.Clear();
            
            _outlineTrail.enabled = false;
            _trail.enabled = false;
            
        }
    }
}