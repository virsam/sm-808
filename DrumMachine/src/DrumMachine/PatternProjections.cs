using DrumMachine.protos;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using static DrumMachine.protos.ProgrammedPattern.Types;

namespace DrumMachine
{
    public class PatternProjections
    {
        public (IEnumerable<IEnumerable<DrumPatternTypes>>, int) Project(IEnumerable<ProgrammedPattern> patterns)
        {
            var projectedPatterns = new List<List<DrumPatternTypes>>();
            var maxStepCount = patterns.Select(p => GetStepCount(p.StepCount)).Max();
            foreach(var pattern in patterns)
            {
                var sequence = Enumerable.Empty<DrumPatternTypes>();
                if (pattern.ProgrammedSteps.Count() != GetStepCount(pattern.StepCount))
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, $"The number of programmed steps in a sequence must be atleast equal to the number of step count in a bar"));
                }
                var tempPattern = pattern.ProgrammedSteps.Take(GetStepCount(pattern.StepCount));
                var steps = Transform(tempPattern.ToList(), pattern.Type);
                var multiplier = maxStepCount / GetStepCount(pattern.StepCount);
                for(int i = 0; i < multiplier; i++)
                {
                    sequence = sequence.Concat(steps);
                }
                projectedPatterns.Add(sequence.ToList());
            }

            return (projectedPatterns, maxStepCount);

            IEnumerable<DrumPatternTypes> Transform(IEnumerable<bool> stateCollection, DrumPatternTypes type)
            {
                foreach(var state in stateCollection)
                {
                    if (state)
                    {
                        yield return type;
                    }
                    else
                    {
                        yield return DrumPatternTypes.None;
                    }
                }
            }
        }

        public IEnumerable<IEnumerable<DrumPatternTypes>> Merge(IEnumerable<IEnumerable<DrumPatternTypes>> projectedPatterns, int columnCount)
        {
            for (int column = 0; column < columnCount; column++)
            {
                var types = new List<DrumPatternTypes>();
                for(int row = 0; row < projectedPatterns.Count(); row++)
                {
                    types.Add((projectedPatterns.ElementAt(row)).ElementAt(column));
                }
                yield return types.Distinct();            
            }
        }

        private int GetStepCount(Steps stepCount)
        {
            switch (stepCount)
            {
                case Steps.Eight: return 8;
                case Steps.Sixteen: return 16;
                case Steps.ThirtyTwo: return 32;
                default: return 8;
            }
        }
    }
}
