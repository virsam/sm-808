using DrumMachine;
using DrumMachine.protos;
using FluentAssertions;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DrumMachineTest
{
    public class DrumMachineServiceTest
    {
        private readonly ILogger<DrumMachineService> logger;
        private readonly PatternProjections projections;
        private readonly DrumMachineService service;
        public DrumMachineServiceTest()
        {
            logger = Substitute.For<ILogger<DrumMachineService>>();
            projections = Substitute.For<PatternProjections>();
            service = new DrumMachineService(logger, projections);
        }

        [Fact]
        public async Task ShouldThrowOnEmptyRequest()
        {
            await ThenExceptionIsThrown<RpcException>(async () => await service.PlayMusicStream(null, null, context: null), $"request cannot be empty");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(59)]
        public async Task ShouldThrowOnInvalidBpm(int bpm)
        {
            var request = new ProgrammedMusicRequest() { Bpm = bpm };
            await ThenExceptionIsThrown<RpcException>(async () => await service.PlayMusicStream(request, null, context: null), $"{nameof(request.Bpm)} must be a valid integer starting with 60");
        }

        [Fact]
        public async Task ShouldThrowWhenPatternIsEmpty()
        {
            var request = new ProgrammedMusicRequest() { Bpm = 120 };
            await ThenExceptionIsThrown<RpcException>(async () => await service.PlayMusicStream(request, null, context: null), $"{nameof(request.Pattern)} cannot be empty");
        }

        [Fact]
        public async Task ShouldThrowWhenPatternDoesNotMatchStepCount()
        {
            var request = new ProgrammedMusicRequest() { Bpm = 120 };
            var pattern = new ProgrammedPattern() { Type = DrumPatternTypes.Kick, StepCount = ProgrammedPattern.Types.Steps.Eight };
            pattern.ProgrammedSteps.Add(new List<bool> { false, false, false, false });
            request.Pattern.Add(pattern);

            await ThenExceptionIsThrown<RpcException>(async () => await service.PlayMusicStream(request, null, context: null), $"The number of programmed steps in a sequence must be atleast equal to the number of step count in a bar");
        }

        private async Task ThenExceptionIsThrown<T>(Func<Task> action, string errorMessage = "") where T : RpcException
        {
            var ex = await Assert.ThrowsAsync<T>(action);
            ex.StatusCode.Should().Be(StatusCode.InvalidArgument);
            if (!string.IsNullOrWhiteSpace(errorMessage)) Assert.Contains(errorMessage, ex.Message);
        }
    }
}
