Mezm.Owin.Razor
---------------

Implementation of Razor templating with OWIN

Instalation
-----------

```
Install-Package Mezm.Owin.Razor -Pre
```

Using
-----

```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    appBuilder.UseRazor(InitRoutes);
  }

  private static void InitRoutes(IRouteTable table)
  {
    table
        .AddFileRoute("/", "Views/index.cshtml")
        .AddFileRoute("/about/me", "Views/about.cshtml", new AboutMeModel { Name = "Val" });
  }
}
```
