### Environment

Download [Visual Studio](https://visualstudio.microsoft.com/downloads)
Download [.NET Core 3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0)

### Framework and why this approach

We have highlighted the separation of concerns by drawing service boundary on DrumMachine server that is powered by [gRPC server](https://grpc.io/docs/tutorials/basic/csharp/). We have introduced a streaming endpoint to play programmed music. We have chosen this approach because we wanted to stay connected to the client and be able to control the tempo based on the selected bpm on the client.
Our client is a console app which is powered by the same service contract.

### Project structure
```
    DrumMachine
    |
    |__src
    |   |__DrumMachine (server)
    |   |__DrumMachineClient (client)
    |
    |__test
        |__DrumMachineTest
```

### To start debugging with Visual Studio
1. Open Visual Studio and Debug `DrumMachine`.
2. Open another Visual Studio and debug `src\DrumMachineClient`.
3. In `DrumMachineClient`, you can either use pre-defined request `BuildFirstRequest()` or `BuildSecondRequest()`, or build your own.
4. The output is printed in tempo on the client console. (Have a very low bpm to increase the gap between the beats)