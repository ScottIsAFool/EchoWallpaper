﻿using Cimbalino.Toolkit.Services;

namespace EchoWallpaper.WindowsPhone.Silverlight.Empty
{
    public class EmptySettingsService : IApplicationSettingsService
    {
        public IApplicationSettingsServiceHandler Local { get; private set; }
        public IApplicationSettingsServiceHandler Roaming { get; private set; }
        public IApplicationSettingsServiceHandler Legacy { get; private set; }
    }
}