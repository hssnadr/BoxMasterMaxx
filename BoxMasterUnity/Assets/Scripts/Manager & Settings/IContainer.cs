using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRI.HitBox.Lang;

namespace CRI.HitBox
{
    public interface IAudioContainer
    {
        StringCommon GetAudioPath();
    }

    public interface IVideoContainer
    {
        StringCommon GetVideoPath();
    }

    public interface IImageContainer
    {
        StringCommon[] GetImagePaths();
    }
}