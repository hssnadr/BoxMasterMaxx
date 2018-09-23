using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRI.HitBox.Lang;

namespace CRI.HitBox
{
    public interface IAudioContainer
    {
        StringCommon[] GetAudioPaths();
    }

    public interface IVideoContainer
    {
        StringCommon[] GetVideoPaths();
    }

    public interface IImageContainer
    {
        StringCommon[] GetImagePaths();
    }
}