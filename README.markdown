NES v0.2
======================================================================

NES (.NET Event Sourcing) is a lightweight framework that helps you build domain models when you're doing event sourcing.

### What
If you're looking to take advantage of event sourcing on the .NET platform then you may also be looking to use [NServiceBus](http://www.nservicebus.com) and Jonathan Oliver's [EventStore](https://github.com/joliver/eventstore). NES is an attempt to fill in the gaps between these two frameworks.

### Why
The code required to bring NServiceBus and the EventStore together isn't hard to write but it is important and it does have to work a certain way to take full advantage of both frameworks. There's some great resources out there already that help you write this yourself but ideally you'd want to concentrate on developing your domain model's behaviour rather than the infrastructure to glue these two frameworks together.

### How
NES hooks into NServiceBus' and the EventStore's configuration objects and transparently takes care of everything for you. All you have to do is add two extra lines to your application's initialisation code.

## Project Goals
* Allow processing of messages sent in a batch as a single transaction ([Ref](http://www.udidahan.com/2008/03/30/nservicebus-explanations-3/))
* Allow use of interfaces for events ([Ref](http://www.nservicebus.com/MessagesAsInterfaces.aspx))
* Allow upconversion of events ([Ref](http://distributedpodcast.com/2011/episode-5-cqrs-eventstore-best-frameworklibrary-ever) @ 36:00)
* Automatic publishing of persisted events
* Convention based event handling within aggregates
* No 'Save()' methods on repositories
* Transparent configuration.

## Building
You can install NES via [NuGet](http://nuget.org/List/Packages/NES) or download the [binaries](http://github.com/elliotritchie/NES/downloads) or download the source and run 'build.bat' from the command line. Once built, the files will be placed in the 'build' folder. NES is targeted for .NET v4.0 and references the following assemblies:

* NServiceBus [v2.5.0.1446 Community Edition](http://www.nservicebus.com/downloads/Community.NServiceBus.2.5.0.1446.zip)
* EventStore [v2.0.11126.34](http://github.com/downloads/joliver/EventStore/EventStore-2.0.11126.34-net40.zip)

At the time of writing these are the recommended versions of these frameworks to use. Currently, if you require a build for a different minor version of these assemblies you should replace their corresponding dlls in the 'lib' folder, re-reference them from the solution and re-run 'build.bat'.

## Using NES

	```C#
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
	{
		public void Init()
		{
			EventStore.Wireup.Init()
				.UsingInMemoryPersistence()
				.NES()
				.Build();

			NServiceBus.Configure.With()
				.Log4Net()
				.DefaultBuilder()
				.XmlSerializer()
				.NES();
		}
	}
	```

For a more complete example, please open and build NES.Sample.sln in Visual Studio and hit F5. This will start the [NES.Sample](https://github.com/elliotritchie/NES/tree/master/sample/NES.Sample) NServiceBus endpoint aswell as the [NES.Sample.Web](https://github.com/elliotritchie/NES/tree/master/sample/NES.Sample.Web) MVC 3 website.