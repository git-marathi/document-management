"# Smart File Organizer

A production-ready Windows desktop application for intelligently organizing files into categorized folders based on file extensions and user-defined rules.

## Features

- **Smart File Organization**: Automatically categorize files based on extensions
- **Custom Categories**: Create and manage custom file categories
- **Background Processing**: Asynchronous file organization with progress tracking
- **Conflict Handling**: Options for handling duplicate files (skip, replace, rename)
- **Preview Mode**: Preview changes before applying
- **Logging System**: Detailed logs with export functionality
- **Undo Feature**: Reverse the last organization operation
- **File Filters**: Exclude hidden files, specific extensions, large files, etc.
- **Drag-and-Drop Support**: Drag folders directly into the app
- **Settings Persistence**: Save user preferences and window settings
- **Modern UI**: Fluent design with light/dark themes

## Requirements

- .NET 8.0 or later
- Windows 10 or later

## Installation

1. Clone the repository
2. Open `SmartFileOrganizer.sln` in Visual Studio 2022
3. Restore NuGet packages
4. Build and run the application

## Usage

1. Select a source folder to organize
2. Optionally select a destination folder
3. Configure categories if needed
4. Click "Organize Files" to start the process
5. Monitor progress and view logs

## Architecture

- **MVVM Pattern**: Clean separation of concerns
- **Dependency Injection**: Using Microsoft.Extensions.DependencyInjection
- **CommunityToolkit.Mvvm**: For MVVM support
- **Material Design**: Modern UI styling

## Project Structure

- `Models/`: Data models
- `ViewModels/`: View logic
- `Views/`: UI components
- `Services/`: Business logic services
- `Helpers/`: Utility classes and converters
- `Config/`: Configuration files
- `Logs/`: Log files
- `Assets/`: Application assets

## Contributing

Please follow the established coding standards and patterns." 
