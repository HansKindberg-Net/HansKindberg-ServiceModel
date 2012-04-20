using System;
using HansKindberg.ServiceModel.Dispatcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HansKindberg.ServiceModel.Tests.Dispatcher
{
	[TestClass]
	public class InstanceProviderTest
	{
		#region Methods

		private static InstanceProvider CreateInstanceProvider()
		{
			return new Mock<InstanceProvider> {CallBase = true}.Object;
		}

		[TestMethod]
		public void ReleaseInstance_ShouldCallDisposeOnTheInstance_IfTheInstanceImplementsIDisposable()
		{
			Mock<IDisposable> disposableMock = new Mock<IDisposable>();
			disposableMock.Setup(disposable => disposable.Dispose());
			disposableMock.Verify(disposable => disposable.Dispose(), Times.Never());
			CreateInstanceProvider().ReleaseInstance(null, disposableMock.Object);
			disposableMock.Verify(disposable => disposable.Dispose(), Times.Once());
		}

		[TestMethod]
		public void ReleaseInstance_ShouldNotCallDisposeOnTheInstance_IfTheInstanceNotImplementsIDisposable()
		{
			CreateInstanceProvider().ReleaseInstance(null, new InstanceProviderTestFakedDisposable());
		}

		[TestMethod]
		public void ReleaseInstance_ShouldPassWithoutException_IfTheInstanceIsNull()
		{
			CreateInstanceProvider().ReleaseInstance(null, null);
		}

		#endregion
	}

	internal class InstanceProviderTestFakedDisposable
	{
		#region Methods

		public void Dispose()
		{
			throw new Exception("This should not be called.");
		}

		#endregion
	}
}