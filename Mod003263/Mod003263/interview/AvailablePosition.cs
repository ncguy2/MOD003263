
/**
 *  Author: Nick Guy
 *  Date: 01/12/2016
 *  Contains: Position
 */

using System;

namespace Mod003263.interview {
    public class AvailablePosition {
        public int Id { get; set; }
        public string Position { get; set; }
        public int Seats { get; set; }

        public bool Equals(AvailablePosition other) {
            return string.Equals(Position, other.Position, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AvailablePosition && Equals((AvailablePosition) obj);
        }

        public override int GetHashCode() {
            return Position != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Position) : 0;
        }

        public override string ToString() {
            return $"{Position} [{Seats}]";
        }
    }
}