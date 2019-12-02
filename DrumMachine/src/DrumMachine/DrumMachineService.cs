using DrumMachine.protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DrumMachine
{
    public class DrumMachineService : protos.DrumMachine.DrumMachineBase
    {
        private readonly ILogger logger;
        private readonly PatternProjections projections;
        public DrumMachineService(
            ILogger<DrumMachineService> loggr,
            PatternProjections patternProjections)
        {
            logger = loggr;
            projections = patternProjections;
        }

        public override async Task PlayMusicStream(ProgrammedMusicRequest request, IServerStreamWriter<MusicSteamResponse> responseStream, ServerCallContext context)
        {
            if (request == null) throw new RpcException(new Status(StatusCode.InvalidArgument, $"{nameof(request)} cannot be empty"));
            if (request.Bpm < 60) throw new RpcException(new Status(StatusCode.InvalidArgument, $"{nameof(request.Bpm)} must be a valid integer starting with 60"));
            if (!request.Pattern.Any()) throw new RpcException(new Status(StatusCode.InvalidArgument, $"{nameof(request.Pattern)} cannot be empty"));

            logger.LogInformation("processing input ...");
            var (patterns, stepCount) = projections.Project(request.Pattern);
            var programmedSequence = projections.Merge(patterns, stepCount);
            var interval = await GetBeatInterval(request.Bpm, stepCount); // approximating here. Can be precise if we start to cast it to double everywhere

            logger.LogDebug("start music ...");

            int idx = 0;
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(interval); // trying to print in tempo

                var response = new MusicSteamResponse();
                response.Beats.AddRange(programmedSequence.ElementAt(idx));
                idx++;
                if (idx >= stepCount)
                {
                    idx = 0;
                }
                await responseStream.WriteAsync(response);
            }
        }

        private Task<int> GetBeatInterval(int beatsPerMinute, int maxSteps) => Task.FromResult(((60000 / beatsPerMinute) * 4) / maxSteps);
    }
}
