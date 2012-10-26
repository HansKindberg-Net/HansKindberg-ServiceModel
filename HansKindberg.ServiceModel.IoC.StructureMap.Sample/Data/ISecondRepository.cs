using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.ServiceModel.IoC.StructureMap.Sample.Data
{
	public interface ISecondRepository
	{
		#region Methods

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		string GetInformation();

		#endregion
	}
}