using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Standard
{ 
    public sealed class PartitioningDefinition : IValidatable
    {
        public int PartitionsNumber => PartitionsDefinitions.Count;
        public List<decimal> PartitionsDefinitions { get; }
        private readonly decimal _toleranceOfRoundingError;

        public PartitioningDefinition(ICollection<decimal> partitionProportions, decimal toleranceOfRoundingError = 0.00001m)
        {
            if (partitionProportions == null) throw new ArgumentNullException(nameof(partitionProportions));
            if (partitionProportions.Count == 0) throw new ArgumentException(nameof(partitionProportions));
            _toleranceOfRoundingError = toleranceOfRoundingError;
            PartitionsDefinitions = new List<decimal>(partitionProportions.Count);
            var ovarallSum = partitionProportions.Sum();
            foreach (var partitionProportion in partitionProportions)
            {
                PartitionsDefinitions.Add(partitionProportion / ovarallSum);
            }
        }

        public bool IsValid()
        {
            return PartitionsDefinitions?.Count > 0 && Math.Abs(PartitionsDefinitions.Sum() - 1m) < _toleranceOfRoundingError;
        }
    }
}
