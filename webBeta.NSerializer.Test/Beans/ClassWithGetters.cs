namespace webBeta.NSerializer.Test.Beans
{

    public class ClassWithGetters
    {

        private int id;
        private string foo;
        private bool bar;

        private string barMethod;
        private string barMethod2;
        private string barMethod3;

        public ClassWithGetters()
        {
            id = 500;
            foo = "Hello world!";
            bar = true;
            barMethod = "Foo";
            barMethod2 = "Foo";
            barMethod3 = "Foo";
        }


        public ClassWithGetters(int id)
        {
            id = id;
            foo = "Hello world!";
            bar = true;
            barMethod = "Foo";
            barMethod2 = "Foo";
            barMethod3 = "Foo";
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

        public string getBarMethod()
        {
            return barMethod + " Bar";
        }

        public void setBarMethod(string barMethod)
        {
            this.barMethod = barMethod;
        }

        public string getBarMethod2()
        {
            return barMethod2;
        }

        public void setBarMethod2(string barMethod2)
        {
            this.barMethod2 = barMethod2;
        }

        public string getBarMethodBis()
        {
            return barMethod2 + " Bar Foo Bar";
        }

        public string getBarMethod3()
        {
            return barMethod3;
        }

        public void setBarMethod3(string barMethod3)
        {
            this.barMethod3 = barMethod3;
        }

        public string getBarMethod3Bis()
        {
            return barMethod3 + " Bar Foo Bar";
        }

        public string getGetterWithoutProperty()
        {
            return "I'm fake!";
        }

        public string getGetterWithoutProperty2()
        {
            return "I'm fake!";
        }

    }
}