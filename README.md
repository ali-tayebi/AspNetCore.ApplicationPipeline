# Application Pipeline
<img src="https://github.com/ali-tayebi/AspNetCore.ApplicationPipeline/blob/master/application-pipeline.png?raw=true" alt="ApplicationPipeline Logo">

**AspNetCore.ApplicationPipeline** is a package to help ASP.Net Core developers manage application pipeline for HTTP requests programmatically.

Developers have to hard-code the application pipeline in `Configure` method in `Startup` class.
The pipelines built with this approach stop developers from adding/removing middlewares (steps) to/from the pipeline on-demand.

Changing application pipeline programmatically is required for some testing scenarios such as well as developing frameworks.

## Getting Started
First, configure and add the pipeline to the service collection using `AddApplicationPipeline` method.
For instance, the following code shows how to create, configure, and add a `HttpPipeline` to the `IServiceCollection`.
As you can see, this pipeline has different steps, and each step is created using **concrete classes**, **Expressions**, or **Action**.


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


Second, use the pipeline using `UseApplicationPipeline` method in `Configure` method in `Startup` class as the following:

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseApplicationPipeline();
}
```

Now, you can add/remove pipeline steps  on-demand. For instance, the following shows how to add a custom step named `HttpTransactionRecorderStep`
just before step `EndpointsStep` for integration test.

For more details, please have a look at the [sample](https://github.com/ali-tayebi/AspNetCore.ApplicationPipeline/blob/master/sample/HttpPipelines.WebApi.Sample/Startup.cs)
 and its [test](https://github.com/ali-tayebi/AspNetCore.ApplicationPipeline/blob/master/sample/HttpPipelines.WebApi.Sample.Tests/TransactionIdTests.cs) projects.

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
