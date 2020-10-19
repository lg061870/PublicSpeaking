using System.Collections.Generic;
using HotChocolate;

namespace Demo.Reviews
{
    public class Query
    {
        public  GetTopProducts(
            int first, 
            [Service] ProductRepository repository) =>
            repository.GetTopProducts(first);

        public Product GetProduct(
            int upc, 
            [Service] ProductRepository repository) =>
            repository.GetProduct(upc);
    }
}