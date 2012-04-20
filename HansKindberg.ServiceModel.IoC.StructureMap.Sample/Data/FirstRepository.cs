namespace HansKindberg.ServiceModel.IoC.StructureMap.Sample.Data
{
	public class FirstRepository : IFirstRepository
	{
		#region Methods

		public virtual string GetInformation()
		{
			return "Information from first repository.";
		}

		#endregion
	}
}