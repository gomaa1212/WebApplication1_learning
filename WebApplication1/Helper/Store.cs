namespace WebApplication1.Helper
{
    public static class Store
    {
        public static List<Value> chosses { get; set; } = new List<Value>()
        {
            new Value(false,"No","لا"),
            new Value(true,"Yes","نعم")
        };
    }
}
