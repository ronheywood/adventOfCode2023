namespace TestProject1.Helpers;

public class CommunicatorFileSystemTests
{
    [Test]
    public void An_empty_line_is_not_a_console_type()
    {
        Assert.That(CommunicatorFileSystem.InstructionType(string.Empty), Is.EqualTo(InstructionType.None));
    }

    [Test]
    public void Should_treat_dollar_as_command()
    {
        var line = "$ cd /";
        Assert.That(CommunicatorFileSystem.InstructionType(line), Is.EqualTo(InstructionType.Command));
    }

    [Test]
    public void Should_consider_lines_with_no_dollar_as_output()
    {
        var line = "dir a";
        Assert.That(CommunicatorFileSystem.InstructionType(line), Is.EqualTo(InstructionType.Response));
    }

    [TestCase("$ cd /", "/")]
    [TestCase("$ cd ..", "..")]
    public void Should_identify_change_directory_command_from_command_line(string line, string expectedArgument)
    {
        var communicatorFileSystemCommand = new CommunicatorFileSystem().Command(line);
        Assert.Multiple(() =>
        {
            Assert.That(communicatorFileSystemCommand, Is.InstanceOf<ChangeDirectoryCommand>());
            Assert.That(communicatorFileSystemCommand.Arguments.Count(), Is.EqualTo(1));
            Assert.That(communicatorFileSystemCommand.Arguments.First(), Is.EqualTo(expectedArgument));
        });
    }

    [Test]
    public void Should_identify_list_command_from_command_line()
    {
        var communicatorFileSystemCommand = new CommunicatorFileSystem().Command("$ ls");
        Assert.Multiple(() =>
        {
            Assert.That(communicatorFileSystemCommand, Is.InstanceOf<ListDirectoryCommand>());
            Assert.That(communicatorFileSystemCommand.Arguments, Is.Empty);
        });
    }

    [Test]
    public void Should_identify_output_lines_indicating_directories([Values("a", "b", "c")] string dirName)
    {
        var communicatorFileSystemType = new CommunicatorFileSystem().Output($"dir {dirName}");
        Assert.That(communicatorFileSystemType, Is.InstanceOf<CommunicatorFileSystemDirectory>());
        Assert.That(communicatorFileSystemType.Name, Is.EqualTo("/" + dirName));
    }

    [Test]
    public void Should_identify_output_lines_indicating_directories_after_a_change_directory()
    {
        var communicatorFileSystem = new CommunicatorFileSystem();
        var directory = communicatorFileSystem.Output($"dir a") as CommunicatorFileSystemDirectory;

        Assert.That(directory?.Name, Is.EqualTo("/a"));
        communicatorFileSystem.Command("$ cd a");
        var subDirectory = communicatorFileSystem.Output($"dir b") as CommunicatorFileSystemDirectory;
        Assert.That(subDirectory?.Name, Is.EqualTo("/a/b"));
    }

    [TestCase(14848514, "b.txt")]
    [TestCase(8504156, "c.dat")]
    public void Should_identify_output_lines_indicating_files(int fileSize, string fileName)
    {
        var communicatorFileSystemType = new CommunicatorFileSystem().Output($"{fileSize} {fileName}");
        Assert.That(communicatorFileSystemType, Is.InstanceOf<CommunicatorFileSystemFile>());
        var file = communicatorFileSystemType as CommunicatorFileSystemFile;
        Assert.Multiple(() =>
        {
            Assert.That(file?.Name, Is.EqualTo(fileName));
            Assert.That(file?.Size, Is.EqualTo(fileSize));
            Assert.That(file?.Directory, Is.EqualTo("/"));
        });
    }

    [Test]
    public void Should_identify_output_lines_indicating_files()
    {
        var communicatorFileSystem = new CommunicatorFileSystem();
        var communicatorFileSystemType = communicatorFileSystem.Output($"100 a");
        Assert.That(communicatorFileSystemType, Is.InstanceOf<CommunicatorFileSystemFile>());
        var file = communicatorFileSystemType as CommunicatorFileSystemFile;
        Assert.Multiple(() =>
        {
            Assert.That(file?.Name, Is.EqualTo("a"));
            Assert.That(file?.Size, Is.EqualTo(100));
            Assert.That(file?.Directory, Is.EqualTo("/"));
        });
        
        communicatorFileSystem.Command("$ cd a");
        var file2 = communicatorFileSystem.Output($"100 a") as CommunicatorFileSystemFile;
        Assert.That(file2?.Directory, Is.EqualTo("/a"));
    }

    [Test]
    public void Should_change_directory_to_named_directory_when_command_is_given()
    {
        var communicatorFileSystem = new CommunicatorFileSystem();
        Assert.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/"));
        communicatorFileSystem.Command("$ cd a");
        Assert.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/a"));
        communicatorFileSystem.Command("$ cd b");
        Assume.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/a/b"));
    }

    [Test]
    public void Should_change_directory_to_relative_directory_when_command_is_given()
    {
        var communicatorFileSystem = new CommunicatorFileSystem();
        communicatorFileSystem.Command("$ cd a");
        Assume.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/a"));
        communicatorFileSystem.Command("$ cd ..");
        Assert.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/"));
        communicatorFileSystem.Command("$ cd a");
        communicatorFileSystem.Command("$ cd b");
        Assume.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/a/b"));
        communicatorFileSystem.Command("$ cd ..");
        Assume.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/a"));
    }

    [Test]
    public void Should_not_change_back_when_current_directory_is_root()
    {
        var communicatorFileSystem = new CommunicatorFileSystem();
        communicatorFileSystem.Command("$ cd a");
        communicatorFileSystem.Command("$ cd b");
        Assert.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/a/b"));
        communicatorFileSystem.Command("$ cd /");
        Assert.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/"));
        communicatorFileSystem.Command("$ cd ..");
        Assert.That(communicatorFileSystem.CurrentDirectory, Is.EqualTo("/"));
    }

    [Test]
    public void Should_record_files_in_directories()
    {
        var communicatorFileSystem = new CommunicatorFileSystem();
        communicatorFileSystem.Output("14848514 b.txt");
        Assert.That(communicatorFileSystem.TotalFileSize, Is.EqualTo(14848514));
        communicatorFileSystem.Output("8504156 c.dat");
        Assert.That(communicatorFileSystem.TotalFileSize, Is.EqualTo(14848514 + 8504156));
        communicatorFileSystem.Command("$ cd a");
        communicatorFileSystem.Output("29116 f");
        communicatorFileSystem.Output("2557 g");
        Assert.Multiple(() =>
        {
            Assert.That(communicatorFileSystem.TotalFileSize, Is.EqualTo(14848514 + 8504156 + 29116 + 2557));
            Assert.That(communicatorFileSystem.DirectoryFileSize("/a"), Is.EqualTo(29116 + 2557));
        });
    }

    [Test]
    public void Should_record_files_in_sub_directories()
    {
        var communicatorFileSystem = new CommunicatorFileSystem();
        communicatorFileSystem.Command("$ cd a");
        communicatorFileSystem.Output("14848514 a.txt");
        communicatorFileSystem.Command("$ cd b");
        communicatorFileSystem.Output("29116 f");
        communicatorFileSystem.Command("$ cd ..");
        communicatorFileSystem.Command("$ cd c");
        communicatorFileSystem.Output("5 g");
        Assert.Multiple(() =>
        {
            Assert.That(communicatorFileSystem.DirectoryFileSize("/a/b"), Is.EqualTo(29116));
            Assert.That(communicatorFileSystem.DirectoryFileSize("/a"), Is.EqualTo(14848514 + 29116 + 5));
        });
    }

    [Test]
    public void Directory_names_can_be_duplicated_in_different_branches()
    {
        var communicatorFileSystem = new CommunicatorFileSystem();
        communicatorFileSystem.Command("$ cd /");
        communicatorFileSystem.Command("$ cd a");
        communicatorFileSystem.Output("1 a.txt");
        communicatorFileSystem.Command("$ cd ..");
        communicatorFileSystem.Command("$ cd b");
        communicatorFileSystem.Command("$ cd a");
        communicatorFileSystem.Output("2 ba.txt");

        Assert.That(communicatorFileSystem.DirectoryFileSize("/a"), Is.EqualTo(1));
        Assert.That(communicatorFileSystem.DirectoryFileSize("/b"), Is.EqualTo(2));
        Assert.That(communicatorFileSystem.DirectoryFileSize("/b/a"), Is.EqualTo(2));
    }

    [Test]
    public void Should_process_input()
    {
        var input = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";
        var inputLines = PuzzleInput.InputStringToArray(input);
        var c = new CommunicatorFileSystem();
        c.ProcessInput(inputLines);
        Assert.Multiple(() =>
        {
            Assert.That(c.TotalFileSize, Is.EqualTo(48381165));
            Assert.That(c.DirectoryFileSize("/d"), Is.EqualTo(24933642));
            Assert.That(c.DirectoryFileSize("/a"), Is.EqualTo(94853));
            Assert.That(c.DirectoryFileSize("/a/e"), Is.EqualTo(584));
            var smallDirectories = c.DirectoriesWithTotalSizeUpTo(100000).ToArray();

            Assert.That(smallDirectories, Has.Length.EqualTo(2));
            CollectionAssert.AreEqual(new[] { "/a", "/a/e" }, smallDirectories.Select(d => d.Name));
            Assert.That(smallDirectories.Sum(d => c.DirectoryFileSize(d.Name)), Is.EqualTo(95437));
        });
    }

    [Test]
    public void Should_report_free_space()
    {
        var communicator = MakeSpecificationCommunicator();
        Assert.That(communicator.FreeSpace, Is.EqualTo(21618835));
    }

    [Test]
    public void Should_calculate_space_required_to_apply_update()
    {
        
        var communicator = MakeSpecificationCommunicator();
        Assert.That(communicator.SpaceToFreeToApplyUpdate, Is.EqualTo(8381165));
    }

    [Test]
    public void Should_select_directory_to_delete()
    {
        var communicator = MakeSpecificationCommunicator();
        var directories = communicator.GetDirectoryToDelete();

        Assert.Multiple(() =>
        {
            Assert.That(directories.Count(), Is.EqualTo(1));
            Assert.That(directories.First().Name, Is.EqualTo("/d"));
        });
    }

    private static CommunicatorFileSystem MakeSpecificationCommunicator()
    {
        var input = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";
        var inputLines = PuzzleInput.InputStringToArray(input);
        var communicator = new CommunicatorFileSystem();
        communicator.ProcessInput(inputLines);
        return communicator;
    }

    [Test]
    public void Should_find_smallest_directory_to_remove_for_freespace_target()
    {
    }
}