using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using HansKindberg.ServiceModel.Configuration;
using HansKindberg.ServiceModel.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.ServiceModel.Tests.Configuration
{
	[TestClass]
	public class BootstrapperElementTest
	{
		#region Methods

		[TestMethod]
		public void BehaviorType_ShouldReturnABootstrapperBehaviorType()
		{
			Assert.AreEqual(typeof(BootstrapperBehavior), new BootstrapperElement().BehaviorType);
		}

		[TestMethod]
		public void CreateBehavior_ShouldReturnAnObjectOfTypeBootstrapperBehavior_IfTheTypeNameIsTheNameOfATypeThatInheritsFromIBootstrapper()
		{
			BootstrapperElementTestBootstrapperElementMock bootstrapperElement = new BootstrapperElementTestBootstrapperElementMock {TypeName = typeof(BootstrapperElementTestBootstrapperMock).FullName + ", " + typeof(BootstrapperElementTestBootstrapperMock).Assembly.GetName().Name};
			BootstrapperBehavior bootstrapperBehavior = (BootstrapperBehavior) bootstrapperElement.CreateBehavior();
			Assert.IsNotNull(bootstrapperBehavior);
		}

		[TestMethod]
		[ExpectedException(typeof(TargetInvocationException))]
		public void CreateBehavior_ShouldThrowATargetInvocationException_WithAConfigurationErrorsExceptionAsInnerException_IfTheTypeNameIsEmpty()
		{
			BootstrapperElementTestBootstrapperElementMock bootstrapperElement = new BootstrapperElementTestBootstrapperElementMock {TypeName = string.Empty};
			Assert.AreEqual(string.Empty, bootstrapperElement.TypeName);

			try
			{
				bootstrapperElement.CreateBehavior();
			}
			catch(Exception exception)
			{
				if(exception is TargetInvocationException)
				{
					Assert.AreEqual("Could not create behavior.", exception.Message);

					if(exception.InnerException == null || !(exception.InnerException is ConfigurationErrorsException))
						Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The inner exception should be of type \"{0}\".", typeof(ConfigurationErrorsException).FullName));

					Assert.AreEqual("The type/type-name for the bootstrapper can not be null or empty.", exception.InnerException.Message);

					throw;
				}

				Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The thrown exception should be of type \"{0}\".", typeof(TargetInvocationException).FullName));
			}
		}

		[TestMethod]
		[ExpectedException(typeof(TargetInvocationException))]
		public void CreateBehavior_ShouldThrowATargetInvocationException_WithAConfigurationErrorsExceptionAsInnerException_IfTheTypeNameIsNull()
		{
			BootstrapperElementTestBootstrapperElementMock bootstrapperElement = new BootstrapperElementTestBootstrapperElementMock {TypeName = null};
			Assert.IsNull(bootstrapperElement.TypeName);

			try
			{
				bootstrapperElement.CreateBehavior();
			}
			catch(Exception exception)
			{
				if(exception is TargetInvocationException)
				{
					Assert.AreEqual("Could not create behavior.", exception.Message);

					if(exception.InnerException == null || !(exception.InnerException is ConfigurationErrorsException))
						Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The inner exception should be of type \"{0}\".", typeof(ConfigurationErrorsException).FullName));

					Assert.AreEqual("The type/type-name for the bootstrapper can not be null or empty.", exception.InnerException.Message);

					throw;
				}

				Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The thrown exception should be of type \"{0}\".", typeof(TargetInvocationException).FullName));
			}
		}

		[TestMethod]
		[ExpectedException(typeof(TargetInvocationException))]
		public void CreateBehavior_ShouldThrowATargetInvocationException_WithAConfigurationErrorsExceptionAsInnerException_IfTheTypeNameIsTheNameOfANoneExistingType()
		{
			BootstrapperElementTestBootstrapperElementMock bootstrapperElement = new BootstrapperElementTestBootstrapperElementMock {TypeName = "SomeInvalidType"};
			Assert.AreEqual("SomeInvalidType", bootstrapperElement.TypeName);

			try
			{
				bootstrapperElement.CreateBehavior();
			}
			catch(Exception exception)
			{
				if(exception is TargetInvocationException)
				{
					Assert.AreEqual("Could not create behavior.", exception.Message);

					if(exception.InnerException == null || !(exception.InnerException is ConfigurationErrorsException))
						Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The inner exception should be of type \"{0}\".", typeof(ConfigurationErrorsException).FullName));

					Assert.AreEqual("Could not get a type from the string representation \"SomeInvalidType\".", exception.InnerException.Message);

					throw;
				}

				Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The thrown exception should be of type \"{0}\".", typeof(TargetInvocationException).FullName));
			}
		}

		[TestMethod]
		[ExpectedException(typeof(TargetInvocationException))]
		public void CreateBehavior_ShouldThrowATargetInvocationException_WithAConfigurationErrorsExceptionAsInnerException_IfTheTypeNameIsTheNameOfATypeThatNotInheritsFromIBootstrapper()
		{
			BootstrapperElementTestBootstrapperElementMock bootstrapperElement = new BootstrapperElementTestBootstrapperElementMock {TypeName = typeof(object).FullName};
			Assert.AreEqual("System.Object", bootstrapperElement.TypeName);

			try
			{
				bootstrapperElement.CreateBehavior();
			}
			catch(Exception exception)
			{
				if(exception is TargetInvocationException)
				{
					Assert.AreEqual("Could not create behavior.", exception.Message);

					if(exception.InnerException == null || !(exception.InnerException is ConfigurationErrorsException))
						Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The inner exception should be of type \"{0}\".", typeof(ConfigurationErrorsException).FullName));

					Assert.AreEqual("The bootstrapper type \"System.Object\" does not implement the interface \"HansKindberg.ServiceModel.IBootstrapper\".", exception.InnerException.Message);

					throw;
				}

				Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The thrown exception should be of type \"{0}\".", typeof(TargetInvocationException).FullName));
			}
		}

		#endregion
	}

	internal class BootstrapperElementTestBootstrapperElementMock : BootstrapperElement
	{
		#region Methods

		public new virtual object CreateBehavior()
		{
			return base.CreateBehavior();
		}

		#endregion
	}

	internal class BootstrapperElementTestBootstrapperMock : IBootstrapper
	{
		#region Methods

		public virtual void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) {}
		public virtual void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {}

		public virtual IInstanceProvider GetInstanceProvider(Type serviceType, IInstanceProvider defaultInstanceProvider)
		{
			return defaultInstanceProvider;
		}

		public virtual void Initialize() {}
		public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {}

		#endregion
	}
}