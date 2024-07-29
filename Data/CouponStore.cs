using MinimalAPIs.Models;

namespace MinimalAPIs.Data
{
    public static class CouponStore
    {
        public static List<Coupon> couponList = new List<Coupon>
        {
            new Coupon{Id=1,Name="100ff",Percent=10,IsActive=true},
            new Coupon{Id=2,Name="200ff",Percent=20,IsActive=false}
            };



    }
}
