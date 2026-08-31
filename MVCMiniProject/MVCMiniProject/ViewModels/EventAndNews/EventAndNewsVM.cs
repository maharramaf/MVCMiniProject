using MVCMiniProject.ViewModels.Events;
using MVCMiniProject.ViewModels.News;

namespace MVCMiniProject.ViewModels.EventAndNews
{
    public class EventAndNewsVM
    {
        public IEnumerable<EventUIVM> Events { get; set; }
        public IEnumerable<NewsUIVM> News { get; set; }

    }
}
