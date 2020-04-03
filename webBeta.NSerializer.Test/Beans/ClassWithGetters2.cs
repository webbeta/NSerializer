namespace webBeta.NSerializer.Test.Beans
{
    public class ClassWithGetters2
    {
        private bool bar;
        private string foo;

        private int id;
        private string truncatedNameField = "Hello world!";

        public ClassWithGetters2()
        {
            id = 200;
            foo = "Hello world2!";
            bar = false;
        }

        public ClassWithGetters2(int id)
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

        public string getTruncatedNameField()
        {
            return truncatedNameField;
        }

        public void setTruncatedNameField(string truncatedNameField)
        {
            this.truncatedNameField = truncatedNameField;
        }
    }
}