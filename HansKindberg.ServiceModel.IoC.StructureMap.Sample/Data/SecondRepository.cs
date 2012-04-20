namespace HansKindberg.ServiceModel.IoC.StructureMap.Sample.Data
{
	public class SecondRepository : ISecondRepository
	{
		#region Methods

		public virtual string GetInformation()
		{
			return "Information from second repository.";
		}

		#endregion
	}
}