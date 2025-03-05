namespace Directory.Watcher;

internal static class AppConstants
{
	private const long _defaultActionDelay = 1;

	public static TimeSpan ActionDelayTimeSpan = TimeSpan.FromSeconds(_defaultActionDelay);
}