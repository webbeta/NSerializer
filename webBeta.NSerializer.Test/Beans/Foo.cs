using System.Collections.Generic;

namespace webBeta.NSerializer.Test.Beans
{

    public class Foo
    {

        private int id = 654;
        public string fooField = "Random string";
        public Bar bar = new Bar();
        private string truncatedNameField = "Hello world!";

        public List<string> list;
        public List<Bar> listOfBean;

        public string nullField = null;

        //public PrivateFoo privateFoo = new PrivateFoo();

        private Dictionary<string, double> map;

        private BeanWithWrongDefinedMetadata beanWithWrongMetadata;

        private List<Foo> recursiveList;

        private Dictionary<string, Foo> recursiveMap;

        public Foo()
        {
            list = new List<string> {"A", "B"};

            var bean1 = new Bar {id = 111};

            var bean2 = new Bar {id = 112};

            listOfBean = new List<Bar> {bean1, bean2};

            map = new Dictionary<string, double> {{"foo", 50.1}, {"bar", 60.12}};

            beanWithWrongMetadata = new BeanWithWrongDefinedMetadata();

            recursiveList = new List<Foo>();
            recursiveMap = new Dictionary<string, Foo>();
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public void addRecursiveListItem(Foo foo)
        {
            recursiveList.Add(foo);
        }

        public void addRecursiveMapItem(string key, Foo foo)
        {
            recursiveMap.Add(key, foo);
        }

    }
}