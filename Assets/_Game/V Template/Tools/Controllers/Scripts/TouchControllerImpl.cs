using System.Collections;


namespace MWM.PrototypeTemplate
{
    public abstract class TouchControllerImpl : ITouchController
    {
        public event TouchEvent OnTouchStart;
        public event TouchEvent OnTouching;
        public event TouchEvent OnTouchEnd;
    
        private bool _touching;



        protected abstract void DoOnTouchStart();
        protected abstract void DoOnTouching();
        protected abstract void DoOnTouchEnd();
    
        
        
        protected virtual IEnumerator UpdateCoroutine ()
        {
            while (true)
            {
                if (InputWrapper.NonUiTouchStart)
                {
                    _touching = true;
                    DoOnTouchStart();
                }
                
                if (_touching)
                {
                    DoOnTouching();
                }
    
                if (InputWrapper.TouchEnd && _touching)
                {
                    _touching = false;
                    DoOnTouchEnd();
                }
                
                yield return null;
            }
        }

        protected void CallOnTouchStart ()
        {
            OnTouchStart?.Invoke();
        }

        protected void CallOnTouching ()
        {
            OnTouching?.Invoke();
        }

        protected void CallOnTouchEnd ()
        {
            OnTouchEnd?.Invoke();
        }
    }
}