using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/foo",
                 () =>
                 {
                     Foo foo = new Foo1(new Bar1());
                     return TypedResults.Ok(foo);
                 })
         .WithName("GetFoo");

if (args is not [])
{
    app.MapGet("/bar",
               () =>
               {
                   Bar bar = new Bar1();
                   return TypedResults.Ok(bar);
               })
       .WithName("GetBar");
}

app.Run();

[JsonDerivedType(typeof(Foo1), "foo1")]
[JsonDerivedType(typeof(Foo2), "foo2")]
public abstract record Foo(Bar Bar);
public sealed record Foo1(Bar Bar, string Haha = "boo") : Foo(Bar);
public sealed record Foo2(Bar Bar, int Musketeer = 3) : Foo(Bar);

[JsonDerivedType(typeof(Bar1), "bar1")]
[JsonDerivedType(typeof(Bar2), "bar2")]
public abstract record Bar;
public sealed record Bar1(string Hi = "Hi") : Bar;
public sealed record Bar2(Bar Left, Bar Right) : Bar;