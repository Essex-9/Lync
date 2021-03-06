﻿/* Copyright (C) 2012 Modality Systems - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the Microsoft Public License, a copy of which 
 * can be seen at: http://www.microsoft.com/en-us/openness/licenses.aspx
 * 
 * http://www.LyncAutoAnswer.com
*/

using Lync.Enum;
using Lync.Model;
using Microsoft.Lync.Model.Conversation.AudioVideo;
using System;

namespace Lync.EventArg
{
    public class VideoAvailabilityChangedEventArgs : EventArgs
    {
        public VideoDirection Direction { get; private set; }
        public bool IsAvailable { get; private set; }
        public VideoWindow VideoWindow { get; private set; }        

        private VideoAvailabilityChangedEventArgs() { }
        internal VideoAvailabilityChangedEventArgs(VideoWindow videoWindow, VideoDirection direction, bool isAvailable)
        {
            VideoWindow = videoWindow;
            Direction = direction;
            IsAvailable = isAvailable;
        }
    }
}