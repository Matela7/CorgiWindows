using System;

namespace CorgiWindows
{
    public class FocusSession
    {
        private readonly WebsiteBlockerService _blockerService;

        public FocusSession(WebsiteBlockerModel model, bool isActive = false)
        {
            _blockerService = new WebsiteBlockerService(model);
            IsActive = isActive;
            
            if (IsActive)
            {
                _blockerService.RunThread();
            }
        }

        public bool IsActive { get; private set; }

        public void StartStop()
        {
            IsActive = !IsActive;
            
            if (IsActive)
            {
                _blockerService.RunThread();
            }
            else
            {
                _blockerService.StopThread();
            }
        }
    }
}
