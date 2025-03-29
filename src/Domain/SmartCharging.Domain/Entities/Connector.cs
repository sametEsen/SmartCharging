namespace SmartCharging.Domain.Entities
{
	public class Connector
	{
		public int Id { get; private set; }  // Unique per ChargeStation (1-5)
		public int MaxCurrentInAmps { get; private set; }

		public int ChargeStationId { get; private set; }  // Foreign Key for EF Core
		public ChargeStation ChargeStation { get; private set; }  // Navigation Property

		public Connector(int id, int maxCurrentInAmps, ChargeStation chargeStation)
		{
			if (id < 1 || id > 5) throw new ArgumentException("Connector ID must be between 1 and 5.");
			if (maxCurrentInAmps <= 0) throw new ArgumentException("Max current must be greater than zero.");

			Id = id;
			MaxCurrentInAmps = maxCurrentInAmps;
			ChargeStation = chargeStation;
			ChargeStationId = chargeStation.Id;
		}

		public void UpdateMaxCurrent(int maxCurrent)
		{
			if (maxCurrent <= 0) throw new ArgumentException("Max current must be greater than zero.");
			if (ChargeStation.Group != null && ChargeStation.Group.GetTotalCurrentLoad() - MaxCurrentInAmps + maxCurrent > ChargeStation.Group.CapacityInAmps)
				throw new InvalidOperationException("Updating max current exceeds group's capacity.");

			MaxCurrentInAmps = maxCurrent;
		}
	}

}
