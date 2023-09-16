namespace OhMyDearGpnu.Api
{
    public class PageCacheManager
    {
        private List<PageCache> caches = new();

        public void AddCache(PageCache page)
        {
            PageCache? oldCache = null;
            foreach (var cache in caches)
            {
                if (cache.Identifier == page.Identifier)
                {
                    oldCache = cache;
                    break;
                }
            }

            if (oldCache != null)
            {
                if (oldCache.ExpireAt > page.ExpireAt)
                    return;
                caches.Remove(oldCache);
            }
            caches.Add(page);
        }

        public PageCache? GetCache(string identifier)
            => caches.FirstOrDefault(p => p.Identifier == identifier);

        public void Remove(string identifier)
        {
            foreach (var cache in caches)
            {
                if (cache.Identifier == identifier)
                {
                    caches.Remove(cache);
                    return;
                }
            }
        }
    }
}
