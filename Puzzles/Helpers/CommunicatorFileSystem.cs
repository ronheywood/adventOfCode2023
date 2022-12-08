using NUnit.Framework.Constraints;

namespace TestProject1.Helpers;

public class CommunicatorFileSystemDirectory : ICommunicatorFileSystemItem
{
    public CommunicatorFileSystemDirectory()
    {
        Name = "";
    }
    public string Name { get; set; }

    public int Size { get; set; }
}

public class CommunicatorFileSystemFile : ICommunicatorFileSystemItem
{
    public CommunicatorFileSystemFile()
    {
        Name = "";
        Directory = "";
    }
    public string Name { get; set; }

    public int Size { get; set; }

    public string Directory { get; set; }
}

public class CommunicatorFileSystemCommand
{
    protected CommunicatorFileSystemCommand()
    {
        Arguments = Array.Empty<string>();
    }
    public IEnumerable<string> Arguments { get; set; }
}

public class ListDirectoryCommand : CommunicatorFileSystemCommand, ICommunicatorFileSystemCommand
{
}

public class ChangeDirectoryCommand : CommunicatorFileSystemCommand, ICommunicatorFileSystemCommand
{
}

public class CommunicatorFileSystem
{
    public readonly int TotalSpace = 70000000;
    private IList<string> _currentDirectoryStack;
    private readonly IList<CommunicatorFileSystemFile> _files;
    private readonly IList<CommunicatorFileSystemDirectory> _directories;

    public CommunicatorFileSystem()
    {
        _files = new List<CommunicatorFileSystemFile>();
        _directories  = new List<CommunicatorFileSystemDirectory>();
        _currentDirectoryStack = new List<string>{"/"};
    }
    
    public static InstructionType InstructionType(string line)
    {
        if (line == string.Empty) return Helpers.InstructionType.None;
        return (line.StartsWith("$")) ? Helpers.InstructionType.Command : Helpers.InstructionType.Response;
    }

    public ICommunicatorFileSystemCommand Command(string line)
    {
        var commandParts = line.Split(" ");
        var commandText = commandParts.Skip(1).First();
        var args = commandParts.Skip(2);
        var command = MakeCommand(commandText, args);
        if (command is ChangeDirectoryCommand) ChangeDirectory(command.Arguments);
        return command;
    }

    private void ChangeDirectory(IEnumerable<string> commandArguments)
    {
        var targetDirectory = commandArguments.First();
        switch (targetDirectory)
        {
            case "..":
                if(_currentDirectoryStack.Count >1)
                    _currentDirectoryStack.RemoveAt(_currentDirectoryStack.Count - 1);
                break;
            case "/":
                _currentDirectoryStack = new List<string>{"/"};
                break;
            default:
                _currentDirectoryStack.Add(targetDirectory);
                break;
        }
    }
    

    private ICommunicatorFileSystemCommand MakeCommand(string commandText, IEnumerable<string> args)
    {
        return (commandText == "ls")
            ? new ListDirectoryCommand() { Arguments = args }
            : new ChangeDirectoryCommand() { Arguments = args };
    }

    public ICommunicatorFileSystemItem Output(string line)
    {
        var output = line.Split(" ");
        var systemItem = MakeFileSystemItem(output);
        if(systemItem is CommunicatorFileSystemFile file) _files.Add(file);
        if(systemItem is CommunicatorFileSystemDirectory directory) _directories.Add(directory);
        return systemItem;
    }

    private ICommunicatorFileSystemItem MakeFileSystemItem(IEnumerable<string> output)
    {
        var enumerable = output as string[] ?? output.ToArray();
        return (enumerable.First() == "dir")
            ? new CommunicatorFileSystemDirectory() { Name = MakeDirName(enumerable.Last()) }
            : new CommunicatorFileSystemFile() { Name = enumerable.Last(), Size = int.Parse(enumerable.First()), Directory = CurrentDirectory };
    }

    private string MakeDirName(string dirName)
    {
        if (dirName == "/") return dirName;
        if (CurrentDirectory == "/") return "/" + dirName;
        return CurrentDirectory + "/" + dirName;
    }

    public string CurrentDirectory => _currentDirectoryStack.Count == 1 ? _currentDirectoryStack.First() : string.Join("/",_currentDirectoryStack).Substring(1);

    public int TotalFileSize => _files.Sum(f => f.Size);

    public int DirectoryFileSize(string directoryName)
    {
        return _files.Where(f => f.Directory.StartsWith(directoryName)).Sum(f => f.Size);
    }

    public void ProcessInput(IEnumerable<string> inputLines)
    {
        foreach (var line in inputLines)
        {
            if (InstructionType(line) == Helpers.InstructionType.Command) Command(line);
            if (InstructionType(line) == Helpers.InstructionType.Response) Output(line);
        }
    }

    public IEnumerable<CommunicatorFileSystemDirectory> DirectoriesWithTotalSizeUpTo(int maxFileSize)
    {
        return _directories.Where(d => DirectoryFileSize(d.Name) <= maxFileSize);
    }

    public int FreeSpace => TotalSpace - TotalFileSize;

    public IEnumerable<CommunicatorFileSystemDirectory> GetDirectoryToDelete()
    {
        return _directories
            .Select(d => { d.Size = DirectoryFileSize(d.Name);
                return d;
            })
            .Where(d => d.Size >= SpaceToFreeToApplyUpdate )
            .OrderBy(d => d.Size);
    }

    public int SpaceToFreeToApplyUpdate => 30000000 - FreeSpace;
}

public interface ICommunicatorFileSystemItem
{
    public string Name { get; set; }
}

public enum InstructionType
{
    None = 0,
    Command = 1,
    Response = 2,
}

public interface ICommunicatorFileSystemCommand
{
    IEnumerable<string> Arguments { get; set; }
}