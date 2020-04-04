namespace webBeta.NSerializer.Test.Beans
{
    public class ClassWithAutoproperties
    {
        public bool Bar { get; set; }
        private string Foo { get; set; }

        public ClassWithAutoproperties()
        {
            Foo = "Hello world!";
            Bar = true;
        }
    }
}