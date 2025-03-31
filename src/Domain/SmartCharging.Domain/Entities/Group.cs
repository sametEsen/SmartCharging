namespace SmartCharging.Domain.Entities
{
	public class Group
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public int CapacityInAmps { get; private set; }

		private readonly List<ChargeStation> _chargeStations = new();
		public IReadOnlyCollection<ChargeStation> ChargeStations => _chargeStations.AsReadOnly();

		// Parameterless ctor
		public Group() { }

		public Group(int id, string name, int capacityInAmps)
		{
			if (capacityInAmps <= 0) throw new ArgumentException("Capacity must be greater than zero.");

			Id = id;
			Name = name;
			CapacityInAmps = capacityInAmps;
		}

		public void UpdateName(string name) => Name = name;
		public void UpdateCapacity(int capacity)
		{
			if (capacity <= 0) throw new ArgumentException("Capacity must be greater than zero.");
			if (GetTotalCurrentLoad() > capacity) throw new InvalidOperationException("New capacity is less than total current load.");

			CapacityInAmps = capacity;
		}

		public void AddChargeStation(ChargeStation chargeStation)
		{
			if (chargeStation.GroupId != null)
				throw new InvalidOperationException("Charge station already belongs to a group.");

			_chargeStations.Add(chargeStation);
			chargeStation.AssignToGroup(this);
			ValidateCapacity();
		}

		public void RemoveChargeStation(ChargeStation chargeStation)
		{
			_chargeStations.Remove(chargeStation);
			chargeStation.UnassignFromGroup();
		}

		public int GetTotalCurrentLoad()
		{
			return _chargeStations.Sum(cs => cs.GetTotalCurrentLoad());
		}

		private void ValidateCapacity()
		{
			if (GetTotalCurrentLoad() > CapacityInAmps)
				throw new InvalidOperationException("Total current load exceeds group's capacity.");
		}
	}

}
