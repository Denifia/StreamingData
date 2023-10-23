// give the webapi time to start
using System.Diagnostics;

await Task.Delay(2000);

var httpClient = new HttpClient()
{
    BaseAddress = new Uri("http://localhost:5224")
};

while (true)
{
    try
    {
        Console.Write("sending data...");

        // Files over 30,000,000 bytes are rejected by kestrel by default
        Debug.Assert(File.Exists("25000000B.txt"));
        using var stream = File.OpenRead("25000000B.txt");

        var message = new HttpRequestMessage(HttpMethod.Post, "fileupload")
        {
            Content = new StreamContent(stream)
        };
        message.Content.Headers.Add("Content-Type", "application/octet-stream");
        message.Content.Headers.Add("Content-Length", stream.Length.ToString());

        await httpClient.SendAsync(message);

        Console.WriteLine("done.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("error: " + ex.Message);
        Debug.Fail(ex.Message);
    }

    Console.ReadKey();
    Console.SetCursorPosition(0, Console.CursorTop);
}