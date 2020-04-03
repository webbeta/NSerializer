namespace webBeta.NSerializer.Test.Beans
{
    public class ClassWithGetters3
    {

        private int id;
        private string foo;
        private bool bar;

        public ClassWithGetters3()
        {
            id = 200;
            foo = "Hello world2!";
            bar = false;
        }

        public ClassWithGetters3(int id)
        {
            id = id;
            foo = "Hello world2!";
            bar = false;
        }

        public string getFoo()
        {
            return foo;
        }

        public void setFoo(string foo)
        {
            this.foo = foo;
        }

        public bool isBar()
        {
            return bar;
        }

        public void setBar(bool bar)
        {
            this.bar = bar;
        }

    }
}