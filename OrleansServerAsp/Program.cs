using GrainStorage;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseOrleans(siloBuilder => {
	siloBuilder.UseLocalhostClustering();
	siloBuilder.AddFileGrainStorage("File", options => {
		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		options.RootDirectory = Path.Combine(path, "Orleans/GrainState/v1");
	});
});


builder.Services.AddSignalR();


var app = builder.Build();


app.MapHub<GameHub>("/gamehub");
app.MapGet("/", () => "Welcome!");


app.Run();
