using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZSCY_Win10.Data.Community;

namespace ZSCY_Win10.Util
{
    public class HotFeedsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BBDDTemplate { get; set; }
        public DataTemplate nBBDDTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(System.Object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                HotFeed h = item as HotFeed;
                if (h != null && h.content.contentbase != null)
                    return nBBDDTemplate;
                else return BBDDTemplate;
            }
            return null;
        }
    }
}