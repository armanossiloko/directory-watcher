# Directory Watcher

## Introduction
A lightweight CLI tool that watches directories for new ZIP files and attempts to automatically extract them based on how it's ran.

## Quick Start
```bash
# Install
dotnet tool install --global Directory.Watcher

# Watch the /downloads directory, auto-extract to a local directory of your choice
Directory.Watcher watch /downloads --auto-extract --overwrite-files --auto-extract-directory=/projects/my-project
```

## Options
- `--auto-extract`: Automatically extract new ZIP files
- `--overwrite-files`: Automatically overwrite existing files, if found
- `--auto-extract-directory`: Defines the extraction destination

Find more info on GitHub: https://github.com/armanossiloko/directory-watcher