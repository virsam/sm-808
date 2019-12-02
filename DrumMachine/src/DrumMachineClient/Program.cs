using DrumMachine.protos;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static DrumMachine.protos.ProgrammedPattern.Types;

namespace DrumMachineClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new DrumMachine.protos.DrumMachine.DrumMachineClient(channel);

            var request = BuildFirstRequest();

            var stepsInBar = request.Pattern.Select(p => GetStepCount(p.StepCount)).Max();

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15)); // Some arbitrary time to cancel from the client
            using var streamingCall = client.PlayMusicStream(request, cancellationToken: cts.Token);

            try
            {
                int index = 0;
                await foreach (var beatsData in streamingCall.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
                {
                    PrintBeatsData(beatsData, stepsInBar, index);
                    index++;
                    if(index >= stepsInBar)
                    {
                        index = 0;
                    }
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Stream cancelled.");
            }
        }

        private static void PrintBeatsData(MusicSteamResponse data, int stepsInBar, int index)
        {
            data.Beats.Remove(DrumPatternTypes.None);
            
            if (data.Beats.Any())
            {
                string s = "|";
                foreach (var beat in data.Beats)
                {
                    s += beat.ToString() + "+";
                }
                s = s.TrimEnd('+');
                Console.Write(s);
            }
            else
            {
                Console.Write("|_");
            }
            
            if (index == (stepsInBar - 1))
            {
                Console.WriteLine("|");
            }

        }

        private static int GetStepCount(Steps stepCount)
        {
            switch (stepCount)
            {
                case Steps.Eight: return 8;
                case Steps.Sixteen: return 16;
                case Steps.ThirtyTwo: return 32;
                default: return 8;
            }
        }

        private static ProgrammedMusicRequest BuildFirstRequest()
        {
            var request = new ProgrammedMusicRequest() { Bpm = 128 };

            var pattern1 = new ProgrammedPattern { Type = DrumPatternTypes.Kick, StepCount = Steps.Eight };
            pattern1.ProgrammedSteps.AddRange(new List<bool> { true, false, false, false, true, false, false, false });

            var pattern2 = new ProgrammedPattern { Type = DrumPatternTypes.Snare, StepCount = Steps.Eight };
            pattern2.ProgrammedSteps.AddRange(new List<bool> { false, false, false, false, true, false, false, false });

            var pattern3 = new ProgrammedPattern { Type = DrumPatternTypes.Hithat, StepCount = Steps.Eight };
            pattern3.ProgrammedSteps.AddRange(new List<bool> { false, false, true, false, false, false, true, false });

            request.Pattern.AddRange(new List<ProgrammedPattern> { pattern1, pattern2, pattern3 });

            return request;
        }

        private static ProgrammedMusicRequest BuildSecondRequest()
        {
            var request = new ProgrammedMusicRequest() { Bpm = 128 };

            var pattern1 = new ProgrammedPattern { Type = DrumPatternTypes.Kick, StepCount = Steps.Eight };
            pattern1.ProgrammedSteps.AddRange(new List<bool> { false, false, true, false, false, false, true, false });

            var pattern2 = new ProgrammedPattern { Type = DrumPatternTypes.Hithat, StepCount = Steps.Sixteen };
            pattern2.ProgrammedSteps.AddRange(new List<bool> { false, false, true, false, false, true, false, false, true, false, false, true, false, false, true, false });
            
            request.Pattern.AddRange(new List<ProgrammedPattern> { pattern1, pattern2 });

            return request;
        }
    }
}
