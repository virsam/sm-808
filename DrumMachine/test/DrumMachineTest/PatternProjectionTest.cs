using DrumMachine;
using DrumMachine.protos;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DrumMachineTest
{
    public class PatternProjectionTest
    {
        private readonly PatternProjections projectedPattern;
        public PatternProjectionTest() => projectedPattern = new PatternProjections();
        [Fact]
        public void ShouldMergeToUniqueCollection()
        {
            var kickSequence = new List<DrumPatternTypes> { DrumPatternTypes.None, DrumPatternTypes.Kick, DrumPatternTypes.None, DrumPatternTypes.Kick };
            var snareSequence = new List<DrumPatternTypes> { DrumPatternTypes.Snare, DrumPatternTypes.None, DrumPatternTypes.None, DrumPatternTypes.Snare };
            var pattern = new List<List<DrumPatternTypes>> { kickSequence, snareSequence };

            var sequence = projectedPattern.Merge(pattern, 4).ToList();

            sequence[0].Count().Should().Be(2);
            sequence[0].Should().Contain(DrumPatternTypes.None);
            sequence[0].Should().Contain(DrumPatternTypes.Snare);

            sequence[1].Count().Should().Be(2);
            sequence[1].Should().Contain(DrumPatternTypes.None);
            sequence[1].Should().Contain(DrumPatternTypes.Kick);

            sequence[2].Count().Should().Be(1);
            sequence[2].Should().Contain(DrumPatternTypes.None);

            sequence[3].Count().Should().Be(2);
            sequence[3].Should().Contain(DrumPatternTypes.Kick);
            sequence[3].Should().Contain(DrumPatternTypes.Snare);
        }

        [Fact]
        public void ShouldProjectPatternFromState()
        {
            var pattern1 = new ProgrammedPattern() { StepCount = ProgrammedPattern.Types.Steps.Eight, Type = DrumPatternTypes.Kick };
            pattern1.ProgrammedSteps.AddRange(new List<bool> { true, false, false, false, false, false, false, false });

            var pattern2 = new ProgrammedPattern() { StepCount = ProgrammedPattern.Types.Steps.Eight, Type = DrumPatternTypes.Snare };
            pattern2.ProgrammedSteps.AddRange(new List<bool> { false, false, false, false, false, false, false, true });

            var inputPatterns = new List<ProgrammedPattern> { pattern1, pattern2 };

            var (pattern, steps) = projectedPattern.Project(inputPatterns);

            steps.Should().Be(8);
            pattern.Count().Should().Be(inputPatterns.Count());

            pattern.ElementAt(0).Where(p => p == DrumPatternTypes.Kick).Count().Should().Be(1);
            pattern.ElementAt(0).Where(p => p == DrumPatternTypes.None).Count().Should().Be(7);

            pattern.ElementAt(1).Where(p => p == DrumPatternTypes.Snare).Count().Should().Be(1);
            pattern.ElementAt(1).Where(p => p == DrumPatternTypes.None).Count().Should().Be(7);
        }

        [Fact]
        public void ShouldProjectPatternWhenBarContainsDifferentSteps()
        {
            var pattern1 = new ProgrammedPattern() { StepCount = ProgrammedPattern.Types.Steps.Eight, Type = DrumPatternTypes.Kick };
            pattern1.ProgrammedSteps.AddRange(new List<bool> { true, false, false, false, true, false, false, false });

            var pattern2 = new ProgrammedPattern() { StepCount = ProgrammedPattern.Types.Steps.Sixteen, Type = DrumPatternTypes.Snare };
            pattern2.ProgrammedSteps.AddRange(new List<bool> { false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true });

            var inputPatterns = new List<ProgrammedPattern> { pattern1, pattern2 };

            var (pattern, steps) = projectedPattern.Project(inputPatterns);

            steps.Should().Be(16);
            pattern.Count().Should().Be(inputPatterns.Count());

            pattern.ElementAt(0).Where(p => p == DrumPatternTypes.Kick).Count().Should().Be(4);
            pattern.ElementAt(0).Where(p => p == DrumPatternTypes.None).Count().Should().Be(12);

            pattern.ElementAt(1).Where(p => p == DrumPatternTypes.Snare).Count().Should().Be(2);
            pattern.ElementAt(1).Where(p => p == DrumPatternTypes.None).Count().Should().Be(14);
        }
    }
}
