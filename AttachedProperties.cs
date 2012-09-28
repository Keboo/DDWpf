//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com

using System.Windows;
using DDLibWPF.MarkupExtensions;
namespace DDLibWPF
{
    public static class AttachedProperties
    {
        #region Drag Source
        public static readonly DependencyProperty DragSourceProperty = DependencyProperty.RegisterAttached(
                "DragSource",
                typeof(DDSource),
                typeof(AttachedProperties), new PropertyMetadata(null, DragSourceChanged));

        public static void SetDragSource(UIElement element, DDSource value)
        {
            element.SetValue(DragSourceProperty, value);
        }

        public static DDSource GetDragSource(UIElement element)
        {
            return (DDSource)element.GetValue(DragSourceProperty);
        }

        private static void DragSourceChanged(DependencyObject @do, DependencyPropertyChangedEventArgs args)
        {
            var element = (UIElement)@do;
            var source = args.OldValue as DDSource;
            if (source != null)
                source.Stop();
            source = args.NewValue as DDSource;
            if (source != null)
                source.StartOn(element);
        }
        #endregion Drag Source

        #region Drag Destination
        public static readonly DependencyProperty DragDestinationProperty = DependencyProperty.RegisterAttached(
              "DragDestination",
              typeof(DDDestination),
              typeof(AttachedProperties), new PropertyMetadata(null, DragDestinationChanged));

        public static void SetDragDestination(UIElement element, DDDestination value)
        {
            element.SetValue(DragDestinationProperty, value);
        }

        public static DDSource GetDragDestination(UIElement element)
        {
            return (DDSource)element.GetValue(DragDestinationProperty);
        }

        private static void DragDestinationChanged(DependencyObject @do, DependencyPropertyChangedEventArgs args)
        {
            var element = (FrameworkElement)@do;
            var destination = args.OldValue as DDDestination;
            if (destination != null)
                destination.Stop();
            destination = args.NewValue as DDDestination;
            if (destination != null)
                destination.StartOn(element);
        }
        #endregion Drag Destination

        #region Drag Object
        public static readonly DependencyProperty DragObjectProperty = DependencyProperty.RegisterAttached(
              "DragObject",
              typeof(object),
              typeof(AttachedProperties), new PropertyMetadata(null));

        public static void SetDragObject(UIElement element, object value)
        {
            element.SetValue(DragObjectProperty, value);
        }

        public static object GetDragObject(UIElement element)
        {
            return element.GetValue(DragObjectProperty);
        }
        #endregion Drag Object
    }
}
