namespace webBeta.NSerializer.Metadata.Model
{

    /**
 * It describes the customized getter method binded to a field.
 * <b>It requires that "access_type" config to be "public_method".</b>
     */
    public class MetadataPropertyAccessor
    {
        private readonly string getter;

        public MetadataPropertyAccessor(string getter)
        {
            this.getter = getter;
        }

        public bool hasGetter()
        {
            return getter != null;
        }

        public string getGetter()
        {
            return getter;
        }
    }
}