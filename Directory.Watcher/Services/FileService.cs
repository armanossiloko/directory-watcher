using Spectre.Console;
using System.IO.Compression;

namespace Directory.Watcher.Services;

public class FileService
{
    private readonly IEnumerable<string> _directories;
    private readonly WatchCommand.Settings _settings;

    public FileService(
        IEnumerable<string> directories,
        WatchCommand.Settings settings
        )
    {
        _directories = directories;
        _settings = settings;
    }

    public void StartWatching()
    {
        foreach (var dir in _directories)
        {
            var di = new DirectoryInfo(dir);
            if (!di.Exists)
            {
                AnsiConsole.MarkupLine($"[red]Directory not found:[/] {dir}");
                continue;
            }

            var watcher = new FileSystemWatcher(di.FullName, "*.zip")
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            watcher.Created += OnNewZipDetected;

            AnsiConsole.MarkupLine($"[green]Watching directory[/] {di.FullName}");
        }
    }

    private void OnNewZipDetected(object sender, FileSystemEventArgs e)
    {
        AnsiConsole.MarkupLine($"A new ZIP file detected: {e.FullPath}");

        bool extractFiles = _settings.AutoExtractFiles;
        if (!extractFiles)
        {
            extractFiles = GetUserBoolInput($"Extract content of the ZIP? [y/N] ");

            if (!extractFiles)
            {
                AnsiConsole.MarkupLine($"File {e.FullPath} extraction has been skipped.");
                AnsiConsole.MarkupLine("Directory.Watcher continues to watch...");
                return;
            }
        }

        // Give some breathing time, just in case file is not yet ready. This might be needed for some systems.
        Thread.Sleep(AppConstants.ActionDelayTimeSpan);

        string extractPath;
        if (string.IsNullOrWhiteSpace(_settings.AutoExtractDirectory))
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            extractPath = GetUserStringInput($"What directory should the ZIP content be extracted to (leave empty for {currentDirectory})");

            if (string.IsNullOrWhiteSpace(extractPath))
            {
                extractPath = currentDirectory;
            }
        }
        else
        {
            extractPath = _settings.AutoExtractDirectory;
        }

        AnsiConsole.WriteLine($"Extracting {e.Name} files to {extractPath}");

        try
        {
            if (_settings.AutoOverwriteFiles)
            {
                ZipFile.ExtractToDirectory(e.FullPath, extractPath, _settings.AutoOverwriteFiles);
                return;
            }

            using var archive = ZipFile.OpenRead(e.FullPath);
            foreach (var entry in archive.Entries)
            {
                var overwrite = false;
                var destinationFile = new FileInfo(Path.Combine(extractPath, entry.FullName));

                if (destinationFile.Exists)
                {
                    overwrite = GetUserBoolInput($"File {destinationFile.FullName} already exists. Overwrite? [y/N] ");
                }

                if (!destinationFile.Directory!.Exists)
                {
                    destinationFile.Directory.Create();
                }

                if (string.IsNullOrEmpty(destinationFile.Name))
                {
                    // ZIP entry is a directory, already created above.
                    continue;
                }

                entry.ExtractToFile(destinationFile.FullName, overwrite);
            }
            AnsiConsole.MarkupLine($"[green]Extraction has been completed![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]An exception occurred during the extraction of {e.Name}.[/] {ex.Message}");
            AnsiConsole.WriteException(ex);
        }
    }

    private static bool GetUserBoolInput(string message)
    {
        AnsiConsole.Write(message);
        var input = Console.ReadLine() ?? "";
        bool @value = input.Equals("Y", StringComparison.OrdinalIgnoreCase);
        return @value;
    }

    private static string GetUserStringInput(string message)
    {
        AnsiConsole.Write(message);
        var input = Console.ReadLine() ?? "";
        return input;
    }

}
