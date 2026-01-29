using App;

var cts = new CancellationTokenSource();
FileInfo? outputFile = null;

Console.CancelKeyPress += (sender, e) =>
{
    Console.WriteLine("\nОтмена операции");
    e.Cancel = true;
    cts.Cancel();
};

var uris = Input.GetUris();
outputFile = Input.GetOutputFile();

try
{
    using var writer = new StreamWriter(outputFile.FullName, append: false);
    var writeLock = new object();
    var totalLines = 0;
    var tasks = new List<Task>();

    foreach (var uri in uris)
    {
        var task = Task.Run(async () =>
        {
            try
            {
                using var http = new HttpClient();
                var stream = await http.GetStreamAsync(uri, cts.Token);
                using var reader = new StreamReader(stream);

                var linesCount = 0;
                while (true)
                {
                    var line = await reader.ReadLineAsync(cts.Token);
                    if (line == null) break;
                    lock (writeLock)
                    {
                        writer.WriteLine(line);
                        linesCount++;
                        totalLines++;
                    }
                }

                Console.WriteLine($"Обработан {uri} {linesCount} строк");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке {uri} {ex.Message}");
            }
        }, cts.Token);
        tasks.Add(task);
    }

    await Task.WhenAll(tasks);
    Console.WriteLine($"\nВсего строк {totalLines}");
}
catch (OperationCanceledException)
{
    Console.WriteLine("Операция отменена");
    if (outputFile?.Exists == true)
    {
        try
        {
            File.Delete(outputFile.FullName);
            Console.WriteLine("файл удалён");
        }
        catch { }
    }
}