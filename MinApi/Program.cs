using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = builder.Environment.ApplicationName, Version = "v1" });
});
builder.Services.AddSingleton<PostStore>();

var app = builder.Build();
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v0");
    c.RoutePrefix = "";
});

app.Use(Middleware);
app.UseDeveloperExceptionPage();




app.MapGet("/posts", (PostStore postStore) =>
{
    var posts = postStore.All();
    return posts;
});

app.MapGet("/posts/{id}", (HttpContext context, int id, PostStore postStore) =>
{
    var post = postStore.Get(id);
    if (post == null)
    {
        context.Response.StatusCode = 404;
        return null;
    }
    return post;
});

app.MapPost("/posts", async (PostEntry post, PostStore postStore) =>
{
    postStore.Add(post);
    return;
});

app.MapPut("/posts/{id}", async (HttpContext context, int id, PostEntry post, PostStore postStore) =>
{
    if (!postStore.Any(id))
    {
        context.Response.StatusCode = 404;
    }
    postStore.Update(post);
});

app.MapDelete("/posts/{id}", async (HttpContext context, int id, PostStore postStore) =>
{
    var sampleHeader = context.Request.Headers["sample-header-key"];
    var sampleItem = context.Items["item-key"];

    if (!postStore.Any(id))
    {
        context.Response.StatusCode = 404;
    }
    postStore.Delete(id);
});

app.Run("http://0.0.0.0");


async Task Middleware(HttpContext context, Func<Task> handler)
{
    // not used
    context.Request.Headers.Add("sample-header-key", "sample-header-value");
    context.Items.Add("item-key", "item-value"); ;


    Stopwatch sw = new Stopwatch();
    sw.Start();
    await handler();
    sw.Stop();
    Console.WriteLine("Response Took {0} ms", sw.ElapsedMilliseconds);
}