using App;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cts.Cancel();

var uris = Input.GetUris();
var dest = Input.GetOutputFile();
var destStream = dest.OpenWrite();

await Parallel.ForEachAsync(uris, cts.Token, async (uri, ct) =>
{
    using var http = new HttpClient();
    await using var content = await http.GetStreamAsync(uri, ct);
    await content.CopyToAsync(destStream, ct);
});

await destStream.DisposeAsync();
