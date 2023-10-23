using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient("UseDevelopmentStorage=true", preferMsi: true);
});

var app = builder.Build();

app.MapControllers();

var blobServiceClient = app.Services.GetRequiredService<BlobServiceClient>();
blobServiceClient.GetBlobContainerClient("temp").CreateIfNotExists();

app.Run();
