using System;
using System.Threading;
using System.Threading.Tasks;

internal static class UILoggerHelpers
{
    public static Action Debounce(this Action action, int milliseconds = 300)
    {
        CancellationTokenSource lastCToken = null;

        return () =>
        {
            //Cancel/dispose previous
            lastCToken?.Cancel();
            try
            {
                lastCToken?.Dispose();
            }
            catch { }

            var tokenSrc = lastCToken = new CancellationTokenSource();

            Task.Delay(milliseconds).ContinueWith(task => { action(); }, tokenSrc.Token);
        };
    }
}