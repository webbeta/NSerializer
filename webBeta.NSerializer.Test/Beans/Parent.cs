namespace webBeta.NSerializer.Test.Beans
{
    public class Parent : Grandparent
    {
        private string parentField = "Hello parent!";

        public string getParentField()
        {
            return parentField;
        }

        public void setParentField(string parentField)
        {
            this.parentField = parentField;
        }
    }
}