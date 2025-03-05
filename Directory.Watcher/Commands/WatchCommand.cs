using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Directory.Watcher.Commands;

public class WatchCommand : Command<WatchCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<watch-directory>")]
        [Description("Directory to watch for new .ZIP files. Attempts to watch directories defined by the 'DIRECTORY_WATCHER_DIRECTORIES' environment variable if unspecified.")]
        public string? WatchDirectory { get; set; }

        [CommandOption("--auto-extract")]
        [Description("Whether to automatically try to extract the content of the new .ZIP file.")]
        [DefaultValue(false)]
        public bool AutoExtractFiles { get; set; } = false;

        [CommandOption("--overwrite-files")]
        [Description("Whether to automatically overwrite files in the destination directory if any conflicts occur.")]
        [DefaultValue(false)]
        public bool AutoOverwriteFiles { get; set; } = false;

        [CommandOption("--auto-extract-directory")]
        [Description("Directory to watch for new .ZIP files; otherwise, attempts to watch directories defined by the 'DIRECTORY_WATCHER_DIRECTORIES' environment variable.")]
        public string? AutoExtractDirectory { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        List<string> watchDirectories = [];
        if (!string.IsNullOrWhiteSpace(settings.WatchDirectory))
        {
            watchDirectories.Add(settings.WatchDirectory);
        }
        else
        {
            string? defaultWatchDirectories = Environment.GetEnvironmentVariable("DIRECTORY_WATCHER_DIRECTORIES");
            if (string.IsNullOrWhiteSpace(defaultWatchDirectories))
            {
                AnsiConsole.WriteLine("");
                return -1;
            }

            watchDirectories = defaultWatchDirectories.Split(';').ToList();
        }

        AnsiConsole.WriteLine($"Starting ZIP Watcher...");
        var watcherService = new FileService(
            watchDirectories,
            settings
            );
        watcherService.StartWatching();

        AnsiConsole.WriteLine("(Press CTRL+C to stop)");
        Task.Delay(-1).Wait();
        return 0;
    }

}
