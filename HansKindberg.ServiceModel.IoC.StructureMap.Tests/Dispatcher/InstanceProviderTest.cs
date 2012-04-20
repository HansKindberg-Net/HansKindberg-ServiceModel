using System;
using HansKindberg.ServiceModel.IoC.StructureMap.Dispatcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace HansKindberg.ServiceModel.IoC.StructureMap.Tests.Dispatcher
{
	[TestClass]
	public class InstanceProviderTest
	{
		#region Methods

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ShouldThrowAnArgumentNullException_IfTheContainerParameterIsNull()
		{
			new InstanceProvider(null, Mock.Of<Type>());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ShouldThrowAnArgumentNullException_IfTheServiceTypeParameterIsNull()
		{
			new InstanceProvider(Mock.Of<IContainer>(), null);
		}

		[TestMethod]
		public void GetInstance_WithOneParameter_ShouldReturnTheObjectFromContainerGetInstance()
		{
			object instance = new object();
			Mock<IContainer> containerMock = new Mock<IContainer>();
			containerMock.Setup(container => container.GetInstance(It.IsAny<Type>())).Returns(instance);
			Assert.AreEqual(instance, new InstanceProvider(containerMock.Object, Mock.Of<Type>()).GetInstance(null));
		}

		[TestMethod]
		public void GetInstance_WithTwoParameters_ShouldReturnTheObjectFromContainerGetInstance()
		{
			object instance = new object();
			Mock<IContainer> containerMock = new Mock<IContainer>();
			containerMock.Setup(container => container.GetInstance(It.IsAny<Type>())).Returns(instance);
			Assert.AreEqual(instance, new InstanceProvider(containerMock.Object, Mock.Of<Type>()).GetInstance(null, null));
		}

		#endregion
	}
}