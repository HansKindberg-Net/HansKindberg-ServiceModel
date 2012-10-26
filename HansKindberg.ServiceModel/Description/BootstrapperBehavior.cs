using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace HansKindberg.ServiceModel.Description
{
	public class BootstrapperBehavior : IServiceBehavior
	{
		#region Fields

		private readonly IBootstrapper _bootstrapper;

		#endregion

		#region Constructors

		protected internal BootstrapperBehavior(IBootstrapper bootstrapper)
		{
			if(bootstrapper == null)
				throw new ArgumentNullException("bootstrapper");

			this._bootstrapper = bootstrapper;
			this._bootstrapper.Initialize();
		}

		#endregion

		#region Methods

		public virtual void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
			this._bootstrapper.AddBindingParameters(serviceDescription, serviceHostBase, endpoints, bindingParameters);
		}

		public virtual void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			if(serviceDescription == null)
				throw new ArgumentNullException("serviceDescription");

			if(serviceHostBase == null)
				throw new ArgumentNullException("serviceHostBase");

			foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
			{
				foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
				{
					endpointDispatcher.DispatchRuntime.InstanceProvider = this._bootstrapper.GetInstanceProvider(serviceDescription.ServiceType, endpointDispatcher.DispatchRuntime.InstanceProvider);
				}
			}

			this._bootstrapper.ApplyDispatchBehavior(serviceDescription, serviceHostBase);
		}

		public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			this._bootstrapper.Validate(serviceDescription, serviceHostBase);
		}

		#endregion
	}
}