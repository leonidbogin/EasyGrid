using System;

namespace EasyGrid.Core.Services
{
    public class ProgressService
    {
        public event Action<int> ProgressChanged;
        public event Action<string> StatusChanged;

        public double ProgressStep { get; set; }

        private double progress;
        private int percent;

        protected void LogProgress(int value)
        {
            ProgressChanged?.Invoke(value);
        }

        protected void LogAddStep()
        {
            LogAddStep(ProgressStep);
        }

        protected void LogAddStep(double step)
        {
            progress += step;

            if (percent != 0 && (int)progress <= percent) return;

            percent = (int)progress;
            ProgressChanged?.Invoke((int)progress);
        }

        protected void LogStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }
    }
}
