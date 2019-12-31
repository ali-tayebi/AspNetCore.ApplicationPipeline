# HTTP Pipelines

**HttpPipelines** is a package to help ASP.Net Core developers manage HTTP request pipeline programmatically.

ASP.Net Core developers have to hard-code the HTTP request pipeline usually in method `Configure` in class `Startup.cs`.
Unfortunately, it is not flexible enough to let developers add/remove middlewares (steps) to/from the pipeline on-demand.

For instance, changing the pipeline can be very useful in some integration testing scenarios.
Also,  framework developers who need an HTTP request pipelines with a particular order of steps may require supports for some customisations. For example, they may want to let the uses to insert a custom middleware (step) in the middle of their pipeline.

## Getting Started
First, configure the pipeline and add it to dependency injection using `AddHttpPipeline` method.
For instance, the following code shows how to create, configure, and add a `HttpPipeline` to the service collection.
As you can see, the pipeline has six different steps, and each step is created by a **concrete class**, **Expressions**, or **Action**.


```c#
public void ConfigureServices(IServiceCollection services)
{
    //...

    services.AddHttpPipeline(pipeline =>
    {
        pipeline
            .Add<DeveloperExceptionPageStep>()
            .Add(app => app.UseHttpsRedirection())
            .Add("routing-step", app => app.UseRouting())
            .Add<SecurityStep>()
            .Add<EndpointsStep>();
    });

    //...
}
```


Second, use the pipeline using `UsePipeline` method in `Configure` method in `Startup.cs` class as the following:

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseHttpPipeline();
}
```

Finally, steps can be added/removed on-demand. For instance, the following shows how to add step `HttpTransactionRecorderStep`
just before step `EndpointsStep` in an integration test.

For more details, please have a look at the [sample](https://github.com/ali-tayebi/http-pipelines/blob/master/sample/HttpPipelines.WebApi.Sample/Startup.cs)
 and its [test](https://github.com/ali-tayebi/http-pipelines/blob/master/sample/HttpPipelines.WebApi.Sample.Tests/TransactionIdTests.cs) projects.

```c#
public class TransactionIdTests : IClassFixture<WebApplicationFactory<Startup>>
{
   private readonly WebApplicationFactory<Startup> _factory;

   public TransactionIdTests(WebApplicationFactory<Startup> factory)
   {
       _factory = factory.WithWebHostBuilder(host =>
           host.ConfigureTestServices(services =>
           {
               services
                   .AddHttpPipeline(pipeline =>
                   {
                       pipeline.AddBefore<EndpointsStep, HttpTransactionRecorderStep>();
                   });
               //...
           }));
   }

   //...
```
## Prerequisites
.Net Core version **3.1** is only supported version at the time being.


## Documentation
TBA...
