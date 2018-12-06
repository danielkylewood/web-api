using Optional.Unsafe;

namespace WebApiTemplate.Domain.Extensions
{
    public static class OptionalExtensions
    {
        public static bool TryUnwrap<TOutType>(this Optional.Option<TOutType> optional, out TOutType outType) where TOutType : class
        {
            if (optional.HasValue)
            {
                outType = optional.ValueOrFailure();
                return true;
            }

            outType = default(TOutType);
            return false;
        }

        public static bool TryUnwrapStruct<TOutType>(this Optional.Option<TOutType> optional, out TOutType outType) where TOutType : struct
        {
            if (optional.HasValue)
            {
                outType = optional.ValueOrFailure();
                return true;
            }

            outType = default(TOutType);
            return false;
        }
    }
}
