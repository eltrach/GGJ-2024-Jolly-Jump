using UnityEngine;


namespace MWM.PrototypeTemplate
{
    public class ExampleSwerveMovementScript : MonoBehaviour
    {
        [SerializeField] private float _sensitivity = 2;
        [SerializeField] private float _minX = -2;
        [SerializeField] private float _maxX = 2;
        
        private ISwerveController _swerveController;

        
        
        private void Awake ()
        {
            _swerveController = new SwerveControllerImpl(this);
            _swerveController.OnSwerveInputReceived += Move;
            _swerveController.OnTouchStart += () => { Debug.Log("Touch Start"); };
            _swerveController.OnTouching += () => { Debug.Log("Touching"); };
            _swerveController.OnTouchEnd += () => { Debug.Log("Touch End"); };        
        }

        
        
        private void Move (float screenPercentageDelta)
        {
            transform.Translate(new Vector3(screenPercentageDelta * (_maxX - _minX) * _sensitivity, 0, 0));

            if (transform.position.x < _minX)
                transform.Translate(Vector3.right * (_minX - transform.position.x));
            if (transform.position.x > _maxX)
                transform.Translate(Vector3.left * (transform.position.x - _maxX));
        }
    }
}