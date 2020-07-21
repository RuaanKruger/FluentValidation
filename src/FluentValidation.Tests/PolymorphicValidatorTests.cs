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

namespace FluentValidation.Tests {
	using System.Collections.Generic;
	using Results;
	using Validators;
	using Xunit;

	public class PolymorphicValidatorTests {

		[Fact]
		public void Can_define_nested_rules_for_collection() {
			var person = new Person {
				Orders = new List<Order> {
					new Order {
						Amount = 1,
						ProductName = "Simple Order"
					},
					new QuantityOrder {
						Amount = 1,
						Quantity = 2,
						ProductName = "Multiple Orders"
					}
				}
			};

			var validator = new TestValidator(v => {
				v.RuleFor(x => x.Orders).NotNull();

				v.RuleForEach(p => p.Orders)
					.SetValidator(new PolymorphicValidator<Order, IOrder>()
						.Add<Order>(new SimpleOrderValidator())
						.Add<QuantityOrder>(new OrderQuantityValidator()));

			});

		}
	}

	/// <summary>
  /// Simple validator
  /// </summary>
	class SimpleOrderValidator : AbstractValidator<Order> {
		public SimpleOrderValidator() {
			RuleFor(x => x.Amount).NotNull().NotEmpty();
		}
	}

	/// <summary>
	/// Simple validator to check order product quantity
	/// </summary>
	class OrderQuantityValidator : AbstractValidator<QuantityOrder> {
		public OrderQuantityValidator() {
			RuleFor(x => x.Quantity).GreaterThan(0);
		}
	}
}
