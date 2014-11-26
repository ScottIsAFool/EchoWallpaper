﻿using System;

namespace EchoWallpaper.Core.Attributes
{
    internal class Description : Attribute
    {
        public string Text;

        public Description(string text)
        {
            Text = text;
        }
    }
}