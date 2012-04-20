using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using StructureMap;

namespace HansKindberg.ServiceModel.IoC.StructureMap.Dispatcher
{
	public class InstanceProvider : HansKindberg.ServiceModel.Dispatcher.InstanceProvider
	{
		#region Fields

		private readonly IContainer _container;
		private readonly Type _serviceType;

		#endregion

		#region Constructors

		public InstanceProvider(IContainer container, Type serviceType)
		{
			if(container == null)
				throw new ArgumentNullException("container");

			if(serviceType == null)
				throw new ArgumentNullException("serviceType");

			this._container = container;
			this._serviceType = serviceType;
		}

		#endregion

		#region Methods

		public override object GetInstance(InstanceContext instanceContext, Message message)
		{
			return this._container.GetInstance(this._serviceType);
		}

		public override object GetInstance(InstanceContext instanceContext)
		{
			return this.GetInstance(instanceContext, null);
		}

		#endregion
	}
}