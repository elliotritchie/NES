NES v0.1
======================================================================

## Overview
NES (.NET Event Sourcing) is a lightweight framework that helps you build domain models when you're doing event sourcing.

### What
If you're looking at doing Event Sourcing on the .NET platform then you may also be looking to use [NServiceBus](http://www.nservicebus.com) and Jonathan Oliver's [EventStore](https://github.com/joliver/eventstore). NES is an attempt to fill in the gaps between these two frameworks.

### Why
The code required to bring NServiceBus and the EventStore together isn't hard to write but it is important and it does have to work a certain way to take full advantage of both frameworks. There's some great resources out there already that help you write this yourself but ideally you'd want to concentrate on developing your domain model's behaviour rather than the infrastructure to glue these two frameworks together.

### How
NES hooks into NServiceBus' and the EventStore's configuration objects and transparently takes care of everything for you. All you have to do is add two extra lines to your application's initialisation code.

## Project Goals
* Transparent configuration.
* Allow processing of messages sent in a batch as a single transaction ([Ref](https://github.com/NServiceBus/NServiceBus/blob/master/src/core/NServiceBus/IBus.cs#L92))
* Allow use of interfaces for events ([Ref](http://www.nservicebus.com/MessagesAsInterfaces.aspx))
* Convention based event handling within aggregates
* No 'Save()' methods on repositories
* Automatic publishing of persisted events

## Building
You can download the v0.1 [binaries](https://github.com/downloads/elliotritchie/NES/NES-v0.1.zip) or download the source and run 'build.bat' from the command line. Once built, the files will be placed in the 'build' folder. NES is targeted for .NET v4.0 and references the following assemblies:

* NServiceBus [v2.5.0.1446 Community Edition](http://www.nservicebus.com/downloads/Community.NServiceBus.2.5.0.1446.zip)
* EventStore [v2.0.11117.21](https://github.com/downloads/joliver/EventStore/EventStore-2.0.11117.21-net40.zip)

At the time of writing these are the recommended versions of these frameworks to use. Currently, if you require a build for a different minor version of these assemblies you can replace their corresponding dlls in the 'lib' folder and re-run 'build.bat'.

## Using NES

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
	{
		public void Init()
		{
			// EventStore Wireup
			Wireup.Init()
				.UsingInMemoryPersistence()
				.InitializeStorageEngine()
				.UsingJsonSerialization()
				.NES()
				.Build();

			// NServiceBus Configuration
			Configure.With()
				.Log4Net()
				.DefaultBuilder()
				.XmlSerializer()
				.NES();
		}
	}

For a more complete example, please open NES.sln in Visual Studio and hit F5. This will start the [NES.Sample](https://github.com/elliotritchie/NES/tree/master/src/NES.Sample) NServiceBus endpoint aswell as the [NES.Sample.Web](https://github.com/elliotritchie/NES/tree/master/src/NES.Sample.Web) MVC 3 website.