using EasyGrid.Models;
using System.IO;
using System.Threading;

namespace EasyGrid.Providers
{
    public class ResultProvider
    {
        public ResultModel GetResult(int width, int height, string path, GridParametersModel gridParameters, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;

            return new ResultModel()
            {
                FilePath = path,
                SquareSize = gridParameters.SquareSize,
                NumberSquaresWidth = width,
                NumberSquaresHeight = height,
                GridWidth = width * gridParameters.SquareSize,
                GridHeight = height * gridParameters.SquareSize,
                FileType = "gpx",
                FileSize = GetFileSize(path)
            };
        }

        private static double GetFileSize(string path)
        {
            using var file = File.OpenRead(path);
            double fileSizeInBytes = file.Length;
            var fileSizeInKb = fileSizeInBytes / 1024;
            var fileSizeInMb = fileSizeInKb / 1024;
            return fileSizeInMb;
        }
    }
}
