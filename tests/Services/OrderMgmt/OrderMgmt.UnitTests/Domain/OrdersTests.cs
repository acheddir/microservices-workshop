
using System;
using AutoBogus;
using OrderMgmt.Domain.Exceptions;
using OrderMgmt.Domain.Model.Orders;
using Shouldly;
using Xunit;

namespace OrderMgmt.UnitTests.Domain;

public class OrdersTests
{
    [Fact]
    public void create_order_item_with_parameterless_constructor_passes()
    {
        var orderItem = new OrderItem();
        
        orderItem.ShouldNotBeNull();
        orderItem.ProductId.ShouldBe(default);
        orderItem.GetProductName().ShouldBe(default);
        orderItem.GetUnitPrice().ShouldBe(default(decimal));
        orderItem.GetCurrentDiscount().ShouldBe(default(decimal));
        orderItem.GetPictureUri().ShouldBe(default);
        orderItem.GetUnits().ShouldBe(default);
    }
    
    [Fact]
    public void create_order_item_with_constructor_parameters_passes()
    {
        var fakeOrderItem = new AutoFaker<OrderItem>()
            .CustomInstantiator(f =>
                new OrderItem(
                    Guid.NewGuid(),
                    f.Commerce.ProductName(),
                    f.Finance.Amount(100, 200),
                    f.Finance.Amount(0, 50),
                    f.Image.PlaceImgUrl(),
                    f.Random.Int(0, 50)))
            .Generate();

        fakeOrderItem.ShouldNotBeNull();
        fakeOrderItem.ProductId.ShouldNotBe(default);
        fakeOrderItem.GetProductName().ShouldNotBe(default);
        fakeOrderItem.GetUnitPrice().ShouldNotBe(default);
        fakeOrderItem.GetCurrentDiscount().ShouldNotBe(default);
        fakeOrderItem.GetPictureUri().ShouldNotBe(default);
        fakeOrderItem.GetUnits().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void create_order_item_with_invalid_number_of_units_fails()
    {
        Should.Throw<OrderMgmtException>(() =>
        {
            new AutoFaker<OrderItem>()
                .CustomInstantiator(f =>
                    new OrderItem(
                        Guid.NewGuid(),
                        f.Commerce.ProductName(),
                        f.Finance.Amount(100, 200),
                        f.Finance.Amount(0, 50),
                        f.Image.PlaceImgUrl(),
                        f.Random.Int(-50, 0)))
                .Generate();
        });
    }

    [Fact]
    public void create_order_item_with_total_lower_than_applied_discount_fails()
    {
        Should.Throw<OrderMgmtException>(() =>
        {
            new AutoFaker<OrderItem>()
                .CustomInstantiator(f =>
                    new OrderItem(
                        Guid.NewGuid(),
                        f.Commerce.ProductName(),
                        f.Finance.Amount(0, 50),
                        f.Finance.Amount(60, 70),
                        f.Image.PlaceImgUrl()))
                .Generate();
        });
    }

    [Fact]
    public void set_new_discount_on_order_item_passes()
    {
        var fakeOrderItem = new AutoFaker<OrderItem>()
            .CustomInstantiator(f =>
                new OrderItem(
                    Guid.NewGuid(),
                    f.Commerce.ProductName(),
                    f.Finance.Amount(100, 200),
                    f.Finance.Amount(0, 50),
                    f.Image.PlaceImgUrl(),
                    f.Random.Int(0, 50)))
            .Generate();

        const int newDiscount = 60;
        fakeOrderItem.SetNewDiscount(newDiscount);
        
        fakeOrderItem.GetCurrentDiscount().ShouldBe(newDiscount);
    }

    [Fact]
    public void set_new_discount_on_order_item_with_an_invalid_discount_fails()
    {
        var fakeOrderItem = new AutoFaker<OrderItem>()
            .CustomInstantiator(f =>
                new OrderItem(
                    Guid.NewGuid(),
                    f.Commerce.ProductName(),
                    f.Finance.Amount(100, 200),
                    f.Finance.Amount(0, 50),
                    f.Image.PlaceImgUrl(),
                    f.Random.Int(1, 50)))
            .Generate();

        Should.Throw<OrderMgmtException>(() =>
        {
            const int newDiscount = -1;
            fakeOrderItem.SetNewDiscount(newDiscount);
        });
    }
    
    [Fact]
    public void set_new_discount_lower_than_total_order_item_fails()
    {
        var fakeOrderItem = new AutoFaker<OrderItem>()
            .CustomInstantiator(f =>
                new OrderItem(
                    Guid.NewGuid(),
                    f.Commerce.ProductName(),
                    f.Finance.Amount(100, 200),
                    f.Finance.Amount(0, 50),
                    f.Image.PlaceImgUrl()))
            .Generate();

        Should.Throw<OrderMgmtException>(() =>
        {
            const int newDiscount = 300;
            fakeOrderItem.SetNewDiscount(newDiscount);
        });
    }

    [Fact]
    public void add_units_to_order_item_passes()
    {
        var fakeOrderItem = new AutoFaker<OrderItem>()
            .CustomInstantiator(f =>
                new OrderItem(
                    Guid.NewGuid(),
                    f.Commerce.ProductName(),
                    f.Finance.Amount(100, 200),
                    f.Finance.Amount(0, 50),
                    f.Image.PlaceImgUrl()))
            .Generate();
        
        fakeOrderItem.AddUnits(1);
        
        fakeOrderItem.GetUnits().ShouldBe(2);
    }

    [Fact]
    public void add_invalid_units_to_order_item_fails()
    {
        var fakeOrderItem = new AutoFaker<OrderItem>()
            .CustomInstantiator(f =>
                new OrderItem(
                    Guid.NewGuid(),
                    f.Commerce.ProductName(),
                    f.Finance.Amount(100, 200),
                    f.Finance.Amount(0, 50),
                    f.Image.PlaceImgUrl()))
            .Generate();

        Should.Throw<OrderMgmtException>(() =>
        {
            fakeOrderItem.AddUnits(-2);
        });
    }

    [Fact]
    public void create_an_address_with_parameterless_constructor_passes()
    {
        var address = new Address();
        
        address.City.ShouldBe(string.Empty);
        address.Country.ShouldBe(string.Empty);
        address.State.ShouldBe(string.Empty);
        address.Street.ShouldBe(string.Empty);
        address.ZipCode.ShouldBe(string.Empty);
    }

    [Fact]
    public void create_an_address_passes()
    {
        var fakeAddress = new AutoFaker<Address>()
            .CustomInstantiator(f => new Address(
                f.Address.StreetAddress(),
                f.Address.City(),
                f.Address.State(),
                f.Address.Country(),
                f.Address.ZipCode()
            ))
            .Generate();
        
        fakeAddress.City.ShouldNotBe(string.Empty);
        fakeAddress.Country.ShouldNotBe(string.Empty);
        fakeAddress.State.ShouldNotBe(string.Empty);
        fakeAddress.Street.ShouldNotBe(string.Empty);
        fakeAddress.ZipCode.ShouldNotBe(string.Empty);
    }

    [Fact]
    public void comparing_two_addresses_having_the_same_data_passes()
    {
        var fakeAddressA = new AutoFaker<Address>()
            .CustomInstantiator(f => new Address(
                f.Address.StreetAddress(),
                f.Address.City(),
                f.Address.State(),
                f.Address.Country(),
                f.Address.ZipCode()
            ))
            .Generate();
        
        var fakeAddressB = new Address(
            fakeAddressA.Street,
            fakeAddressA.City,
            fakeAddressA.State,
            fakeAddressA.Country,
            fakeAddressA.ZipCode
        );
        
        Equals(fakeAddressA, fakeAddressB).ShouldBeTrue();
    }
}