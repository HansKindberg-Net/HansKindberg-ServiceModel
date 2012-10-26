using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;

namespace HansKindberg.ServiceModel.IoC.StructureMap.Sample
{
	[ServiceContract]
	public interface IService
	{
		#region Methods

		[OperationContract]
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		Information GetInformation();

		#endregion
	}
}