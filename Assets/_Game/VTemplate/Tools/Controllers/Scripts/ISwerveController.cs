namespace MWM.PrototypeTemplate
{
    public delegate void SwerveEvent(float swerveInput);
    
    
    public interface ISwerveController : ITouchController
    {
        /// <summary>
        /// The change in the x position of a touch as a percentage of the screen width
        /// </summary>
        event SwerveEvent OnSwerveInputReceived;
    }
}