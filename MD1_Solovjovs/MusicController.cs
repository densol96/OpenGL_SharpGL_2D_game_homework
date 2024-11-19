using System;
using System.Media;
using System.Windows.Controls;


namespace MD1_Solovjovs
{
    public class MusicController
    {
        private static MediaElement mediaElementForLoop;

        public static void PlaySound(string filePath)
        {
            MediaElement mediaElement = new MediaElement
            {
                Source = new Uri(filePath, UriKind.RelativeOrAbsolute),
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Manual
            };

            mediaElement.MediaEnded += (s, e) =>
            {
               mediaElement.Stop();
            };
            mediaElement.Play();
        }

        public static void Loop(string filepath)
        {
            mediaElementForLoop = new MediaElement
            {
                Source = new Uri(filepath, UriKind.RelativeOrAbsolute),
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Manual,
                Volume = 0.5
            };

            mediaElementForLoop.MediaEnded += (s, e) =>
            {
                mediaElementForLoop.Stop();
                mediaElementForLoop.Position = TimeSpan.FromMilliseconds(1);
                mediaElementForLoop.Play();

            };
            mediaElementForLoop.Play();
        }
    }
}
