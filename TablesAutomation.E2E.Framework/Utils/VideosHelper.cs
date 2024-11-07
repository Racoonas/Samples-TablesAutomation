using Microsoft.Playwright;

namespace TablesAutomation.E2EFramework.Utils
{
    public class VideosHelper
    {
        /// <summary>
        /// Extract paths to the video files from the BrowserContext
        /// </summary>
        /// <param name="BrowserContext"></param>
        /// <returns></returns>
        public static List<string> ExtractVideos(IBrowserContext BrowserContext)
        {
            var videos = new List<string>();

            if (!BrowserContext.Pages.Any()) return videos;

            foreach (var page in BrowserContext.Pages)
            {
                var taskCompleted = page.Video.PathAsync().Wait(1000);
                if (taskCompleted)
                {
                    var path = page.Video.PathAsync().Result;
                    videos.Add(path);
                }
            }

            return videos;
        }

        /// <summary>
        /// Renames video file and return new name
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="testNamePrefix"></param>
        /// <returns></returns>
        public static string AddTestNamePrefix(string sourcePath, string testNamePrefix)
        {
            var directory = Path.GetDirectoryName(sourcePath);
            var filename = Path.GetFileName(sourcePath);
            var newName = $"{testNamePrefix}_{filename}";
            var destinationPath = Path.Combine(directory, newName);
            File.Move(sourcePath, destinationPath);
            return destinationPath;
        }

        /// <summary>
        /// Deletes every video in the list of paths
        /// </summary>
        /// <param name="paths">Paths to be removed</param>
        public static void RemoveVideos(IEnumerable<string> paths)
        {
            foreach (var videoPath in paths)
            {
                File.Delete(videoPath);
            }
        }

    }
}
