using System;
using System.Collections.Generic;
using Cimbalino.Toolkit.Services;

namespace EchoWallpaper.Core.Empty
{
    public class EmptyNavigationService : INavigationService
    {
        public bool Navigate(string source)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Uri source)
        {
            throw new NotImplementedException();
        }

        public bool Navigate<T>()
        {
            throw new NotImplementedException();
        }

        public bool Navigate<T>(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type type)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type type, object parameter)
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public bool RemoveBackEntry()
        {
            throw new NotImplementedException();
        }

        public Uri CurrentSource { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> QueryString { get; private set; }
        public object CurrentParameter { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanGoForward { get; private set; }
        public event EventHandler Navigated;
        public event EventHandler<NavigationServiceBackKeyPressedEventArgs> BackKeyPressed;
    }
}
