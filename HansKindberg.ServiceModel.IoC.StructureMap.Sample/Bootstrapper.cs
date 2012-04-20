using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using HansKindberg.ServiceModel.IoC.StructureMap.Dispatcher;
using HansKindberg.ServiceModel.IoC.StructureMap.Sample.Data;
using StructureMap;

namespace HansKindberg.ServiceModel.IoC.StructureMap.Sample
{
	public class Bootstrapper : IBootstrapper
	{
		#region Methods

		public virtual void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) {}
		public virtual void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {}

		public virtual IInstanceProvider GetInstanceProvider(Type serviceType, IInstanceProvider defaultInstanceProvider)
		{
			return new InstanceProvider(ObjectFactory.Container, serviceType);
		}

		public virtual void Initialize()
		{
			ObjectFactory.Initialize(initializer =>
			{
				initializer.For<IFirstRepository>().Singleton().Use(new FirstRepository());
				initializer.For<ISecondRepository>().Singleton().Use(new SecondRepository());
			});
		}

		public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {}

		#endregion
	}
}