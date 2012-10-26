using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using HansKindberg.ServiceModel.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HansKindberg.ServiceModel.Tests.Description
{
	[TestClass]
	public class BootstrapperBehaviorTest
	{
		#region Methods

		[TestMethod]
		public void AddBindingParameters_ShouldCallAddBindingParametersOnTheBootstrapper()
		{
			Mock<IBootstrapper> bootstrapperMock = new Mock<IBootstrapper>();
			bootstrapperMock.Setup(bootstrapper => bootstrapper.AddBindingParameters(It.IsAny<ServiceDescription>(), It.IsAny<ServiceHostBase>(), It.IsAny<Collection<ServiceEndpoint>>(), It.IsAny<BindingParameterCollection>()));
			bootstrapperMock.Verify(bootstrapper => bootstrapper.AddBindingParameters(It.IsAny<ServiceDescription>(), It.IsAny<ServiceHostBase>(), It.IsAny<Collection<ServiceEndpoint>>(), It.IsAny<BindingParameterCollection>()), Times.Never());
			new BootstrapperBehavior(bootstrapperMock.Object).AddBindingParameters(Mock.Of<ServiceDescription>(), Mock.Of<ServiceHostBase>(), Mock.Of<Collection<ServiceEndpoint>>(), Mock.Of<BindingParameterCollection>());
			bootstrapperMock.Verify(bootstrapper => bootstrapper.AddBindingParameters(It.IsAny<ServiceDescription>(), It.IsAny<ServiceHostBase>(), It.IsAny<Collection<ServiceEndpoint>>(), It.IsAny<BindingParameterCollection>()), Times.Once());
		}

		[TestMethod]
		public void ApplyDispatchBehavior_ShouldCallApplyDispatchBehaviorOnTheBootstrapper()
		{
			Mock<IBootstrapper> bootstrapperMock = new Mock<IBootstrapper>();
			bootstrapperMock.Setup(bootstrapper => bootstrapper.ApplyDispatchBehavior(It.IsAny<ServiceDescription>(), It.IsAny<ServiceHostBase>()));
			bootstrapperMock.Verify(bootstrapper => bootstrapper.ApplyDispatchBehavior(It.IsAny<ServiceDescription>(), It.IsAny<ServiceHostBase>()), Times.Never());
			new BootstrapperBehavior(bootstrapperMock.Object).ApplyDispatchBehavior(Mock.Of<ServiceDescription>(), Mock.Of<ServiceHostBase>());
			bootstrapperMock.Verify(bootstrapper => bootstrapper.ApplyDispatchBehavior(It.IsAny<ServiceDescription>(), It.IsAny<ServiceHostBase>()), Times.Once());
		}

		[TestMethod]
		public void ApplyDispatchBehavior_ShouldCallGetInstanceProviderOnTheBootstrapper_IfThereAreAnyEndpointDispatchers()
		{
			Mock<IBootstrapper> bootstrapperMock = new Mock<IBootstrapper>();
			bootstrapperMock.Setup(bootstrapper => bootstrapper.GetInstanceProvider(It.IsAny<Type>(), It.IsAny<IInstanceProvider>()));
			bootstrapperMock.Verify(bootstrapper => bootstrapper.GetInstanceProvider(It.IsAny<Type>(), It.IsAny<IInstanceProvider>()), Times.Never());

			byte amountOfChannelDispatchers = (byte) DateTime.Now.Day;
			byte endpointsPerChannelDispatcher = (byte) DateTime.Now.AddDays(10).Day;
			int numberOfCalls = amountOfChannelDispatchers*endpointsPerChannelDispatcher;

			using (ServiceHostBase serviceHost = CreateServiceHostBase(amountOfChannelDispatchers, endpointsPerChannelDispatcher))
			{
				new BootstrapperBehavior(bootstrapperMock.Object).ApplyDispatchBehavior(Mock.Of<ServiceDescription>(), serviceHost);
				bootstrapperMock.Verify(bootstrapper => bootstrapper.GetInstanceProvider(It.IsAny<Type>(), It.IsAny<IInstanceProvider>()), Times.Exactly(numberOfCalls));
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ApplyDispatchBehavior_IfTheServiceHostBaseParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			BootstrapperBehavior bootstrapperBehavior = new BootstrapperBehavior(new Mock<IBootstrapper>().Object);
			bootstrapperBehavior.ApplyDispatchBehavior(new ServiceDescription(), null);
		}

		[TestMethod]
		[SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "HansKindberg.ServiceModel.Description.BootstrapperBehavior")]
		public void Constructor_ShouldCallInitializeOnTheBootstrapper_IfTheBootstrapperParameterIsNotNull()
		{
			Mock<IBootstrapper> bootstrapperMock = new Mock<IBootstrapper>();
			bootstrapperMock.Setup(bootstrapper => bootstrapper.Initialize());
			bootstrapperMock.Verify(bootstrapper => bootstrapper.Initialize(), Times.Never());
			new BootstrapperBehavior(bootstrapperMock.Object);
			bootstrapperMock.Verify(bootstrapper => bootstrapper.Initialize(), Times.Once());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "HansKindberg.ServiceModel.Description.BootstrapperBehavior")]
		public void Constructor_ShouldThrowAnArgumentNullException_IfTheBootstrapperParameterIsNull()
		{
			new BootstrapperBehavior(null);
		}

		private static ServiceHostBase CreateServiceHostBase(byte amountOfChannelDispatchers, byte endpointsPerChannelDispatcher)
		{
			Mock<ServiceHostBase> serviceHostBaseMock = new Mock<ServiceHostBase> {CallBase = true};
			ServiceHostBase serviceHostBase = serviceHostBaseMock.Object;
			for(int i = 0; i < amountOfChannelDispatchers; i++)
			{
				ChannelDispatcher channelDispatcher = new ChannelDispatcher(Mock.Of<IChannelListener>());

				for(int j = 0; j < endpointsPerChannelDispatcher; j++)
				{
					EndpointDispatcher endpointDispatcher = new EndpointDispatcher(new EndpointAddress("http://testendpointaddress/" + j.ToString(CultureInfo.InvariantCulture)), "TestContractName", "TestContractNamespace");
					DispatchRuntime dispatchRuntime = (DispatchRuntime) Activator.CreateInstance(typeof(DispatchRuntime), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] {endpointDispatcher}, null);
					dispatchRuntime.InstanceProvider = new Mock<IInstanceProvider>().Object;
					channelDispatcher.Endpoints.Add(endpointDispatcher);
				}

				serviceHostBase.ChannelDispatchers.Add(channelDispatcher);
			}

			return serviceHostBase;
		}

		[TestMethod]
		public void Validate_ShouldCallValidateOnTheBootstrapper()
		{
			Mock<IBootstrapper> bootstrapperMock = new Mock<IBootstrapper>();
			bootstrapperMock.Setup(bootstrapper => bootstrapper.Validate(It.IsAny<ServiceDescription>(), It.IsAny<ServiceHostBase>()));
			bootstrapperMock.Verify(bootstrapper => bootstrapper.Validate(It.IsAny<ServiceDescription>(), It.IsAny<ServiceHostBase>()), Times.Never());
			new BootstrapperBehavior(bootstrapperMock.Object).Validate(Mock.Of<ServiceDescription>(), Mock.Of<ServiceHostBase>());
			bootstrapperMock.Verify(bootstrapper => bootstrapper.Validate(It.IsAny<ServiceDescription>(), It.IsAny<ServiceHostBase>()), Times.Once());
		}

		#endregion
	}
}