
/////////////////////////////////////////////////////////////////////////
//
/// LINQ Joins
///  - inner join
///  - left outer join
///  - select records on the left with no match on the right
///  
/// - resources
///   https://visualstudiomagazine.com/articles/2017/02/01/joining-with-linq.aspx
/// 
/////////////////////////////////////////////////////////////////////////
using System.Linq;
using System.Collections.Generic;

/////////////////////////////////////////////////////////////////////////
#region setup data
var customers = new List<Customer>
{
    new Customer { CustomerId = 1,  Name = "Bob Lesman", City = "Chicago" },
    new Customer {  CustomerId = 2,Name = "Joe Stevens", City = "Chicago" },
    new Customer {  CustomerId = 3,Name = "Merry Smith", City = "Chicago" },
    new Customer {  CustomerId = 4,Name = "Sue Lin", City = "New York" },
    new Customer {  CustomerId = 5,Name = "Jose Gonzalez", City = "New York" },
    new Customer {  CustomerId = 6,Name = "Nathan Jones", City = "New York" },
    new Customer { CustomerId = 7, Name = "Jane Doe", City = "Seattle" },
    new Customer {  CustomerId = 8,Name = "Sammy Adams", City = "Seattle" },
    new Customer {  CustomerId = 9,Name = "Ed Wards", City = "Seattle" }
};

var orders = new List<Order>
{
    new Order { OrderId = 1, CustomerId = 1, Item = "Shoes" },
    new Order { OrderId = 2, CustomerId = 1, Item = "Headphones" },
    new Order { OrderId = 3, CustomerId = 1, Item = "Bat" },
    new Order { OrderId = 4, CustomerId = 2, Item = "Gloves" },
    new Order { OrderId = 5, CustomerId = 3, Item = "Shirt" },
    new Order { OrderId = 6, CustomerId = 4, Item = "Hat" },
    new Order { OrderId = 7, CustomerId = 4, Item = "Socks" },
};

#endregion
/////////////////////////////////////////////////////////////////////////

// default inner join
var invoice1 = from cust in customers
               join o in orders on cust.CustomerId equals o.CustomerId
               select new { cust.CustomerId, cust.Name, cust.City, o.Item };

Console.WriteLine("--------------------------");
foreach (var i in invoice1)
{
    Console.WriteLine($"{i.CustomerId} {i.Name} {i.City}, {i.Item}");
}

// left join with DefaultIfEmpty
// 
// 
// - three steps:
//   -  1. we do the join into an intermediate object
//   -  2. specify what happens if the join does not match using DefaultIfEmpty()
//        - if there is no match, then use linq DefaultIfEmpty() to set default values
//        - note: in the DefaultIfEmpty(), we only set the property that we are going to include in our select
//   -  3. finally select from the intermediate object
var invoice2 = from cust in customers
               join o in orders on cust.CustomerId equals o.CustomerId
               into customerOrders // 1. intermediate object
               from co in customerOrders.DefaultIfEmpty(new Order() { Item = "null" }) //2/ DefaultIfEmpty() sets default values in case of missing joins
               select new { cust.CustomerId, cust.Name, cust.City, co.Item }; // 3. select from intermediate object 

Console.WriteLine("--------------------------");
foreach (var i in invoice2)
{
    Console.WriteLine($"{i.CustomerId} {i.Name} {i.City}, {i.Item}");
}


// customers with no orders
// - same add a whre clause to filter the intermediate object
// - we select the only when the join did not match anything
var invoice3 = from cust in customers
               join o in orders on cust.CustomerId equals o.CustomerId
               into customerOrders
               from co in customerOrders.DefaultIfEmpty()
               where co == null // there was no match in the join
               select new { cust.CustomerId, cust.Name, cust.City };

Console.WriteLine("--------------------------");
foreach (var i in invoice3)
{
    Console.WriteLine($"{i.CustomerId} {i.Name} {i.City}");
}

/////////////////////////////////////////////////////////////////////////
#region classes
internal class Order
{

    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string Item { get; set; }
}

internal class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
}

#endregion classes
/////////////////////////////////////////////////////////////////////////