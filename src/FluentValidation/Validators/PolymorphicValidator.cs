#region License
// Copyright (c) .NET Foundation and contributors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// The latest version of this file can be found at https://github.com/FluentValidation/FluentValidation
#endregion

namespace FluentValidation.Validators {
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Custom interface validator
	/// <para>Used to register a specific validator if the property type is interface</para>
	/// </summary>
	/// <typeparam name="TType">Current property type</typeparam>
	/// <typeparam name="TInterface">Property interface</typeparam>
	public class PolymorphicValidator<TType, TInterface> : ChildValidatorAdaptor<TType, TInterface>
	{ 
		private readonly Dictionary<Type, IValidator<TInterface>> _derivedValidators = new Dictionary<Type, IValidator<TInterface>>();

		public PolymorphicValidator() : base((IValidator<TInterface>)null, typeof(IValidator<TInterface>)) {
		}

		public PolymorphicValidator<TType, TInterface> Add<TDerived>(IValidator<TDerived> derivedValidator)
			where TDerived : TInterface {
			_derivedValidators[typeof(TDerived)] = (IValidator<TInterface>) derivedValidator;
			return this;
		}

		public override IValidator<TInterface> GetValidator(PropertyValidatorContext context) {
	
			if (context.PropertyValue == null) {
				return null;
			}

			if (_derivedValidators.TryGetValue(context.PropertyValue.GetType(), out var derivedValidator))
			{
				return derivedValidator;
			}

			return null;
		}
  }
}
