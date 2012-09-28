//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com

using System;
using System.Windows;
using System.Windows.Markup;

namespace DDLibWPF.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(DDDestination))]
    public sealed class DDDestination : MarkupExtension
    {
        private DDReceiver _receiver;
        private FrameworkElement _element;

        private Type _allowedType = typeof(object); //Default to accecpt everything
        public Type AllowedType
        {
            get { return _allowedType; }
            set
            {
                _allowedType = value;
                SetAllowedTypes();
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        internal void StartOn(FrameworkElement element)
        {
            Stop();
            _element = element;
            _element.DataContextChanged += ElementDataContextChanged;
            _receiver = new DDReceiver(element);
            SetAllowedTypes();
            _receiver.Start();
        }

        internal void Stop()
        {
            if (_element != null)
            {
                _element.DataContextChanged -= ElementDataContextChanged;
                _element = null;
            }
            if (_receiver != null)
                _receiver.Stop();
        }

        private void SetAllowedTypes()
        {
            var receiver = _receiver;
            if (receiver == null)
                return;

            receiver.ClearAllAllowedTypes();

            var element = _element;
            if (element != null)
            {
                var allowedType = AllowedType;
                if (allowedType != null)
                {
                    var dataContext = element.DataContext;
                    if (dataContext != null)
                    {
                        var ddHandler = dataContext as IDDDropHandler;
                        if (ddHandler != null)
                        {
                            receiver.AllowType(allowedType, ddHandler.ItemDropped);
                        }
                    }
                }
            }
        }

        private void ElementDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetAllowedTypes();
        }
    }
}
