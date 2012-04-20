using System;
using System.Configuration;
using System.Reflection;
using System.ServiceModel.Configuration;
using HansKindberg.ServiceModel.Description;

namespace HansKindberg.ServiceModel.Configuration
{
	public class BootstrapperElement : BehaviorExtensionElement
	{
		#region Fields

		private const string _typePropertyName = "type";

		#endregion

		#region Properties

		public override Type BehaviorType
		{
			get { return typeof(BootstrapperBehavior); }
		}

		[ConfigurationProperty(_typePropertyName, IsRequired = true)]
		public virtual string TypeName
		{
			get { return (string) base[_typePropertyName]; }
			set { base[_typePropertyName] = value; }
		}

		#endregion

		#region Methods

		protected override object CreateBehavior()
		{
			try
			{
				return new BootstrapperBehavior((IBootstrapper) Activator.CreateInstance(this.TryGetType(this.TypeName)));
			}
			catch(Exception exception)
			{
				throw new TargetInvocationException("Could not create behavior.", exception);
			}
		}

		protected virtual Type TryGetType(string typeName)
		{
			if(string.IsNullOrEmpty(typeName))
				throw new ConfigurationErrorsException("The type/typename for the bootstrapper can not be null or empty.");

			Type type;

			try
			{
				type = Type.GetType(typeName, true, true);
			}
			catch(Exception exception)
			{
				throw new ConfigurationErrorsException(string.Format("Could not get a type from the string representation \"{0}\".", typeName), exception);
			}

			if(!typeof(IBootstrapper).IsAssignableFrom(type))
				throw new ConfigurationErrorsException(string.Format("The bootstrapper type \"{0}\" does not implement the interface \"{1}\".", type.FullName, typeof(IBootstrapper).FullName));

			return type;
		}

		#endregion
	}
}