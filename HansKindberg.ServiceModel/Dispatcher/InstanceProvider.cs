using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace HansKindberg.ServiceModel.Dispatcher
{
	public abstract class InstanceProvider : IInstanceProvider
	{
		#region Methods

		public abstract object GetInstance(InstanceContext instanceContext, Message message);
		public abstract object GetInstance(InstanceContext instanceContext);

		public virtual void ReleaseInstance(InstanceContext instanceContext, object instance)
		{
			IDisposable disposable = instance as IDisposable;
			if(disposable != null)
				disposable.Dispose();
		}

		#endregion
	}
}