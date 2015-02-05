using System;
using System.Collections.Generic;

namespace RLToolkit.WidgetsTests
{
    public static class TestList
    {
        public static List<TestDefinition> bigList = new List<TestDefinition>();

        static TestList()
        {
            // Add all the tests from each module testLists.
            // example:  bigList.AddRange(new widget().listTests);
			
			TestList.bigList.AddRange(new OutputListTest().listTests);
			TestList.bigList.AddRange(new LabelTest().listTests);
            TestList.bigList.AddRange(new GridSelectorTest().listTests);
		}

		public static List<string> GetTestNames()
		{
			List<string> list = new List<string>();
			foreach (TestDefinition testDefinition in TestList.bigList)
			{
				list.Add(testDefinition.testName);
			}
			return list;
		}

		public static TestDefinition FindDefinition(string input)
		{
			foreach (TestDefinition testDefinition in TestList.bigList)
			{
				if (testDefinition.testName == input)
				{
					return testDefinition;
				}
			}
			return (TestDefinition) null;
		}
    }
}