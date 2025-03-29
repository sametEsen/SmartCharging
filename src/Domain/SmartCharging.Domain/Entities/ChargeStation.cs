namespace SmartCharging.Domain.Entities
{
	public class ChargeStation
	{
		public int Id { get; private set; }
		public string Name { get; private set; }

		public int? GroupId { get; private set; }  // Foreign Key for EF Core
		public Group? Group { get; private set; }  // Navigation Property

		private readonly List<Connector> _connectors = new();
		public IReadOnlyCollection<Connector> Connectors => _connectors.AsReadOnly();

		// Parameterless constructor
		public ChargeStation() { }

		public ChargeStation(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public void UpdateName(string name) => Name = name;

		public void AssignToGroup(Group group)
		{
			GroupId = group.Id;
			Group = group;
		}

		public void UnassignFromGroup()
		{
			GroupId = null;
			Group = null;
		}

		public void AddConnector(Connector connector)
		{
			if (_connectors.Count >= 5)
				throw new InvalidOperationException("A charge station can have at most 5 connectors.");
			_connectors.Add(connector);
		}

		public void RemoveConnector(Connector connector)
		{
			_connectors.Remove(connector);
		}

		public int GetTotalCurrentLoad()
		{
			return _connectors.Sum(c => c.MaxCurrentInAmps);
		}
	}

}
