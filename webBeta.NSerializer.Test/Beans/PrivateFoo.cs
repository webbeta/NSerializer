namespace webBeta.NSerializer.Test.Beans
{
    internal class PrivateFoo
    {
        private int id = 654;

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }
    }
}