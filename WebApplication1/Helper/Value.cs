namespace WebApplication1.Helper
{
    public  class Value
    {
        public  bool ChooseValue { get; set; }
        public  string NameEn { get; set; }
        public  string NameAr { get; set; }
        public Value(bool _ChooseValue,string _NameEn,string _NameAr)
        {
            ChooseValue = _ChooseValue;
            NameAr = _NameAr;
            NameEn = _NameEn;
        }
    }
}
