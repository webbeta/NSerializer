namespace webBeta.NSerializer.Test.Beans
{
    public class Child : Parent
    {
        private string childField = "Hello child!";

        public string getChildField()
        {
            return childField;
        }

        public void setChildField(string childField)
        {
            this.childField = childField;
        }
    }
}