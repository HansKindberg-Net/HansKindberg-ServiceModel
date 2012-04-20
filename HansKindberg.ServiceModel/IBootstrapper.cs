using System;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace HansKindberg.ServiceModel
{
	public interface IBootstrapper : IServiceBehavior
	{
		#region Methods

		IInstanceProvider GetInstanceProvider(Type serviceType, IInstanceProvider defaultInstanceProvider);
		void Initialize();

		#endregion
	}
}