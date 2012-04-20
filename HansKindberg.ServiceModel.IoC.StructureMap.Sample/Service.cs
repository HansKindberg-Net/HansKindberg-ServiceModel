using System;
using HansKindberg.ServiceModel.IoC.StructureMap.Sample.Data;

namespace HansKindberg.ServiceModel.IoC.StructureMap.Sample
{
	public class Service : IService
	{
		#region Fields

		private readonly IFirstRepository _firstRepository;
		private readonly ISecondRepository _secondRepository;

		#endregion

		#region Constructors

		public Service(IFirstRepository firstRepository, ISecondRepository secondRepository)
		{
			if(firstRepository == null)
				throw new ArgumentNullException("firstRepository");

			if(secondRepository == null)
				throw new ArgumentNullException("secondRepository");

			this._firstRepository = firstRepository;
			this._secondRepository = secondRepository;
		}

		#endregion

		#region Methods

		public virtual Information GetInformation()
		{
			return new Information
				{
					InformationFromFirstRepository = this._firstRepository.GetInformation(),
					InformationFromSecondRepository = this._secondRepository.GetInformation()
				};
		}

		#endregion
	}
}