<p align="center">
  <a title="NuGet download" target="_blank" href="https://www.nuget.org/packages/Directory.Watcher"><img alt="NuGet" src="https://img.shields.io/nuget/v/Directory.Watcher"></a>
  <img alt="GitHub last commit" src="https://img.shields.io/github/last-commit/armanossiloko/directory-watcher">
  <img alt="GitHub repo size" src="https://img.shields.io/github/repo-size/armanossiloko/directory-watcher">
  <a title="MIT License" target="_blank" href="https://licenses.nuget.org/MIT"><img src="https://img.shields.io/github/license/armanossiloko/directory-watcher"></a>
</p>

# Directory Watcher

## Backstory

When using Brave browser with [bolt.diy](https://github.com/stackblitz-labs/bolt.diy), the `Sync files` button would fail basically 100% of the time. The reason for that appears to be that Brave has File System API access blocked altogether and there is no way to have a workaround. I was not so keen to switch to another browser just so that I can use `bolt.diy` myself (though, fully blocking the feature in Brave is still not the correct thing to do, in my opinion).

Nonetheless, using the `Download as ZIP` in `bolt.diy` works just fine. So, my solution was to come up with a simple tool that would monitor a certain directory for new ZIP files and automatically extract them. The product - Directory Watcher!

This is a simple CLI tool that watches specified directories for new ZIP files and tries to automatically extract them. For my use case, run `bolt.diy` and download the current state of a project as a ZIP into a specific directory and Directory Watcher will try to automatically extract its content to a specified destination (which in my case was my local repository where I could view any git changes and decide what files to commit and push).


## Features

- Watches and logs new ZIP files in one or multiple directories
- Automatically extracts ZIP files upon their creation
- Allows controlling file overwrite behavior


## Installation

### Prerequisites

- .NET 9.0 or later

### Building from Source

1. Clone the repository
	```bash
	git clone https://github.com/armanossiloko/directory-watcher.git
	```

2. Set your working directory
	```bash
	cd directory-watcher/Directory.Watcher
	```

3. Build and pack the CLI tool
	```bash
	dotnet pack
	```

4. Install it on your local machine
	```
	dotnet tool install --global --add-source ./nupkg Directory.Watcher
	```

### Via NuGet:

```
dotnet tool install --global Directory.Watcher
```

In order to remove, use:
```
dotnet tool uninstall --global Directory.Watcher
```

### Environment Variables

- `DIRECTORY_WATCHER_DIRECTORIES`: Semicolon-separated list of directories to watch when no directory is specified in runtime.

## Examples

```bash
# Retrieve auto-help from the tool
Directory.Watcher watch --help

# Watch a directory defined in `DIRECTORY_WATCHER_DIRECTORIES` env variable
Directory.Watcher watch ""

# Watch a directory with auto-extraction
Directory.Watcher watch /downloads --auto-extract --auto-extract-directory=/projects/my-project

# Set up a fully automated workflow (watch downloads, auto-extract, overwrite)
Directory.Watcher watch /downloads --auto-extract --overwrite-files --auto-extract-directory=/projects/my-project
```


## Behavior

When a new ZIP file is detected, the tool notifies you in the console:

- If `--auto-extract` is set, extraction begins immediately; otherwise, you'll be prompted
- If `--auto-extract-directory` is not set, you'll be prompted for an extraction location
- If `--overwrite-files` is set, existing files will be automatically overwritten; otherwise, you'll be prompted for each conflict


## Credits

This project utilizes the following open-source libraries, please star them on Github and contribute to them, if able to:
- [Spectre.Console](https://github.com/spectreconsole/spectre.console): A .NET library that makes it easier to create beautiful console applications.


## Contributing

Got suggestions, found a bug or would like to contribute? Feel free to create issues or pull requests. Please do make sure to try to write clear and concise commit messages.


## Support
If you find this project helpful, you can support me on [PayPal.me](https://paypal.me/armanossiloko).


## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.