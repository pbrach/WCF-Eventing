# WCF-Eventing
A mid-weight eventing solution fo WCF. Implemented using an Observer pattern. This project exists mainly for teaching/learning purposes, as the implementation is somewhat simple and not optimized. So take this as a general idea of how such a system could be realized, but definitely not as something that is production ready.

## Requirements
This solution was developed using
- Visual Studio 2017 Pro
- .NET Framwork 4.6.1
- Used libs: No external libs only .NET WCF

## Problem
Event transfer in WCF is mainly organized via WCF-Callbacks.Although this is a nice way it has 1 major downside: callbacks create a duplex channel and keep it open. Because it is a normal channel like every WCF channel, it is (and should be!) disposed after a certain timeout period. For short lived clients (e.g.: web clients) this is perfect solution. 

There are however scenarios where channel timeouts are a huge problem (e.g.: events that are fired after a very long time). The alternative to WCF-callbacks is normally a message queue system, but this normally comes with a big overhead. These systems often need a separate installation and on the coding side one must wirte some abstraction layer. This makes sense for large distributed applications, but in smaller WCF applications where WCF is mainly used as an IPC method on the same system a intermediate solution is needed.

## Idea
For this reason I tested the necessary steps to implement an Observer pattern via WCF. The idea is that all WCF services implement a common interface (public listener registration and an event handler method). Only a list of registered services is known and the data for connecting to those. During event invokation this data is used to create a handle to that service and invoke its event handler method. 
