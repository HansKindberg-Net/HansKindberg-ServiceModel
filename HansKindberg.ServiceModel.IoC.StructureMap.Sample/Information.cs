using System.Runtime.Serialization;

namespace HansKindberg.ServiceModel.IoC.StructureMap.Sample
{
	[DataContract]
	public class Information
	{
		#region Properties

		[DataMember]
		public virtual string InformationFromFirstRepository { get; set; }

		[DataMember]
		public virtual string InformationFromSecondRepository { get; set; }

		#endregion
	}
}