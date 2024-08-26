using Restic;
Environment.SetEnvironmentVariable("RESTIC_PASSWORD", "123");

ResticClient restic = new ResticClient();
Repository repository = new Repository(restic, "/app/test-repo123")
    .WithPasswordInEnvironment();

Console.WriteLine("Starting...");
var resticVersion = await restic.GetVersion();
Console.WriteLine(resticVersion);
//Console.WriteLine(repository.Initialize());
/*
var backup = await repository.Backup("/app/test-repo123")
    .ExcludeFile("Program.cs")
    .ExcludeFile("*.cs")
    .ExcludeFile("appsettings.json")
    .ExcludeLargerThan("128")
    .Execute();
*/

//Console.WriteLine(backup.Success);
//Console.WriteLine(backup.Output);
//Console.WriteLine(backup.ErrorOutput);