using Restic;
//Environment.SetEnvironmentVariable("RESTIC_PASSWORD", "123");

ResticClient restic = new ResticClient();
Repository repository = new Repository(restic, "/app/test-repo123");

Console.WriteLine(restic.GetVersion());
Console.WriteLine(repository.Initialize());

var backup = repository.Backup("/app/test-repo123")
    .ExcludeFile("Program.cs")
    .ExcludeFile("*.cs")
    .ExcludeFile("appsettings.json")
    .ExcludeLargerThan("128")
    .Execute();

Console.WriteLine(backup.Success);
Console.WriteLine(backup.Output);
Console.WriteLine(backup.ErrorOutput);