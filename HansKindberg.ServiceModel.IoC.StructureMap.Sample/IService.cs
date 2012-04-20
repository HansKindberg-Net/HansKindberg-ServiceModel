using System.ServiceModel;

namespace HansKindberg.ServiceModel.IoC.StructureMap.Sample
{
	[ServiceContract]
	public interface IService
	{
		#region Methods

		[OperationContract]
		Information GetInformation();

		#endregion
	}
}