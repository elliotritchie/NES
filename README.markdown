NES - .NET Event Sourcing
=========================

NES is an extremely lightweight framework that helps you build domain models when you're doing Event Sourcing.

What is it?
-----------

CQRS and Event Sourcing are gaining in popularity. If you're looking to take advantage of these patterns on the .NET platform then you'll probably want to use [NServiceBus](http://www.nservicebus.com) and [EventStore](https://github.com/joliver/eventstore). NES is an attempt to fill in the gaps between these two frameworks.

Why would I use it?
-------------------

The code required to bring NServiceBus and EventStore together isn't hard to write but it is important and it does have to work a certain way to take full advantage of both frameworks. There's some great resources out there already that help you write this yourself but ideally you'd want to concentrate on developing your domain model's behaviour rather than the infrastructure to hold your application together.

How does it work?
-------------------

An abstract class for you to inherit from and a couple of interfaces for you to wire up with your IoC container.

When should I use it?
---------------------

 * If you want to use NServiceBus with EventStore and -
  * Need to get up and running quickly.
  * You're a stickler for following 'recommended best practices' but are overwhelmed when trying to piece together the various implementation details and documentation for either framework.
  * You want to learn how to use the frameworks together by looking at C# rather than English.
