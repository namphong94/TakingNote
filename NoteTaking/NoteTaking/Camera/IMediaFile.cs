using System;
using System.IO;


namespace Camera_Test.Media
{
    
    public interface IMediaFile : IDisposable {

        string Path { get; }
        Stream GetStream();
    }
}
