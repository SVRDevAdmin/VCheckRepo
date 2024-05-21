namespace VCheckViewerAPI.Models
{
    public class TestClass
    {
        public static List<TestObject> GetTestObject()
        {
            List<TestObject> sTestObject = new List<TestObject>
            {
                new TestObject { ID = 1, Column1 = "Test 123", Column2 = "Test 456" },
                new TestObject { ID = 2, Column1 = "Test 999", Column2 = "Test 987" }
            };

            return sTestObject;
        }
    }

    public class TestObject
    {
        public int ID { get; set; }
        public String Column1 { get; set; }
        public String Column2 { get; set; }
    }
}
