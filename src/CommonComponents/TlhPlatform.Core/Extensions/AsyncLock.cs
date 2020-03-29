using System.Collections.Concurrent;

namespace TlhPlatform.Core
{

    public static class AsyncLock
    {
        private static ConcurrentDictionary<string, Nito.AsyncEx.AsyncLock> _lockMap = new ConcurrentDictionary<string, Nito.AsyncEx.AsyncLock>();

        public static Nito.AsyncEx.AsyncLock GetLockByKey(string key)
        {
            return _lockMap.GetOrAdd(key, (x) => new Nito.AsyncEx.AsyncLock());
        }


        // µ÷ÓÃÀý×Ó
        //Need lock to prevent concurrent access to same cart
        //public async Task<ActionResult<ShoppingCartItems>> AddItemToCart([FromBody] AddCartItem cartItem)
        //{
        //    EnsureCartExists();

        //    //Need lock to prevent concurrent access to same cart
        //    using (await AsyncLock.GetLockByKey(WorkContext.CurrentCart.Value.GetCacheKey()).LockAsync())
        //    {
        //        var cartBuilder = await LoadOrCreateCartAsync();

        //        var products = await _catalogService.GetProductsAsync(new[] { cartItem.ProductId }, Model.Catalog.ItemResponseGroup.Inventory | Model.Catalog.ItemResponseGroup.ItemWithPrices);
        //        if (products != null && products.Any())
        //        {
        //            await cartBuilder.AddItemAsync(products.First(), cartItem.Quantity);
        //            await cartBuilder.SaveAsync();
        //        }
        //        return new ShoppingCartItems { ItemsCount = cartBuilder.Cart.ItemsQuantity };
        //    }
        //}
    }
}
