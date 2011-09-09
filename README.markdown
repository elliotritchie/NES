NES v0.3
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
* Allow up-conversion of events ([Ref](http://distributedpodcast.com/2011/episode-5-cqrs-eventstore-best-frameworklibrary-ever) @ 36:00)
* Automatic publishing of persisted events
* Convention based event handling within aggregates
* No 'Save()' methods on repositories
* Transparent configuration

## Download
The easiest way to install NES is via [NuGet](http://nuget.org/List/Packages/NES) or you can download the source and run 'build.bat' from the command line. Once built, the files will be placed in the 'build' folder.

## Using NES

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

For a more complete example, please open and build NES.Sample.sln in Visual Studio and hit F5. This will start the [NES.Sample](https://github.com/elliotritchie/NES/tree/master/sample/NES.Sample) NServiceBus endpoint aswell as the [NES.Sample.Web](https://github.com/elliotritchie/NES/tree/master/sample/NES.Sample.Web) MVC 3 website.

## Need help? Found a bug? Have a question?

* For guidance on how to use NES please take a look at the wiki [pages](http://github.com/elliotritchie/NES/wiki/_pages).
* If you have any problems using NES please submit a new [issue](https://github.com/elliotritchie/NES/issues).
* Ask your question on [Stack Overflow](http://stackoverflow.com) and tag your question with the CQRS tag and the word "NES" in the title.