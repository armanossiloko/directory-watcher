global using Directory.Watcher.Commands;
global using Directory.Watcher.Services;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.SetApplicationName("Directory.Watcher");
    config.ValidateExamples();

    config.AddCommand<WatchCommand>("watch")
        .WithDescription(@"Starts watching a directory (or directories specified by the 'DIRECTORY_WATCHER_DIRECTORIES' environment variable) for new .ZIP files.")
        .WithExample("watch")
        .WithExample("watch", "/home/user/downloads")
        .WithExample("watch", ".");
});

return app.Run(args);