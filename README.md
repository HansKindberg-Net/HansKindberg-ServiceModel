HansKindberg-ServiceModel
=========================
Additions and extensions for System.ServiceModel.dll. For the moment there is only bootstrapper functionallity, mainly for hooking up IoC containers for WCF's. Later I will add other additons/extensions for System.ServiceModel in this solution if I find it necesarry.
HansKindberg.ServiceModel.IoC.StructureMap is included in the solution and is a library for bootstrapping StructureMap. The solution also includes a WCF library named HansKindberg.ServiceModel.IoC.StructureMap.Sample, it is a sample WCF libray that uses StructureMap as IoC container.

Bootstrapper
----------------
To add a bootstrapper for your WCF project add the following to the config file:
<pre>
&lt;configuration&gt;
	&lt;system.serviceModel&gt;
		&lt;behaviors&gt;
			&lt;serviceBehaviors&gt;
				&lt;behavior&gt;
					&lt;bootstrapper type="SomeCompany.SomeName.SomeBootstrapper,  SomeCompany.SomeName" /&gt;
				&lt;/behavior&gt;
			&lt;/serviceBehaviors&gt;
		&lt;/behaviors&gt;
		&lt;extensions&gt;
			&lt;behaviorExtensions&gt;
				&lt;add name="bootstrapper" type="HansKindberg.ServiceModel.Configuration.BootstrapperElement, HansKindberg.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8d368c2da66412b2" /&gt;
			&lt;/behaviorExtensions&gt;
		&lt;/extensions&gt;
	&lt;/system.serviceModel&gt;
&lt;/configuration&gt;
</pre>
Where the type "SomeCompany.SomeName.SomeBootstrapper,  SomeCompany.SomeName" is a class implementing the interface  "HansKindberg.ServiceModel.IBootstrapper".

Bootstrapper sample:
<pre>
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using HansKindberg.ServiceModel;

namespace SomeCompany.SomeName
{
	public class Bootstrapper : IBootstrapper
	{
		#region Methods

		public virtual void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
			// Add binding parameters if you want to.
		}

		public virtual void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			// Apply dispatch behavior if you want to.
		}

		public virtual IInstanceProvider GetInstanceProvider(Type serviceType, IInstanceProvider defaultInstanceProvider)
		{
			// Return your preferred InstanceProvider or, if you dont want to, return defaultInstanceProvider.
			// If you use an IoC container you can use it to get an InstanceProvider instance for the specified serviceType. You have to implement an InstanceProvider for this, see the HansKindberg.ServiceModel.IoC.StructureMap for an example.
			return defaultInstanceProvider;
		}

		public virtual void Initialize()
		{
			// This triggers first.
			// Here you can initialize your IoC container.
		}

		public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			// Validate if you want to.
		}

		#endregion
	}
}
</pre>